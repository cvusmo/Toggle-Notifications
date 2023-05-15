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
        private bool interfaceEnabled;
        private bool isGUIVisible = true;
        public Rect windowRect = Rect.zero;
        private int windowWidth = 250;
        public ToggleNotificationsUI MainUI;
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
        private static string assemblyFolder;
        private static string settingsPath;
        private static string AssemblyFolder => ToggleNotificationsPlugin.assemblyFolder ?? (ToggleNotificationsPlugin.assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static string SettingsPath => ToggleNotificationsPlugin.settingsPath ?? (ToggleNotificationsPlugin.settingsPath = Path.Combine(ToggleNotificationsPlugin.AssemblyFolder, "settings.json"));

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
            TNBaseSettings.Init(ToggleNotificationsPlugin.SettingsPath);
            TNBaseSettings.Init(TNBaseSettings.SettingsPath);
            MainUI = new ToggleNotificationsUI(this);
            ToggleNotificationsPlugin.Instance = this;
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
            Harmony.CreateAndPatchAll(typeof(ToggleNotificationsPlugin).Assembly);

            currentState = true; // Update the currentState variable to true
            Logger.LogInfo($"Current State: {currentState}");
        }
        public void ToggleButton(bool toggle)
        {
            interfaceEnabled = toggle;
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(this.interfaceEnabled);
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
                this.ToggleButton(!this.interfaceEnabled);
                ToggleNotificationsPlugin.Logger.LogInfo((object)"Update: Toggle Notifications UI toggled with hotkey");
            }
            if (this.MainUI == null)
                return;

            this.MainUI.Update();

        }
        public void InitializeRects()
        {
            mainGuiRect = settingsGuiRect = parGuiRect = orbGuiRect = surGuiRect = fltGuiRect = manGuiRect = tgtGuiRect = stgGuiRect = new Rect();
        }
        private void saverectpos()
        {
            TNBaseSettings.WindowXPos = (int)this.windowRect.xMin;
            TNBaseSettings.WindowYPos = (int)this.windowRect.yMin;
        }
        public void OnGUI()
        {
            isGUIVisible = false;
            GameState? state;
            GameState? nullable = state = BaseSpaceWarpPlugin.Game?.GlobalGameState?.GetState();
            GameState gameState1 = GameState.Map3DView;
            if (nullable.GetValueOrDefault() == gameState1 & nullable.HasValue)
                isGUIVisible = true;
            nullable = state;
            GameState gameState2 = GameState.FlightView;
            if (nullable.GetValueOrDefault() == gameState2 & nullable.HasValue)
                isGUIVisible = true;
            if (!interfaceEnabled && !isGUIVisible && currentState == false)
                return;
            TNStyles.Init();
            WindowTool.CheckMainWindowPos(ref windowRect);
            GUI.skin = TNBaseStyle.Skin;
            windowRect = GUILayout.Window(GUIUtility.GetControlID(FocusType.Passive), windowRect, new GUI.WindowFunction(FillWindow), "<color=#696DFF>FLIGHT PLAN</color>", GUILayout.Height(0.0f), GUILayout.Width((float)this.windowWidth));
            this.saverectpos();
            ToolTipsManager.DrawToolTips();
            UIFields.CheckEditor();
            Debug.Log("OnGUI is being called!");
        }
        private void FillWindow(int windowID)
        {
            TopButtons.Init(windowRect.width);
            GUI.Label(new Rect(9f, 2f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            if (TopButtons.Button(TNBaseStyle.Cross))
                this.CloseWindow();
            this.currentState = isGUIVisible;
            this.MainUI.OnGUI();
            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));
        }
        private void CloseWindow()
        {
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);
            this.interfaceEnabled = false;
            this.ToggleButton(this.interfaceEnabled);
            UIFields.GameInputState = true;
        }
    }
}

