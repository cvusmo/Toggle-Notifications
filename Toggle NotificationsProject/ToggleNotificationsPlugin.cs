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
        public bool isWindowOpen;
        public bool isGUIVisible = false;
        private Rect windowRect = Rect.zero;
        private int windowWidth = 250;
        public ToggleNotificationsUI MainUI;
        public ToggleNotificationsPlugin mainPlugin;
        public GameInstance game;
        private NotificationToggle notificationToggle;
        //config
        public ConfigEntry<bool> TNconfig;
        protected bool defaultValue;
        public ConfigEntry<bool> SolarToggleConfig { get; private set; }
        public ConfigEntry<bool> PauseToggleConfig { get; private set; }
        //appbar
        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        private static string assemblyFolder;
        private static string settingsPath;
        private SettingsFile settingsFile;
        private static string AssemblyFolder => assemblyFolder ?? (assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static string SettingsPath => settingsPath ?? (settingsPath = Path.Combine(AssemblyFolder, "settings.json"));

        public static ToggleNotificationsPlugin Instance { get; private set; }
        public new static ManualLogSource Logger { get; set; }

        public void Awake()
        {
            TNStyles.Init();
        }
        public override void OnInitialized()
        {
            // initialize
            base.OnInitialized();
            TNBaseSettings.Init(SettingsPath);
            // config initialize
            Dictionary<NotificationType, bool> initialNotificationStates = new Dictionary<NotificationType, bool>();
            SolarToggleConfig = Config.Bind("Notification Settings", "Solar Config", true, "Solar configuration value");
            PauseToggleConfig = Config.Bind("Notification Settings", "Pause Toggle State Config", true, "Game Pause Toggle State configuration value");
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
                AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"),
                isOpen =>
                {
                    ToggleButton(isOpen, isOpen); // Update GUI visibility and window open state
                    Debug.Log($"Initial isWindowOpen value: {isWindowOpen}");
                }
            );

            // Set initial GUI visibility and window open state based on the appbar button value
            isWindowOpen = GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.GetValue() ?? false;

            // configuration
            TNconfig = Config.Bind("Notification Settings", "Toggle Notifications", defaultValue, "Toggle Notifications is a mod that allows you to enable or disable notifications");
            defaultValue = TNconfig.Value;
            TNconfig.Value = true;
            notificationToggle = new NotificationToggle(this, new Dictionary<NotificationType, bool>()
            {
                [NotificationType.GamePauseToggledMessage] = PauseToggleConfig.Value,
                [NotificationType.PauseStateChangedMessageToggle] = PauseToggleConfig.Value,
                [NotificationType.SolarPanelsIneffectiveMessage] = SolarToggleConfig.Value
            });
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
        private void OnGUI()
        {
            if (!isGUIVisible)
                return;

            WindowTool.CheckMainWindowPos(ref windowRect, windowWidth);
            GUI.skin = TNBaseStyle.Skin;
            GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                windowRect,
                FillWindow,
                "<color=#696DFF>TOGGLE NOTIFICATIONS</color>",
                GUILayout.Height(0),
                GUILayout.Width(windowWidth)
            );

            saverectpos();
        }

        private int selectedRadioButton = 1; // Start with "on" selected
        private void RadioButtonToggle(int toggleValue)
        {
            selectedRadioButton = toggleValue;

            if (selectedRadioButton == 1)
            {
                notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, true);
                notificationToggle.CheckCurrentState(NotificationType.PauseStateChangedMessageToggle, true);
            }
            else if (selectedRadioButton == 0)
            {
                notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
                notificationToggle.CheckCurrentState(NotificationType.PauseStateChangedMessageToggle, false);
            }
        }
        public void FillWindow(int windowID)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(AssetsLoader.LoadIcon("logo_icon"), GUIStyle.none, GUILayout.Width(24), GUILayout.Height(24));
            GUILayout.Label("Toggle Notifications v0.2.1", TNBaseStyle.Label);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(selectedRadioButton == 1, AssetsLoader.LoadIcon("Toggle_On"), GUIStyle.none))
            {
                RadioButtonToggle(1);
            }
            if (GUILayout.Toggle(selectedRadioButton == 0, AssetsLoader.LoadIcon("Toggle_Off"), GUIStyle.none))
            {
                RadioButtonToggle(0);
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));
        }

        public void CloseWindow()
        {
            isGUIVisible = false;
        }
    }
}
