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
                if (this.MainUI == null)
                    return;
                this.MainUI.Update();
            }
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
            WindowTool.CheckMainWindowPos(ref windowRect, windowWidth);
            GUI.skin = TNBaseStyle.Skin;

            this.windowRect = GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                this.windowRect,
                new GUI.WindowFunction(this.FillWindow),
                "<color=#696DFF>TOGGLE NOTIFICATIONS</color>",
                GUILayout.Height(0.0f),
                GUILayout.Width((float)this.windowWidth),
                GUILayout.MinHeight(200) // Adjust the value to your desired height
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
            // Initialize the position of the buttons
            TopButtons.Init(this.windowRect.width);

            GUILayout.BeginHorizontal();

            // MENU BAR
            GUI.Label(new Rect(10f, 2f, 29f, 29f), TNBaseStyle.Icon, TNBaseStyle.IconsLabel);

            Rect closeButtonPosition = new Rect(this.windowRect.width - 10, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);

            if (TopButtons.Button(TNBaseStyle.Cross))
                this.CloseWindow();

            GUILayout.Space(10);

            if (TopButtons.Button(TNBaseStyle.Gear))
            {
                // Handle the gear button action here if needed
            }
            GUILayout.EndHorizontal();

            // Radio Buttons
            GUILayout.BeginVertical();
            // Set the height of the vertical group based on the available space in the window
            float verticalGroupHeight = this.windowRect.height - GUILayoutUtility.GetLastRect().height - 10f;
            GUILayout.BeginArea(new Rect(0f, GUILayoutUtility.GetLastRect().yMax, this.windowRect.width, verticalGroupHeight));
            bool radioButton1 = GUI.Toggle(new Rect(10, 40, 120, 20), selectedRadioButton == 1, "Enable", TNBaseStyle.ToggleRadio);
            if (radioButton1)
            {
                selectedRadioButton = 1;
            }

            bool radioButton2 = GUI.Toggle(new Rect(10, 90, 120, 20), selectedRadioButton == 2, "Disable", TNBaseStyle.ToggleRadio);
            if (radioButton2)
            {
                selectedRadioButton = 2;
            }
            GUILayout.EndArea();
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));

            // Save the window position
            saverectpos();
        }
        public void CloseWindow()
        {
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);
            interfaceEnabled = false;
            ToggleButton(false, false);
            Rect closeButtonPosition = new Rect(windowRect.width - 30, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);
        }
    }
}
