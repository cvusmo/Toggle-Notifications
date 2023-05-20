using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KSP.Game;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI.Appbar;
using System.Drawing;
using System.Reflection;
using ToggleNotifications.TNTools;
using ToggleNotifications.TNTools.UI;
using UnityEngine;
using Color = UnityEngine.Color;

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
                GUILayout.MinHeight(400) // Adjust the value to your desired height
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
                PauseToggleConfig.Value = true;
            }
            else if (selectedRadioButton == 0)
            {
                notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
                notificationToggle.CheckCurrentState(NotificationType.PauseStateChangedMessageToggle, false);
                PauseToggleConfig.Value = false;
            }
        }
        public void FillWindow(int windowID)
        {
            // Initialize the position of the buttons
            TopButtons.Init(this.windowRect.width);

            GUILayout.BeginHorizontal();

            // MENU BAR
            GUILayout.FlexibleSpace();

            GUI.Label(new Rect(10f, 2f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            Rect closeButtonPosition = new Rect(this.windowRect.width - 10, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);

            if (TopButtons.Button(TNBaseStyle.Cross))
                this.CloseWindow();

            GUILayout.Space(10);

            if (TopButtons.Button(TNBaseStyle.Gear))
            {
                // Handle the gear button action here if needed
            }
            //GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);
            GUILayout.EndHorizontal();

            
            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);
           

            // Notification Toggle Buttons
            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            //version number
            GUIStyle nameLabelStyle = new GUIStyle()
            {
                border = new RectOffset(3, 3, 5, 5),
                padding = new RectOffset(3, 3, 4, 4),
                overflow = new RectOffset(0, 0, 0, 0),
                normal = { textColor = ColorTools.ParseColor("#C0C1E2") },
                alignment = TextAnchor.MiddleRight
            };
 
            GUILayout.Label("v0.2.2", nameLabelStyle, GUILayout.Height(10));

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            //notifications

            float verticalGroupHeight = this.windowRect.height - GUILayoutUtility.GetLastRect().height - 10f;
            GUILayout.BeginArea(new Rect(0f, GUILayoutUtility.GetLastRect().yMax, this.windowRect.width, verticalGroupHeight));

            GUIStyle gamePauseStyle = new GUIStyle()
            {
                border = new RectOffset(3, 3, 5, 5),
                padding = new RectOffset(3, 3, 4, 4),
                overflow = new RectOffset(0, 0, 0, 0),
                normal = { textColor = ColorTools.ParseColor("#C0C1E2") },
                alignment = TextAnchor.MiddleLeft
            };

            GUILayout.Label("Pause Notification", gamePauseStyle, GUILayout.Height(10));


            bool radioButton1 = GUI.Toggle(new Rect(this.windowRect.width - 140, 70, 120, 20), selectedRadioButton == 1, "Enable", TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedRadioButton == 1 ? ColorTools.ParseColor("#C0E2DC") : ColorTools.ParseColor("#C0C1E2");
            if (radioButton1)
            {
                selectedRadioButton = 1;
            }

            bool radioButton2 = GUI.Toggle(new Rect(this.windowRect.width - 140, 110, 120, 20), selectedRadioButton == 2, "Disable", TNBaseStyle.ToggleError);
            TNBaseStyle.ToggleError.normal.textColor = selectedRadioButton == 2 ? ColorTools.ParseColor("#C0E2DC") : Color.red;
            if (radioButton2)
            {
                selectedRadioButton = 2;
            }

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            GUILayout.EndArea();
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));

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
