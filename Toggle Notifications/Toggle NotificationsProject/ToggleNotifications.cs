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
using ToggleNotifications.TNTools;
using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using UnityEngine;


namespace ToggleNotifications
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]

    public class ToggleNotificationsPlugin : BaseSpaceWarpPlugin
    {
        public static ToggleNotificationsPlugin Instance { get; private set; }

        public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
        public const string ModName = MyPluginInfo.PLUGIN_NAME;
        public const string ModVer = MyPluginInfo.PLUGIN_VERSION;
        //private readonly Texture2D icon = AssetsLoader.loadIcon("images/icon.png");


        //input states and text input from player
        public ConfigEntry<bool> _enableConfig;
        public bool currentState;
        public NotificationToggle _notificationToggle;
        private bool allNotificationsState = true;
        //GUI related
        bool GUIenabled = true; // or false, depending on your use case
        private Rect _windowRect;
        private int _windowWidth;
        private int _windowHeight;
        private bool _isWindowOpen;
        public ConfigEntry<bool> ShowSolarPanelIneffectiveMessageConfig;
        public ConfigEntry<bool> ShowCommunicationRangeMessageConfig;
        public ConfigEntry<bool> ShowThrottleLockedWarpMessageConfig;
        public ConfigEntry<bool> ShowManeuverNodeOutOfFuelMessageConfig;
        public ConfigEntry<bool> ShowGamePauseToggledMessageConfig;
        private bool loaded = false;

        //the vars volta
        public bool popoutSettings, popoutPar, popoutOrb, popoutSur, popoutMan, popoutTgt, popoutFlt, popoutStg;
        public bool solarPanelStateEnable, communicationRangeStateEnable, throttleLockedWarpStateEnable, mnOutofFuelStateEnable, pauseToggleStateEnable;
        public Rect mainGuiRect, settingsGuiRect, parGuiRect, orbGuiRect, surGuiRect, fltGuiRect, manGuiRect, tgtGuiRect, stgGuiRect;
        public bool solarPanelState, communicationRangeState, throttleLockedWarpState, mnOutofFuelState, pauseToggleState;

        //toggle button webster
        private Dictionary<string, bool> _toggles = new Dictionary<string, bool>();
        private Dictionary<string, bool> _previousToggles = new Dictionary<string, bool>();
        private Dictionary<string, bool> _initialToggles = new Dictionary<string, bool>();
        public Dictionary<NotificationToggle.NotificationType, bool> GetCurrentState()
        {
            return _notificationToggle.notificationStates;
        }

        //appbar info
        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        //private const string ToolbarOABButtonID = "BTN-ToggleNotificationsOAB";

        private static string settings_path;
        public bool inputFields;
        private static string SettingsPath =>
        settings_path ?? (settings_path = Path.Combine(BepInEx.Paths.ConfigPath, "ToggleNotifications", "settings.json"));
        public new static ManualLogSource Logger { get; set; }

        public void Load()
        {
            _enableConfig = Config.Bind("Toggle Notifications", "Enable", false, "Enable or disable notifications");
            currentState = _enableConfig.Value;
        }
        public override void OnInitialized()
        {
            base.OnInitialized();

            try
            {
                TNSettings.Init(SettingsPath);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to initialize TNSettings: {ex}");
                return;
            }

            Instance = this;

            //bepinex log
            Logger = base.Logger;

            //call notificationlib
            _notificationToggle = new NotificationToggle();
            _notificationToggle.SolarPanelsIneffectiveMessageToggle = false;
            _notificationToggle.VesselLeftCommunicationRangeMessageToggle = false;
            _notificationToggle.VesselThrottleLockedDueToTimewarpingMessageToggle = false;
            _notificationToggle.CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle = false;
            _notificationToggle.GamePauseToggledMessageToggle = false;
            _notificationToggle.GamePauseToggledMessageToggle = false;
            _notificationToggle.SetSolarPanelState(true);
            _notificationToggle.SetAllNotificationsState(true);
            _notificationToggle.SetCommunicationRangeState(true);
            _notificationToggle.SetThrottleLockedWarpState(true);
            _notificationToggle.SetManeuverNodeOutOfFuelState(true);
            _notificationToggle.SetGamePauseToggledState(true);
            ShowSolarPanelIneffectiveMessageConfig = Config.Bind("ToggleNotifications", "ShowSolarPanelIneffectiveMessage", true, "Show solar panel ineffective message.");
            ShowCommunicationRangeMessageConfig = Config.Bind("ToggleNotifications", "ShowCommunicationRangeMessage", true, "Show communication range message.");
            ShowThrottleLockedWarpMessageConfig = Config.Bind("ToggleNotifications", "ShowThrottleLockedWarpMessage", true, "Show throttle locked warp message.");
            ShowManeuverNodeOutOfFuelMessageConfig = Config.Bind("ToggleNotifications", "ShowManeuverNodeOutOfFuelMessage", true, "Show maneuver node out of fuel message.");
            ShowGamePauseToggledMessageConfig = Config.Bind("ToggleNotifications", "ShowGamePauseToggledMessage", true, "Show game pause toggled message.");

            Logger.LogInfo("Loaded");
            if (loaded)
            {
             Destroy(this);
            }
            loaded = true;

            // Register Flight AppBar button
            Appbar.RegisterAppButton(
            "Toggle Notifications",
            ToolbarFlightButtonID,
            AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"),
            ToggleButton);
            {
                // This code will be executed when the button is clicked
                _enableConfig.Value = !_enableConfig.Value; // toggle the configuration value

                currentState = _enableConfig.Value; // update the state based on the new configuration value

                if (currentState)
                {
                    // Do something when the button is toggled on
                    Debug.Log("Toggle Notifications button toggled on!");
                }
                else
                {
                    // Do something when the button is toggled off
                    Debug.Log("Toggle Notifications button toggled off!");
                }
            }

            //log config value
            //currentState.Init(this);
            Logger.LogInfo($"Current State: {currentState}");

            // Harmony creates the mainPlugin/patch
            Harmony.CreateAndPatchAll(typeof(ToggleNotificationsPlugin).Assembly);

        }
        private void ToggleButton(bool toggle)
        {
            currentState = toggle;
            GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(currentState);
        }
        void Awake()
        {
            TNStyles.Init();
            _windowRect = new Rect((Screen.width * 0.7f) - (_windowWidth / 2), (Screen.height * 2) - (_windowHeight / 2), 0, 0);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.RightAlt))
            {
                currentState = !currentState; // toggle the state

                UpdateCurrentStates(); // update the UI based on the new state

                Logger.LogInfo($"Update: ToggleNotifications toggled, current state is {currentState}");
            }
        }

        //make rects
        void save_rect_pos()
        {
            TNTools.TNBaseSettings.window_x_pos = (int)_windowRect.xMin;
            TNTools.TNBaseSettings.window_y_pos = (int)_windowRect.yMin;
        }

        private void InitializeRects()
        {
            mainGuiRect = settingsGuiRect = parGuiRect = orbGuiRect = surGuiRect = fltGuiRect = manGuiRect = tgtGuiRect = stgGuiRect = new Rect();
        }

        private void ResetLayout()
        {
            popoutPar = popoutStg = popoutOrb = popoutSur = popoutFlt = popoutTgt = popoutMan = popoutSettings = false;
            mainGuiRect.position = new Vector2(Screen.width * 0.8f, Screen.height * 0.2f);
            Vector2 popoutWindowPosition = new Vector2(Screen.width * 0.6f, Screen.height * 0.2f);
            parGuiRect.position = popoutWindowPosition;
            orbGuiRect.position = popoutWindowPosition;
            manGuiRect.position = popoutWindowPosition;
        }

        //begin GUI functionality
        //check currentState of notification and update UpdateCurrentStates to update GUI
        void OnGUI()
        {
            void FillMainGUI(int windowID)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Toggle Notifications:");

                solarPanelState = GUILayout.Toggle(solarPanelState, "Solar Panels Ineffective Message");
                communicationRangeState = GUILayout.Toggle(communicationRangeState, "Vessel Left Communication Range Message");
                throttleLockedWarpState = GUILayout.Toggle(throttleLockedWarpState, "Vessel Throttle Locked Due To Timewarping Message");
                mnOutofFuelState = GUILayout.Toggle(mnOutofFuelState, "Cannot Place Maneuver Node While Out Of Fuel Message");
                pauseToggleState = GUILayout.Toggle(pauseToggleState, "Game Pause Toggled Message");

                GUILayout.EndVertical();

                UpdateCurrentStates(); // Set the current state of each notification

                GUI.DragWindow();
            }

            GUIenabled = false;
            var gameState = Game?.GlobalGameState?.GetState();
            if (gameState == GameState.Map3DView) GUIenabled = true;
            if (gameState == GameState.FlightView) GUIenabled = true;

            if (GUILayout.Button(_enableConfig.Value ? "Disable Notifications" : "Enable Notifications"))
            {
                _enableConfig.Value = !_enableConfig.Value;
            }

            if (GUIenabled && currentState)
            {
                //TNStyles.Init();
                UI.UIWindow.check_main_window_pos(ref _windowRect);
                GUI.skin = TNStyles.skin;
                _windowRect = GUILayout.Window(
                    GUIUtility.GetControlID(FocusType.Passive),
                    _windowRect,
                    FillMainGUI,
                    "<color=#696DFF>TOGGLE NOTIFICATIONS</color>");

                save_rect_pos();

                GetCurrentState(); // Set the current state of the notifications to the value returned by getCurrentState()
            }
        }
        private void UpdateCurrentStates()
        {
            _notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", solarPanelState);
            _notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", communicationRangeState);
            _notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", throttleLockedWarpState);
            _notificationToggle.SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", mnOutofFuelState);
            _notificationToggle.SetNotificationState("GamePauseToggledMessage", pauseToggleState);
        }

        private void SetCurrentState()
        {
            _notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", allNotificationsState);
            _notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", allNotificationsState);
            _notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", allNotificationsState);
            _notificationToggle.SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", allNotificationsState);
            _notificationToggle.SetNotificationState("GamePauseToggledMessage", allNotificationsState);
        }

        private void SetAllNotificationsState(bool state)
        {
            allNotificationsState = state;
            SetCurrentState();
        }

        private void SetSolarPanelState(bool state)
        {
            _notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", state);
        }

        private void SetCommunicationRangeState(bool state)
        {
            _notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", state);
        }

        private void SetThrottleLockedWarpState(bool state)
        {
            _notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", state);
        }

        private void SetManeuverNodeOutOfFuelState(bool state)
        {
            _notificationToggle.SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", state);
        }

        private void SetGamePauseToggledState(bool state)
        {
            _notificationToggle.SetNotificationState("GamePauseToggledMessage", state);
        }
        private void CloseWindow()
        {
            // Set currentState to false to disable the GUI
            currentState = false;

            // Set the flight button toggle value to false to close the GUI window
            GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);

            // Set GameInputState to true to re-enable game input
            UI_Fields.GameInputState = true;
        }
    }
}
