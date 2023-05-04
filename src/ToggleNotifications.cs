using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using SpaceWarp;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI;
using UnityEngine;

namespace ToggleNotifications
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]

    public class ToggleNotificationsPlugin : BaseSpaceWarpPlugin
    {
        private ConfigEntry<bool> _enableNotificationsConfig;

        public const string ModGuid = "com.github.cvusmo.Toggle-Notifications";
        public const string ModName = "Toggle Notifications";
        public const string ModVer = "0.1.0";

        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        private const string ToolbarOABButtonID = "BTN-ToggleNotificationsOAB";
        private bool _isWindowOpen;
        private Rect _windowRect;

        public static ToggleNotificationsPlugin Instance { get; private set; }
        public new static ManualLogSource Logger { get; set; }

        public override void OnInitialized()
        {
            Instance = this;
            _enableNotificationsConfig = Config.Bind("Settings section", "Enable Notifications", true, "Toggle Notifications: Enabled (Notifications Enabled) or Disabled (Notifications Disabled)");

            // Harmony creates the plugin/patch
            Harmony.CreateAndPatchAll(typeof(ToggleNotificationsPlugin).Assembly);
        }

        public void OnAwake()
        {
            //Harmony.CreateAndPatchAll(typeof(NotificationToggle).Assembly);
        }
        private void Update()
        {
            //figure out what to update
        }

        //gui not super important but would be nice to have a gui for community fixes
        private void OnGUI()
        {
            // Set the UI
            GUI.skin = Skins.ConsoleSkin;
            GUI.backgroundColor = Color.white;
            GUI.enabled = true;

            if (_isWindowOpen)
            {
                _windowRect = GUILayout.Window(
                    GUIUtility.GetControlID(FocusType.Passive),
                    _windowRect,
                    FillWindow,
                    "Toggle Notifications",
                    GUILayout.Height(350),
                    GUILayout.Width(350)
                );
            }
        }
        private static void FillWindow(int windowID)
        {
            GUILayout.Label("Toggle Notifications - Toggle Notifications to be added to Community Fixes");
            GUI.DragWindow(new Rect(80, 20, 500, 500));
        }
    }
    [HarmonyPatch(typeof(NotificationEvents))]
    [HarmonyPatch("Update")]

    public static class SolarPanelNotification
    {
        public static bool SolarPanelsIneffectiveMessageToggle = false;
        //public static bool VesselOutOfElectricityMessageToggle = false;

        [HarmonyPatch(typeof(NotificationEvents))]
        [HarmonyPatch("SolarPanelsIneffectiveMessage")]
        [HarmonyPrefix]

        public static bool NotificationsEvents_SolarPanelsIneffectiveMessage(NotificationEvents __instance)
        {
            if (SolarPanelsIneffectiveMessageToggle)
            {
                return true;
            }
            return false;
        }

        public static bool VesselLeftCommunicationRangeMessageToggle = false;

        [HarmonyPatch(typeof(NotificationEvents))]
        [HarmonyPatch("VesselLeftCommunicationRangeMessage")]
        [HarmonyPrefix]

        public static bool NotificationsEvents_VesselLeftCommunicationRangeMessage(NotificationEvents __instance)
        {
            if (VesselLeftCommunicationRangeMessageToggle)
            {
                return true;
            }
            return false;
        }
        public static bool VesselThrottleLockedDueToTimewarpingMessageToggle = false;

        [HarmonyPatch(typeof(NotificationEvents))]
        [HarmonyPatch("VesselThrottleLockedDueToTimewarpingMessage")]
        [HarmonyPrefix]

        public static bool NotificationsEvents_VesselThrottleLockedDueToTimewarpingMessage(NotificationEvents __instance)
        {
            if (VesselThrottleLockedDueToTimewarpingMessageToggle)
            {
                return true;
            }
            return false;
        }

        public static bool CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle = false;

        [HarmonyPatch(typeof(NotificationEvents))]
        [HarmonyPatch("CannotPlaceManeuverNodeWhileOutOfFuelMessage")]
        [HarmonyPrefix]
        public static bool NotificationsEvents_CannotPlaceManeuverNodeWhileOutOfFuelMessage(NotificationEvents __instance)
        {
            if (CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle)
            {
                return true;
            }
            return false;
        }

        public static bool GamePauseToggledMessageToggle = false;

        [HarmonyPatch(typeof(NotificationEvents))]
        [HarmonyPatch("GamePauseToggledMessage")]
        [HarmonyPrefix]
        public static bool NotificationsEvents_GamePauseToggledMessage(NotificationEvents __instance)
        {
            if (GamePauseToggledMessageToggle)
            {
                return true;
            }
            return false;
        }
    }
}
