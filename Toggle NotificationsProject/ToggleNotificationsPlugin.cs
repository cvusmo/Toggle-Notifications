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
        internal bool interfaceEnabled;
        internal bool isGUIVisible = false;
        private Rect windowRect = Rect.zero;
        private int windowWidth = 250;
        internal ToggleNotificationsUI MainUI;
        internal ToggleNotificationsPlugin mainPlugin;
        internal GameInstance game;
        private NotificationToggle notificationToggle;
        //config
        public ConfigEntry<bool> tnConfig;
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

        internal static ToggleNotificationsPlugin Instance { get; private set; }
        internal new static ManualLogSource Logger { get; set; }
        public override void OnInitialized()
        {
            TNBaseSettings.Init(SettingsPath);

            SolarToggleConfig = Config.Bind("Notification Settings", "Solar Config", true, "Solar configuration value");
            PauseToggleConfig = Config.Bind("Notification Settings", "Pause Toggle State Config", true, "Game Pause Toggle State configuration value");

            base.OnInitialized();
            
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
            tnConfig = Config.Bind("Notification Settings", "Toggle Notifications", defaultValue, "Toggle Notifications is a mod that allows you to enable or disable notifications");
            defaultValue = tnConfig.Value;
            tnConfig.Value = true;

            notificationToggle = new NotificationToggle(this, new Dictionary<NotificationType, bool>()
            {
                [NotificationType.GamePauseToggledMessage] = PauseToggleConfig.Value,
                [NotificationType.PauseStateChangedMessageToggle] = PauseToggleConfig.Value,
                [NotificationType.SolarPanelsIneffectiveMessage] = SolarToggleConfig.Value
            });
            AssistantToTheAssistantPatchManager.ApplyPatches(notificationToggle);
        }


        internal void ToggleButton(bool toggle, bool isOpen)
        {
            interfaceEnabled = isOpen;
            isGUIVisible = toggle;
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isGUIVisible);
        }
        internal void Update()
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
        internal void saverectpos()
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

        private int selectedRadioButton2 = 1;
        private void RadioButtonToggle2(int toggleValue)
        {
            selectedRadioButton2 = toggleValue;

            if (selectedRadioButton2 == 1)
            {
                notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, true);
                SolarToggleConfig.Value = true;
            }
            else if (selectedRadioButton2 == 0)
            {
                notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
                SolarToggleConfig.Value = false;
            }
        }

        internal void FillWindow(int windowID)
        {
            // Initialize the position of the buttons
            TopButtons.Init(this.windowRect.width);

            GUILayout.BeginHorizontal();

            // MENU BAR
            GUILayout.FlexibleSpace();

            //GUI.Label(new Rect(10f, 2f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            GUI.Label(new Rect(10f, 4f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
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

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            // Notification Toggle Buttons
            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            // Group 1: Pause Notification
            GUILayout.BeginVertical();

            GUIStyle gamePauseStyle = new GUIStyle()
            {
                border = new RectOffset(3, 3, 5, 5),
                padding = new RectOffset(3, 3, 4, 4),
                overflow = new RectOffset(0, 0, 0, 0),
                normal = { textColor = ColorTools.ParseColor("#C0C1E2") },
                alignment = TextAnchor.UpperLeft
            };
            GUILayout.Label("Pause Notification", gamePauseStyle, GUILayout.Height(68));

            GUIStyle solarStyle = new GUIStyle()
            {
                border = new RectOffset(3, 3, 5, 5),
                padding = new RectOffset(3, 3, 4, 4),
                overflow = new RectOffset(0, 0, 0, 0),
                normal = { textColor = ColorTools.ParseColor("#C0C1E2") },
                alignment = TextAnchor.UpperLeft
            };
            GUILayout.Label("Solar Panels Ineffective", solarStyle, GUILayout.Height(151));

            GUILayout.EndVertical();

            // Group 2: Toggle Buttons
            GUILayout.BeginVertical(GUILayout.Height(60));

            bool radioButton1 = GUI.Toggle(new Rect(this.windowRect.width - 140, 53, 120, 20), selectedRadioButton == 1, "Enable", TNBaseStyle.ToggleError);
            TNBaseStyle.ToggleError.normal.textColor = selectedRadioButton == 0 ? ColorTools.ParseColor("#C0E2DC") : Color.red;
            TNBaseStyle.Toggle.normal.textColor = selectedRadioButton == 1 ? ColorTools.ParseColor("#C0E2DC") : ColorTools.ParseColor("#C0C1E2");
            if (radioButton1)
            {
                selectedRadioButton = 1;
            }

            bool radioButton2 = GUI.Toggle(new Rect(this.windowRect.width - 140, 93, 120, 20), selectedRadioButton == 0, "Disable", TNBaseStyle.ToggleError);
            TNBaseStyle.ToggleError.normal.textColor = selectedRadioButton == 0 ? ColorTools.ParseColor("#C0E2DC") : Color.red;
            TNBaseStyle.Toggle.normal.textColor = selectedRadioButton == 1 ? ColorTools.ParseColor("#C0E2DC") : ColorTools.ParseColor("#C0C1E2");
            if (radioButton2)
            {
                selectedRadioButton = 0;
            }

            bool radioButton3 = GUI.Toggle(new Rect(this.windowRect.width - 140, 133, 120, 20), selectedRadioButton2 == 1, "Enable", TNBaseStyle.ToggleError);
            TNBaseStyle.ToggleError.normal.textColor = selectedRadioButton2 == 0 ? ColorTools.ParseColor("#C0E2DC") : Color.red;
            TNBaseStyle.Toggle.normal.textColor = selectedRadioButton2 == 1 ? ColorTools.ParseColor("#C0E2DC") : ColorTools.ParseColor("#C0C1E2");
            if (radioButton3)
            {
                selectedRadioButton2 = 1;
            }

            bool radioButton4 = GUI.Toggle(new Rect(this.windowRect.width - 140, 173, 120, 20), selectedRadioButton2 == 0, "Disable", TNBaseStyle.ToggleError);
            TNBaseStyle.ToggleError.normal.textColor = selectedRadioButton2 == 0 ? ColorTools.ParseColor("#C0E2DC") : Color.red;
            TNBaseStyle.Toggle.normal.textColor = selectedRadioButton2 == 1 ? ColorTools.ParseColor("#C0E2DC") : ColorTools.ParseColor("#C0C1E2");
            if (radioButton4)
            {
                selectedRadioButton2 = 0;
            }

            GUILayout.EndVertical();


            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            // Group 3: Version Number
            GUIStyle nameLabelStyle = new GUIStyle()
            {
                border = new RectOffset(3, 3, 5, 5),
                padding = new RectOffset(3, 3, 4, 4),
                overflow = new RectOffset(0, 0, 0, 0),
                normal = { textColor = ColorTools.ParseColor("#C0C1E2") },
                alignment = TextAnchor.MiddleRight
            };

            GUILayout.FlexibleSpace();

            GUILayout.Label("v0.2.2", nameLabelStyle);

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));

            saverectpos();
        }
        internal void CloseWindow()
        {
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);
            interfaceEnabled = false;
            ToggleButton(false, false);
            Rect closeButtonPosition = new Rect(windowRect.width - 30, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);
        }
    }
}