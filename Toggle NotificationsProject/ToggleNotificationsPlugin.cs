using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KSP.Game;
using KSP.Messages;
using KSP.Sim.impl;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI.Appbar;
using System.Reflection;
using ToggleNotifications.Pages;
using ToggleNotifications.PatchManager;
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
        internal ToggleNotificationsUI MainUI;
        internal NotificationToggle notificationToggle;
        internal GameInstance game;
        internal MessageCenter messageCenter;
        internal VesselComponent _activeVessel;
        internal GearPage gearPage;
        public ConfigEntry<bool> tnConfig;
        internal bool _interfaceEnabled;
        internal bool _isGUIenabled = false;
        internal Rect windowRect = Rect.zero;
        internal int windowWidth = 250;

        //appbar
        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        private static string assemblyFolder;
        private static string settingsPath;
        private static string AssemblyFolder => assemblyFolder ?? (assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static string SettingsPath => settingsPath ?? (settingsPath = Path.Combine(AssemblyFolder, "settings.json"));

        internal static ToggleNotificationsPlugin Instance { get; private set; }
        NotificationData data = new NotificationData();
        internal new static ManualLogSource Logger { get; set; }
        private void Awake()
        {
            AssistantToTheAssistantPatchManager.ApplyPatches(notificationToggle);
        }
        public override void OnInitialized()
        {
            TNBaseSettings.Init(SettingsPath);

            base.OnInitialized();

            Instance = this;
            Logger = base.Logger;
            Logger.LogInfo("Loaded");

            game = GameManager.Instance.Game;
            messageCenter = game.Messages;

            MainUI = new ToggleNotificationsUI(this, _isGUIenabled, messageCenter);

            // Register Flight AppBar button
            Appbar.RegisterAppButton(
                "Toggle Notifications",
                ToolbarFlightButtonID,
                AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"), //changed
                isOpen =>
                {
                    ToggleButton(isOpen, isOpen);
                    Debug.Log($"Initial _interfaceEnabled value: {_interfaceEnabled}");
                    Debug.Log($"Initial _isGUIenabled value: {_isGUIenabled}");
                }
            );

            notificationToggle = new NotificationToggle(this, new Dictionary<NotificationType, bool>()
            {
                { NotificationType.GamePauseToggledMessage, true },
                { NotificationType.SolarPanelsIneffectiveMessage, true },
                { NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, true },
                { NotificationType.VesselOutOfElectricity, true },
                { NotificationType.VesselLostControlMessage, true },
                { NotificationType.VesselLeftCommunicationRangeMessage, true },
                { NotificationType.CannotChangeNodeWhileOutOfFuelMessage, true },
            });

            // configuration
            tnConfig = Config.Bind("Notification Settings", "Toggle Notifications", true, "Toggle Notifications is a mod that allows you to enable or disable notifications");

        }

        internal void ToggleButton(bool toggle, bool isOpen)
        {
            _interfaceEnabled = isOpen;
            _isGUIenabled = toggle;
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(_isGUIenabled);
        }
        internal void Update()
        {
            if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.P))
            {
                ToggleButton(!_interfaceEnabled, !_isGUIenabled);
                Logger.LogInfo("Update: Toggle Notifications UI toggled with hotkey");
            }
            if (_isGUIenabled)
            {
                if (MainUI == null)
                    return;
            }
        }
        internal void saverectpos()
        {
            TNBaseSettings.WindowXPos = (int)windowRect.xMin;
            TNBaseSettings.WindowYPos = (int)windowRect.yMin;
        }
        private void OnGUI()
        {
            _isGUIenabled = false;
            GameState? state = BaseSpaceWarpPlugin.Game?.GlobalGameState?.GetState();
            GameState? nullable = state;
            GameState gameState1 = GameState.Map3DView;
            if (nullable.GetValueOrDefault() == gameState1 & nullable.HasValue)
                _isGUIenabled = true;
            nullable = state;
            GameState gameState2 = GameState.FlightView;
            if (nullable.GetValueOrDefault() == gameState2 & nullable.HasValue)
                _isGUIenabled = true;

            if (_isGUIenabled)
            {
                TNStyles.Init();
                WindowTool.CheckMainWindowPos(ref windowRect, windowWidth);
                GUI.skin = TNBaseStyle.Skin;
                _activeVessel = GameManager.Instance?.Game?.ViewController?.GetActiveVehicle(true)?.GetSimVessel();
                if (!_interfaceEnabled || !_isGUIenabled || _activeVessel == null)
                    return;

                MainUI.OnGUI();
                saverectpos();
                UIFields.CheckEditor();
            }
        }
        internal void CloseWindow()
        {
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);
            _interfaceEnabled = false;
            ToggleButton(false, false);
            Rect closeButtonPosition = new Rect(windowRect.width - 30, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);
        }
    }
}