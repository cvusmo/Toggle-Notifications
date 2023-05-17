using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI;
using SpaceWarp.API.UI.Appbar;
using System.Reflection;
using ToggleNotifications.TNTools;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
    public class ToggleNotificationsPlugin : BaseSpaceWarpPlugin
    {
        // These are useful in case some other mod wants to add a dependency to this one
        public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
        public const string ModName = MyPluginInfo.PLUGIN_NAME;
        public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

        private bool isWindowOpen;
        private bool interfaceEnabled;
        private bool isGUIVisible = true;
        private bool toggleState;
        private Rect windowRect = Rect.zero;
        private int windowWidth = 250;
        public ToggleNotificationsUI MainUI;
        public ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;
        public GameInstance game;
        private bool solarPanelsIneffectiveMessageToggle = true;
        private bool gamePauseToggledMessageToggle = true;

        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        private static string assemblyFolder;
        private static string settingsPath;
        private static string AssemblyFolder => ToggleNotificationsPlugin.assemblyFolder ?? (ToggleNotificationsPlugin.assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static string SettingsPath => ToggleNotificationsPlugin.settingsPath ?? (ToggleNotificationsPlugin.settingsPath = Path.Combine(ToggleNotificationsPlugin.AssemblyFolder, "settings.json"));
        public void CheckCurrentState()
        {
            bool solarPanelsIneffectiveMessageToggle = notificationToggle.GetNotificationState(NotificationToggle.NotificationType.SolarPanelsIneffectiveMessage);
            bool gamePauseToggledMessageToggle = notificationToggle.GetNotificationState(NotificationToggle.NotificationType.GamePauseToggledMessage);
        }
        public static ToggleNotificationsPlugin Instance { get; set; }
        public new static ManualLogSource Logger { get; set; }
        public override void OnInitialized()
        {
            base.OnInitialized();

            TNBaseSettings.Init(ToggleNotificationsPlugin.SettingsPath);

            Instance = this;
            MainUI = new ToggleNotificationsUI(this);
            ToggleNotificationsPlugin.Instance = this;
            game = GameManager.Instance.Game;

            Logger = base.Logger;
            Logger.LogInfo("Loaded");

            //Register Flight AppBar button
            Appbar.RegisterAppButton(
            "Toggle Notifications",
            ToolbarFlightButtonID,
            AssetManager.GetAsset<Texture2D>($"togglenotifications/images/icon.png"),
            isOpen =>
            {
                isWindowOpen = isOpen;
                GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isOpen);
                Debug.Log($"Initial isWindowOpen value: {isWindowOpen}");
            }
        );

            // Initialize and pass the notification toggle instance to the patch manager
            var notificationToggle = new NotificationToggle(this, new Dictionary<NotificationToggle.NotificationType, bool>());
            AssistantToTheAssistantPatchManager.NotificationToggle = notificationToggle;


            solarPanelsIneffectiveMessageToggle = true;
            gamePauseToggledMessageToggle = true;

            // Register all Harmony patches in the project
            AssistantToTheAssistantPatchManager.ApplyPatches();

            // Fetch a configuration value or create a default one if it does not exist
            var defaultValue = "Enabled";
            var configValue = Config.Bind<string>("Settings section", "Option 1", defaultValue, "Option description");

            // Log the config value into <KSP2 Root>/BepInEx/LogOutput.log
            ToggleNotificationsPlugin.Logger.LogInfo($"Option 1: {configValue.Value}");
        }
        public void ToggleButton(bool toggle)
        {
            interfaceEnabled = toggle;
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(this.interfaceEnabled);
        }
        public void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                this.ToggleButton(!this.interfaceEnabled);
                ToggleNotificationsPlugin.Logger.LogInfo((object)"Update: Toggle Notifications UI toggled with hotkey");
            }
            if (this.MainUI == null)
                return;

            this.MainUI.Update();

        }
        public NotificationToggle GetNotificationToggle()
        {
            return notificationToggle;
        }

        public void saverectpos()
        {
            TNBaseSettings.WindowXPos = (int)this.windowRect.xMin;
            TNBaseSettings.WindowYPos = (int)this.windowRect.yMin;
        }
        public void OnGUI()
        {
            isGUIVisible = false;
            GUI.skin = Skins.ConsoleSkin;

            if (isWindowOpen)
            {

                // Draw the window using TNBaseStyle
                windowRect = GUILayout.Window(
                    0,
                    windowRect,
                    FillWindow,
                    "Toggle Notifications",
                    GUILayout.Height(350),
                    GUILayout.Width(windowWidth)
                );

                isGUIVisible = true;
                TNStyles.Init();
                WindowTool.CheckWindowPos(ref this.windowRect);
                GUI.skin = ScriptableObject.CreateInstance<GUISkin>(); ;
                windowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive),
                windowRect,
                new GUI.WindowFunction(FillWindow),
                "<color=#696DFF>TOGGLE NOTIFICATIONS</color>");
                GUILayout.Height(0.0f);
                GUILayout.Width((float)this.windowWidth);
            }
        }
        public void FillWindow(int windowID)
        {
            TopButtons.Init(windowRect.width);
            GUI.Label(new Rect(9f, 2f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            if (TopButtons.Button(TNBaseStyle.Cross))
                this.CloseWindow();
            this.toggleState = isGUIVisible;
            this.MainUI.OnGUI();
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));
        }
        public void CloseWindow()
        {
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);
            this.interfaceEnabled = false;
            this.ToggleButton(this.interfaceEnabled);
            UIFields.GameInputState = true;
        }
    }
}