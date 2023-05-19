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
        public bool interfaceEnabled;
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
                AssetManager.GetAsset<Texture2D>($"{this.SpaceWarpMetadata.ModID}/images/icon.png"),
                isOpen =>
                {
                    ToggleButton(isOpen, isOpen);
                    Debug.Log($"Initial isWindowOpen value: {interfaceEnabled}");
                }
            );

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
            AssistantToTheAssistantPatchManager.ApplyPatches(notificationToggle);
        }

        public void ToggleButton(bool toggle, bool isOpen)
        {
            interfaceEnabled = isOpen;
            isGUIVisible = toggle;
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isGUIVisible);
        }
        public void Update()
        {
            if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.P))
            {
                this.ToggleButton(!this.interfaceEnabled, !this.isGUIVisible);
                Logger.LogInfo("Update: Toggle Notifications UI toggled with hotkey");
            }
            if (isGUIVisible)
            {
                MainUI.OnGUI();
            }
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

            TNStyles.Init();
            Texture2D windowTexture = AssetsLoader.LoadIcon("window");
            WindowTool.CheckMainWindowPos(ref windowRect, windowWidth);
            GUI.skin = TNBaseStyle.Skin;
            this.windowRect = GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                this.windowRect,
                new GUI.WindowFunction(this.FillWindow),
                "<color=#696DFF>TOGGLE NOTIFICATIONS</color>",
                GUILayout.Height(0.0f),
                GUILayout.Width((float)this.windowWidth)
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
            TopButtons.Init(this.windowRect.width);
            GUI.Label(new Rect(9f, 2f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            if (TopButtons.Button(TNBaseStyle.Cross))
                this.CloseWindow();
            GUILayout.Space(10);

            if (GUILayout.Toggle(selectedRadioButton == 1, AssetsLoader.LoadIcon("Toggle_On"), GUIStyle.none))
            {
                RadioButtonToggle(1);
            }
            if (GUILayout.Toggle(selectedRadioButton == 0, AssetsLoader.LoadIcon("Toggle_Off"), GUIStyle.none))
            {
                RadioButtonToggle(0);
            }

            MainUI.OnGUI();
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));
        }

        public void CloseWindow()
        {
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);
            interfaceEnabled = false;
            ToggleButton(false, false);
        }
    }
}
