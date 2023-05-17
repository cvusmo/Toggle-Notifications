using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DocumentFormat.OpenXml.Wordprocessing;
using HarmonyLib;
using KSP.Game;
using KSP.Messages.PropertyWatchers;
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
        public GameInstance game;
        private NotificationToggle notificationToggle;
        private bool solarPanelsIneffectiveMessageToggle;
        private bool gamePauseToggledMessageToggle;

        //config
        public ConfigEntry<bool> TNconfig;
        protected bool defaultValue;
        public static bool IsPatchingEnabled { get; set; }

        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        private static string assemblyFolder;
        private static string settingsPath;
        private static string AssemblyFolder => ToggleNotificationsPlugin.assemblyFolder ?? (ToggleNotificationsPlugin.assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static string SettingsPath => ToggleNotificationsPlugin.settingsPath ?? (ToggleNotificationsPlugin.settingsPath = Path.Combine(ToggleNotificationsPlugin.AssemblyFolder, "settings.json"));
        
        public static ToggleNotificationsPlugin Instance { get; private set; }
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

            // Configuration settings
            TNconfig = Config.Bind("Notification Settings", "Toggle Notifications", defaultValue, "Toggle Notifications is a mod that allows you to enable or disable notifications");
            defaultValue = TNconfig.Value;
            TNconfig.Value = true;

            ToggleNotificationsPlugin.Logger.LogInfo($"Toggle Notifications: {TNconfig.Value}");

            // Register Flight AppBar button
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

            // Register all Harmony patches in the project
            solarPanelsIneffectiveMessageToggle = true;
            gamePauseToggledMessageToggle = true;

            // Register all Harmony patches in the project
            AssistantToTheAssistantPatchManager.ApplyPatches();

            // Config buttons
            defaultValue = TNconfig.Value;
            if (defaultValue)
            {
                defaultValue = false;
                Debug.Log("Configuration is true");
            }
            else
            {
                defaultValue = true;
                Debug.Log("Configuration is false");
            }
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
        public void CheckCurrentState()
        {
            bool solarPanelsIneffectiveMessageToggle = notificationToggle.GetNotificationState(NotificationToggle.NotificationType.SolarPanelsIneffectiveMessage);
            bool gamePauseToggledMessageToggle = notificationToggle.GetNotificationState(NotificationToggle.NotificationType.GamePauseToggledMessage);
        }
        public void saverectpos()
        {
            TNBaseSettings.WindowXPos = (int)this.windowRect.xMin;
            TNBaseSettings.WindowYPos = (int)this.windowRect.yMin;
        }
        private void OnGUI()
        {
            isGUIVisible = false;
            var _gameState = Game?.GlobalGameState?.GetState();
            if (_gameState == GameState.Map3DView) isGUIVisible = true;
            if (_gameState == GameState.FlightView) isGUIVisible = true;
            if (interfaceEnabled && isGUIVisible && MainUI.RefreshState)
            {
                TNStyles.Init();
                WindowTool.CheckMainWindowPos(ref windowRect);
                GUI.skin = TNBaseStyle.Skin;

                windowRect = GUILayout.Window(
                    GUIUtility.GetControlID(FocusType.Passive),
                    windowRect,
                    FillWindow,
                    "<color=#696DFF>TOGGLE NOTIFICATIONS</color>",
                    GUILayout.Height(0),
                    GUILayout.Width(windowWidth));
                saverectpos();
                ToolTipsManager.DrawToolTips();
                UIFields.CheckEditor();
            }
        }
        public void FillWindow(int windowID)
        {
            ToggleButton(true);
            TopButtons.Init(windowRect.width);
            GUI.Label(new Rect(9f, 2f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            if (TopButtons.Button(TNBaseStyle.Cross))
                CloseWindow();
            //this.MainUI.OnGUI();
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));
        }
        public void CloseWindow()
        {
            ToggleButton(false);
            interfaceEnabled = false;
            UIFields.GameInputState = true;
        }
    }
}