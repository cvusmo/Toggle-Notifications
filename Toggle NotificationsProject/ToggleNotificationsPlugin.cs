using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KSP.Game;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
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
        //modinfo
        public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
        public const string ModName = MyPluginInfo.PLUGIN_NAME;
        public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

        //core
        private bool isWindowOpen;
        public bool isGUIVisible = true;
        private Rect windowRect = Rect.zero;
        private int windowWidth = 250;
        public ToggleNotificationsUI MainUI;
        public ToggleNotificationsPlugin mainPlugin;
        public GameInstance game;
        private NotificationToggle notificationToggle;

        //config
        public ConfigEntry<bool> TNconfig;
        protected bool defaultValue;

        //appbar
        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        private static string assemblyFolder;
        private static string settingsPath;
        private SettingsFile settingsFile;

        private static string AssemblyFolder => assemblyFolder ?? (assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static string SettingsPath => settingsPath ?? (settingsPath = Path.Combine(AssemblyFolder, "settings.json"));

        public static ToggleNotificationsPlugin Instance { get; private set; }
        public new static ManualLogSource Logger { get; set; }

        public override void OnInitialized()
        {
            //initialize
            base.OnInitialized();
            TNBaseSettings.Init(SettingsPath);
            Instance = this;
            Logger = base.Logger;
            Logger.LogInfo("Loaded");
            MainUI = new ToggleNotificationsUI(this, isGUIVisible);
            Debug.Log("MainUI instantiated");
            game = GameManager.Instance.Game;

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

            //configuration
            TNconfig = Config.Bind("Notification Settings", "Toggle Notifications", defaultValue, "Toggle Notifications is a mod that allows you to enable or disable notifications");
            defaultValue = TNconfig.Value;
            TNconfig.Value = true;

            // Register Harmony patches
            AssistantToTheAssistantPatchManager.ApplyPatches();
        }

        public void ToggleButton(bool toggle, bool isOpen)
        {
            isGUIVisible = toggle;
            isWindowOpen = isOpen;
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isGUIVisible);
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.P))
            {
                this.ToggleButton(!this.isGUIVisible, !this.isWindowOpen);
                Logger.LogInfo("Update: Toggle Notifications UI toggled with hotkey");
            }
            MainUI?.Update();
        }
        public void LogCurrentState()
        {
            bool solarPanelsIneffectiveMessageToggle = notificationToggle.GetNotificationState(NotificationType.SolarPanelsIneffectiveMessage);
            bool gamePauseToggledMessageToggle = notificationToggle.GetNotificationState(NotificationType.GamePauseToggledMessage);
            bool pauseStateChangedMessageToggle = notificationToggle.GetNotificationState(NotificationType.PauseStateChangedMessageToggle);

            Logger.LogInfo($"Solar Panels Ineffective Message Toggle: {solarPanelsIneffectiveMessageToggle}");
            Logger.LogInfo($"Game Pause Toggled Message Toggle: {gamePauseToggledMessageToggle}");
            Logger.LogInfo($"Game Pause State Toggled Message Toggle: {pauseStateChangedMessageToggle}");
        }

        public void saverectpos()
        {
            TNBaseSettings.WindowXPos = (int)windowRect.xMin;
            TNBaseSettings.WindowYPos = (int)windowRect.yMin;
        }

        public void OnGUI()
        {
            GameState? gameState = BaseSpaceWarpPlugin.Game?.GlobalGameState?.GetState();

            if (gameState == GameState.FlightView || gameState == GameState.Map3DView)
            {
                this.isGUIVisible = true;
            }

            TNStyles.Init();
            WindowTool.CheckMainWindowPos(ref windowRect, windowWidth);
            GUI.skin = TNBaseStyle.Skin;
            windowRect = GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                windowRect,
                FillWindow,
                "<color=#696DFF>TOGGLE NOTIFICATIONS</color>"
            );

            saverectpos();
            ToggleButton(true, true);
        }

        public void FillWindow(int windowID)
        {
            GUI.Label(new Rect(9f, 2f, 29f, 29f), TNBaseStyle.Icon, TNBaseStyle.IconsLabel);

            if (TopButtons.Button(TNBaseStyle.Cross))
                CloseWindow();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));
        }

        public void CloseWindow()
        {
            isGUIVisible = false;
        }
    }
}