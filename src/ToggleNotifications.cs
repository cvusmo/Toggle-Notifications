using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.UI.Binding;
using TNUtility;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI;
using SpaceWarp.API.UI.Appbar;
using ToggleNotifications.Tools;
using ToggleNotifications.Tools.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Microsoft.CodeAnalysis;

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

        private ConfigEntry<bool> _enableConfig;

        //input states and text input from player
        private NotificationToggle _notificationToggle;
        private bool interfaceEnabled = false;
        private bool GUIenabled = true;
        private Rect _windowRect;
        private int _windowWidth;
        private int _windowHeight;
        private bool _isWindowOpen;

        //toggle button webster
        private Dictionary<string, bool> _toggles = new Dictionary<string, bool>();
        private Dictionary<string, bool> _previousToggles = new Dictionary<string, bool>();
        private Dictionary<string, bool> _initialToggles = new Dictionary<string, bool>();


        //local vars
        public bool popoutSettings, popoutPar, popoutOrb, popoutSur, popoutMan, popoutTgt, popoutFlt, popoutStg;
        public bool solarPanelStateEnable, communicationRangeStateEnable, throttleLockedWarpStateEnable, mnOutofFuelStateEnable, pauseToggleStateEnable;
        public Rect mainGuiRect, settingsGuiRect, parGuiRect, orbGuiRect, surGuiRect, fltGuiRect, manGuiRect, tgtGuiRect, stgGuiRect;
        public bool solarPanelState, communicationRangeState, throttleLockedWarpState, mnOutofFuelState, pauseToggleState;
        public bool currentState;

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
            _enableConfig = Config.Bind("Section name", "Setting name", true, "Description");
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

            // Register Flight AppBar button
            var icon = AssetManager.GetAsset<Texture2D>($"{MyPluginInfo.PLUGIN_GUID}/assets/images/icon.png");
            Appbar.RegisterAppButton("Toggle Notifications", ToolbarFlightButtonID, icon, isEnabled =>
            {
                // This code will be executed when the button is clicked
                // You can add your own code here to perform the desired action
                Debug.Log("Toggle Notifications button clicked!");
            });


            // Harmony creates the plugin/patch
            Harmony.CreateAndPatchAll(typeof(ToggleNotificationsPlugin).Assembly);

            //log config value
            Logger.LogInfo($"Current State: {currentState}");
        }        
        private void ToggleButton(bool toggle)
        {
            interfaceEnabled = toggle;
            GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(interfaceEnabled);
        }
        void Awake()
        {
            _windowRect = new Rect((Screen.width * 0.7f) - (_windowWidth / 2), (Screen.height * 2) - (_windowHeight / 2), 0, 0);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.RightControl))
            {
                ToggleButton(!interfaceEnabled);
                Logger.LogInfo("Update: ToggleNotifications toggled with hotkey");
            }
        }

        //make rects
        void save_rect_pos()
        {
            BaseSettings.window_x_pos = (int)_windowRect.xMin;
            BaseSettings.window_y_pos = (int)_windowRect.yMin;
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

        //check currentState of notification and update GUI
        private void UpdateCurrentStates()
        {
            _notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", solarPanelState);
            _notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", communicationRangeState);
            _notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", throttleLockedWarpState);
            _notificationToggle.SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", mnOutofFuelState);
            _notificationToggle.SetNotificationState("GamePauseToggledMessage", pauseToggleState);
        }
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
            GUI.DragWindow();
        }
        void OnGUI()
        {
            GUIenabled = false;
            var gameState = Game?.GlobalGameState?.GetState();
            if (gameState == GameState.Map3DView) GUIenabled = true;
            if (gameState == GameState.FlightView) GUIenabled = true;

            if (GUILayout.Button(_enableConfig.Value ? "Disable Notifications" : "Enable Notifications"))
            {
                _enableConfig.Value = !_enableConfig.Value;
            }

            if (interfaceEnabled && GUIenabled && currentState)
            {
                TNStyles.Init();
                UI.UIWindow.check_main_window_pos(ref _windowRect);
                GUI.skin = TNStyles.skin;
                _windowRect = GUILayout.Window(
                    GUIUtility.GetControlID(FocusType.Passive),
                    _windowRect,
                    FillMainGUI,
                    "<color=#696DFF>TOGGLE NOTIFICATIONS</color>");

                save_rect_pos();

                UpdateCurrentStates();
            }
        }

    }
}
