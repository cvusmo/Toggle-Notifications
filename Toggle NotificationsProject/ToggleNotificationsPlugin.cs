using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
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
        public ToggleNotificationsUI ToggleNotificationsUI { get; set; }
        public ToggleNotificationsUI MainUI;

        private List<NotificationUIAlert> Alerts = new List<NotificationUIAlert>();
        public NotificationToggle notificationToggle;
        public List<string> GetNotificationList;
        public List<string> SetNotificationList;

        public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
        public const string ModName = MyPluginInfo.PLUGIN_NAME;
        public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

        //config setup
        internal ConfigEntry<bool> _enableConfig;
        internal ConfigEntry<bool> SolarPanelIneffectiveMessageConfig;
        internal ConfigEntry<bool> CommunicationRangeMessageConfig;
        internal ConfigEntry<bool> ThrottleLockedWarpMessageConfig;
        internal ConfigEntry<bool> ManeuverNodeOutOfFuelMessageConfig;
        internal ConfigEntry<bool> GamePauseToggledMessageConfig;
        public bool currentState;

        //GUI related
        private bool isWindowOpen;
        private bool isGUIVisible = true;
        public Rect windowRect = Rect.zero;
        private int windowWidth = 250;
        public bool Loaded = false;
        public GameInstance game;
        public bool inputFields;
        private static GUIStyle boxStyle;
        private static int selectedItem = 0;

        //the vars volta
        public bool popoutSettings, popoutPar, popoutOrb, popoutSur, popoutMan, popoutTgt, popoutFlt, popoutStg;
        public Rect mainGuiRect, settingsGuiRect, parGuiRect, orbGuiRect, surGuiRect, fltGuiRect, manGuiRect, tgtGuiRect, stgGuiRect;
        public bool solarPanelState, commRangeState, throttleLockedWarpState, maneuverNodeOutOfFuelState, pauseToggleState;

        //appbar info
        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        //private const string ToolbarOABButtonID = "BTN-ToggleNotificationsOAB";

        //settingsfile and assembly
        private static string settingspath;
        private static string assemblyFolder;
        private static string AssemblyFolder => ToggleNotificationsPlugin.assemblyFolder ?? (ToggleNotificationsPlugin.assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static string SettingsPath => ToggleNotificationsPlugin.settingspath ?? (ToggleNotificationsPlugin.settingspath = Path.Combine(ToggleNotificationsPlugin.AssemblyFolder, "settings.json"));

        public new static ManualLogSource Logger { get; set; }
        public static ToggleNotificationsPlugin Instance { get; private set; }
        private void Awake()
        {
            // Initialize the configuration entries
            _enableConfig = Config.Bind("General", "EnableConfig", true, "Toggle overall notifications enable state");
            SolarPanelIneffectiveMessageConfig = Config.Bind("Notifications", "SolarPanelIneffectiveMessage", true, "Toggle Solar Panel Ineffective message");
            CommunicationRangeMessageConfig = Config.Bind("Notifications", "CommunicationRangeMessage", true, "Toggle Communication Range message");
            ThrottleLockedWarpMessageConfig = Config.Bind("Notifications", "ThrottleLockedWarpMessage", true, "Toggle Throttle Locked Warp message");
            ManeuverNodeOutOfFuelMessageConfig = Config.Bind("Notifications", "ManeuverNodeOutOfFuelMessage", true, "Toggle Maneuver Node Out Of Fuel message");
            GamePauseToggledMessageConfig = Config.Bind("Notifications", "GamePauseToggledMessage", true, "Toggle Game Pause Toggled message");

            // Access the configuration entry values
            bool enableConfigValue = _enableConfig.Value;
            bool solarPanelIneffectiveMessageValue = SolarPanelIneffectiveMessageConfig.Value;
            bool communicationRangeMessageValue = CommunicationRangeMessageConfig.Value;
            bool throttleLockedWarpMessageValue = ThrottleLockedWarpMessageConfig.Value;
            bool maneuverNodeOutOfFuelMessageValue = ManeuverNodeOutOfFuelMessageConfig.Value;
            bool gamePauseToggledMessageValue = GamePauseToggledMessageConfig.Value;


        }
        public override void OnInitialized()
        {
            base.OnInitialized();

            TNBaseSettings.Init(SettingsPath);

            MainUI = new ToggleNotificationsUI(this);
            Instance = this;

            //bepinex log
            game = GameManager.Instance.Game;
            Logger = base.Logger;

            Logger.LogInfo("Loaded");
            Debug.Log($"Initial currentState value: {currentState}"); // Add debug logs to verify the value of currentState

            //Register Flight AppBar button
            Appbar.RegisterAppButton(
            "Toggle Notifications",
            ToolbarFlightButtonID,
            AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"),
            isOpen =>
            {
                isWindowOpen = isOpen;
                GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isOpen);
            }
        );

            // Harmony creates the mainPlugin/patch
            Harmony harmony = new Harmony(ModGuid);
            harmony.PatchAll(typeof(AssistantToTheAssistantPatchManager));
            //Harmony.CreateAndPatchAll(typeof(ToggleNotificationsPlugin).Assembly);

            currentState = true; // Update the currentState variable to true
            Logger.LogInfo($"Current State: {currentState}");
        }
        public void ToggleButton(bool toggle)
        {
            isGUIVisible = toggle;
        }
        private void saverectpos()
        {
            TNBaseSettings.WindowXPos = (int)this.windowRect.xMin;
            TNBaseSettings.WindowYPos = (int)this.windowRect.yMin;
        }
        public Dictionary<NotificationToggle.NotificationType, bool> ActualState()
        {
            var state = new Dictionary<NotificationToggle.NotificationType, bool>();

            foreach (NotificationToggle.NotificationType type in Enum.GetValues(typeof(NotificationToggle.NotificationType)))
            {
                if (type == NotificationToggle.NotificationType.None)
                    continue;

                bool isEnabled = notificationToggle.GetNotificationState(type);

                state[type] = isEnabled;
            }

            return state;
        }
        public Dictionary<NotificationToggle.NotificationType, bool> CurrentState
        {
            get
            {
                var currentState = new Dictionary<NotificationToggle.NotificationType, bool>();

                foreach (NotificationToggle.NotificationType type in Enum.GetValues(typeof(NotificationToggle.NotificationType)))
                {
                    if (type == NotificationToggle.NotificationType.None) continue;

                    bool isRefreshing = false;
                    bool isRefreshingNotification = false;

                    // Code to determine isRefreshing and isRefreshingNotification

                    currentState[type] = isRefreshing || isRefreshingNotification;
                }

                return currentState;
            }
        }
        public void SetNotificationState(NotificationToggle.NotificationType notificationType, bool currentState)
        {
            notificationToggle.SetNotificationState(notificationType, currentState);
        }
        public void Update()
        {
            if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.P))
            {
                this.ToggleButton(!isGUIVisible);
                ToggleNotificationsPlugin.Logger.LogInfo((object)"Update: UI toggled with hotkey");
            }

        }
        public void InitializeRects()
        {
            mainGuiRect = settingsGuiRect = parGuiRect = orbGuiRect = surGuiRect = fltGuiRect = manGuiRect = tgtGuiRect = stgGuiRect = new Rect();
        }
        public void OnGUI()
        {
            Debug.Log("OnGUI is being called!");
            WindowTool.CheckMainWindowPos(ref windowRect);
            GUI.skin = TNBaseStyle.Skin;

            if (isWindowOpen)
            {

                windowRect = GUILayout.Window(
                    GUIUtility.GetControlID(FocusType.Passive),
                    windowRect,
                    FillWindow,
                    "<color=#696DFF>Toggle Notifications</color>",
                    GUILayout.Height(350f),
                    GUILayout.Width((float)windowWidth)
                    );

                saverectpos();
                ToolTipsManager.DrawToolTips();
                UIFields.CheckEditor();

                var currentState = CurrentState;

                foreach (var kvp in currentState)
                {
                    NotificationToggle.NotificationType notificationType = kvp.Key;
                    bool isEnabled = kvp.Value;

                    bool enableNotification = GUILayout.Toggle(isEnabled, $"Enable {notificationType.ToString()} Notification");
                    notificationToggle.SetNotificationState(notificationType, enableNotification);
                }
                Debug.Log("OnGUI is being called!");
            }
        }
        private void FillWindow(int windowID)
        {
            if (isGUIVisible)
            {
                string[] menuOptions = { "Basic", "Vessel" };
                selectedItem = GUILayout.SelectionGrid(selectedItem, menuOptions, 2);
                boxStyle = GUI.skin.GetStyle("Box");
                GUILayout.BeginVertical();

                TopButtons.Init(windowRect.width);
                GUI.Label(new Rect(9f, 2f, 29f, 29f), TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
                if (TopButtons.Button(TNBaseStyle.Cross))
                {
                    GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);
                    this.isGUIVisible = false;
                    this.ToggleButton(this.isGUIVisible);
                    UIFields.GameInputState = true;
                }
            }
            GUILayout.Label($"Active Vessel: {GameManager.Instance.Game.ViewController.GetActiveSimVessel().DisplayName}");

            var currentState = CurrentState;

            foreach (var kvp in currentState)
            {
                NotificationToggle.NotificationType notificationType = kvp.Key;
                bool isEnabled = kvp.Value;

                bool enableNotification = GUILayout.Toggle(isEnabled, $"Enable {notificationType.ToString()} Notification");
                notificationToggle.SetNotificationState(notificationType, enableNotification);
            }


            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0, 0, 10000, 500));

        }
    }
}
