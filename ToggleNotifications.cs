using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DocumentFormat.OpenXml.Wordprocessing;
using HarmonyLib;
using KSP.Game;
using KSP.UI.Binding;
using Microsoft.Win32;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI.Appbar;
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
        public static ToggleNotificationsPlugin Instance { get; private set; }

        public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
        public const string ModName = MyPluginInfo.PLUGIN_NAME;
        public const string ModVer = MyPluginInfo.PLUGIN_VERSION;


        //input states and text input from player
        public ConfigEntry<bool> _enableConfig;
        public ConfigEntry<bool> ShowSolarPanelIneffectiveMessageConfig;
        public ConfigEntry<bool> ShowCommunicationRangeMessageConfig;
        public ConfigEntry<bool> ShowThrottleLockedWarpMessageConfig;
        public ConfigEntry<bool> ShowManeuverNodeOutOfFuelMessageConfig;
        public ConfigEntry<bool> ShowGamePauseToggledMessageConfig;
        public bool currentState;
        public NotificationToggle notificationToggle;
        private bool allNotificationsState = true;

        //GUI related
        bool GUIenabled = true; // or false, depending on your use case
        private Rect windowRect = Rect.zero;
        private int windowWidth;
        private int windowHeight;
        private bool isWindowOpen;
        private bool loaded = false;
        private GameInstance game;

        //LOOK THIS UP IN FP
        public ToggleNotificationsUI MainUI;

        //the vars volta
        public bool popoutSettings, popoutPar, popoutOrb, popoutSur, popoutMan, popoutTgt, popoutFlt, popoutStg;
        public Rect mainGuiRect, settingsGuiRect, parGuiRect, orbGuiRect, surGuiRect, fltGuiRect, manGuiRect, tgtGuiRect, stgGuiRect;
        public bool solarPanelState, commRangeState, throttleLockedWarpState, maneuverNodeOutOfFuelState, pauseToggleState;
        public Dictionary<NotificationToggle.NotificationType, bool> GetCurrentState()
        {
            var currentState  = new Dictionary<NotificationToggle.NotificationType, bool>();

            foreach (NotificationToggle.NotificationType type in Enum.GetValues(typeof(NotificationToggle.NotificationType)))
            {
                if (type == NotificationToggle.NotificationType.None) continue;

                bool isRefreshing = false;
                bool isRefreshingNotification = false;

                // Code to determine isRefreshing and isRefreshingNotification

                currentState [type] = isRefreshing || isRefreshingNotification;
            }

            return currentState ;
        }

        //appbar info
        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        //private const string ToolbarOABButtonID = "BTN-ToggleNotificationsOAB";

        private static string settingspath;
        public bool inputFields;
        private static string SettingsPath =>
        settingspath ?? (settingspath = Path.Combine(BepInEx.Paths.ConfigPath, "ToggleNotifications", "settings.json"));
        public new static ManualLogSource Logger { get; set; }
        public void Load()
        {
            _enableConfig = Config.Bind("Toggle Notifications", "Enable", false, "Enable or disable notifications");
            currentState = _enableConfig.Value;

            Logger.LogInfo("Loaded");
            if (loaded)
            {
                Destroy(this);
            }
            loaded = true;
        }
        public override void OnInitialized()
        {
            Instance = this;
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

            MainUI = new ToggleNotificationsUI(this);

            //bepinex log
            game = GameManager.Instance.Game;
            Logger = base.Logger;

            //call notificationlib
            notificationToggle = new NotificationToggle();
            notificationToggle.SolarPanelsIneffectiveMessageToggle = false;
            notificationToggle.VesselLeftCommunicationRangeMessageToggle = false;
            notificationToggle.VesselThrottleLockedDueToTimewarpingMessageToggle = false;
            notificationToggle.CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle = false;
            notificationToggle.GamePauseToggledMessageToggle = false;
            notificationToggle.GamePauseToggledMessageToggle = false;
            notificationToggle.SetSolarPanelState(true);
            notificationToggle.SetAllNotificationsState(true);
            notificationToggle.SetCommunicationRangeState(true);
            notificationToggle.SetThrottleLockedWarpState(true);
            notificationToggle.SetManeuverNodeOutOfFuelState(true);
            notificationToggle.SetGamePauseToggledState(true);
            ShowSolarPanelIneffectiveMessageConfig = Config.Bind("ToggleNotifications", "ShowSolarPanelIneffectiveMessage", true, "Show solar panel ineffective message.");
            ShowCommunicationRangeMessageConfig = Config.Bind("ToggleNotifications", "ShowCommunicationRangeMessage", true, "Show communication range message.");
            ShowThrottleLockedWarpMessageConfig = Config.Bind("ToggleNotifications", "ShowThrottleLockedWarpMessage", true, "Show throttle locked warp message.");
            ShowManeuverNodeOutOfFuelMessageConfig = Config.Bind("ToggleNotifications", "ShowManeuverNodeOutOfFuelMessage", true, "Show maneuver node out of fuel message.");
            ShowGamePauseToggledMessageConfig = Config.Bind("ToggleNotifications", "ShowGamePauseToggledMessage", true, "Show game pause toggled message.");

            ToggleNotificationsPlugin plugin = new ToggleNotificationsPlugin();

            {
                // This code will be executed when the button is clicked
                _enableConfig.Value = !_enableConfig.Value; // toggle the configuration value

                currentState = _enableConfig.Value; // update the currentState  based on the new configuration value

                if (currentState)
                {
                    // Do something when the button is toggled on
                    //ToggleButton();
                    Debug.Log("Toggle Notifications button toggled on!");
                }
                else
                {
                    // Do something when the button is toggled off
                    Debug.Log("Toggle Notifications button toggled off!");
                }
            }

            //Register Flight AppBar button
            Appbar.RegisterAppButton(
            "Toggle Notifications",
            ToolbarFlightButtonID,
            AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/Icon.png"),
            ToggleButton);

            // Harmony creates the mainPlugin/patch
            Harmony.CreateAndPatchAll(typeof(ToggleNotificationsPlugin).Assembly);

            TNStatus.Init(this);

            //log config value
            Logger.LogInfo($"Current State: {currentState}");

        }
        public void ToggleButton(bool toggle)
        {
            currentState = toggle;
        }
        void Awake()
        {
            TNStyles.Init();
            windowRect = new Rect((Screen.width * 0.7f) - (windowWidth / 2), (Screen.height * 2) - (windowHeight / 2), 0, 0);
        }
        public void OnEnable()
        {
            Logger.LogInfo("ToggleNotificationsPlugin enabled");
        }
        public void OnDisable()
        {
            Logger.LogInfo("ToggleNotificationsPlugin disabled");
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
            {
                currentState = !currentState; // toggle the currentState 

                UpdateCurrentStates(); // update the UI based on the new currentState 

                Logger.LogInfo($"Update: ToggleNotifications toggled, current currentState  is {currentState}");
            }
        }
        void saveRectPos()
        {
            TNTools.TNBaseSettings.WindowXPos = (int)windowRect.xMin;
            TNTools.TNBaseSettings.WindowYPos = (int)windowRect.yMin;
        }
        public void InitializeRects()
        {
            mainGuiRect = settingsGuiRect = parGuiRect = orbGuiRect = surGuiRect = fltGuiRect = manGuiRect = tgtGuiRect = stgGuiRect = new Rect();
        }
        public void ResetLayout()
        {
            popoutPar = popoutStg = popoutOrb = popoutSur = popoutFlt = popoutTgt = popoutMan = popoutSettings = false;
            mainGuiRect.position = new Vector2(Screen.width * 0.8f, Screen.height * 0.2f);
            Vector2 popoutWindowPosition = new Vector2(Screen.width * 0.6f, Screen.height * 0.2f);
            parGuiRect.position = popoutWindowPosition;
            orbGuiRect.position = popoutWindowPosition;
            manGuiRect.position = popoutWindowPosition;
        }

        public event Action<NotificationToggle> NotificationChange;
        public void OnNotificationChange(NotificationToggle toggle)
        {
            // Raise the event if there are subscribers
            NotificationChange?.Invoke(toggle);
        }
        void OnGUI()
        {
            void FillMainGUI(int windowID)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Toggle Notifications:");

                solarPanelState = GUILayout.Toggle(solarPanelState, "Solar Panels Ineffective Message");
                commRangeState = GUILayout.Toggle(commRangeState, "Vessel Left Communication Range Message");
                throttleLockedWarpState = GUILayout.Toggle(throttleLockedWarpState, "Vessel Throttle Locked Due To Timewarping Message");
                maneuverNodeOutOfFuelState = GUILayout.Toggle(maneuverNodeOutOfFuelState, "Cannot Place Maneuver Node While Out Of Fuel Message");
                pauseToggleState = GUILayout.Toggle(pauseToggleState, "Game Pause Toggled Message");

                GUILayout.EndVertical();

                UpdateCurrentStates(); // Set the current currentState  of each notification

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
                TNTools.UI.WindowTool.CheckMainWindowPos(ref windowRect);
                GUI.skin = TNStyles.skin;
                windowRect = GUILayout.Window(
                    GUIUtility.GetControlID(FocusType.Passive),
                    windowRect,
                    FillMainGUI,
                    "<color=#696DFF>TOGGLE NOTIFICATIONS</color>");

                saveRectPos();

                GetCurrentState(); // Set the current currentState  of the notifications to the value returned by getCurrentState()
            }
        }
        public void UpdateCurrentStates()
        {
            notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", solarPanelState);
            notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", commRangeState);
            notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", throttleLockedWarpState);
            notificationToggle.SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", maneuverNodeOutOfFuelState);
            notificationToggle.SetNotificationState("GamePauseToggledMessage", pauseToggleState);

        }
        public void SetCurrentState()
        {
            notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", allNotificationsState);
            notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", allNotificationsState);
            notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", allNotificationsState);
            notificationToggle.SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", allNotificationsState);
            notificationToggle.SetNotificationState("GamePauseToggledMessage", allNotificationsState);
        }
        public void SetAllNotificationsState(bool currentState )
        {
            allNotificationsState = currentState ;
            SetCurrentState();
        }
        public void SetSolarPanelState(bool currentState )
        {
            notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", currentState );
            solarPanelState = currentState ;
        }
        public void SetCommunicationRangeState(bool currentState )
        {
            notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", currentState );
        }
        public void SetThrottleLockedWarpState(bool currentState )
        {
            notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", currentState );
        }
        public void SetManeuverNodeOutOfFuelState(bool currentState )
        {
            notificationToggle.SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", currentState );
        }
        public void SetGamePauseToggledState(bool currentState )
        {
            notificationToggle.SetNotificationState("GamePauseToggledMessage", currentState );
        }
        public void CloseWindow()
        {
            // Set currentState to false to disable the GUI
            currentState = false;

            // Set the flight button toggle value to false to close the GUI window
            GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);

            // Set GameInputState to true to re-enable game input
            UIFields.GameInputState = true;
        }
    }
}
