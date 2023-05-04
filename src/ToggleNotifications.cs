using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI;
using SpaceWarp.API.UI.Appbar;
using UnityEngine;
using System;
using BepInEx.Logging;

namespace ToggleNotifications
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]

    //[HarmonyPatch(typeof(DiscoverableMessage))]
    //[HarmonyPatch("isActive", MethodType.Setter)]
    //[DiscoverableMessage("Events/Flight/Parts/SolarPanelsIneffectiveMessage")]
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
        private static object _solarPanelsIneffectiveNotificationHandle;
        private static object PartIneffective;
        private bool _ToggleNotifications;

        public static ToggleNotificationsPlugin Instance { get; private set; }
        public new static ManualLogSource Logger { get; set; }

        public override void OnInitialized()
        {
            Instance = this;
            _enableNotificationsConfig = Config.Bind("Settings section", "Enable Notifications", true, "Toggle Notifications: Enabled (Notifications Enabled) or Disabled (Notifications Disabled)");
            //Logger.LogInfo($"Toggle Notifications Plugin: Enabled = {_enableNotificationsConfig.Value}");

            // Harmony creates the plugin/patch
            Harmony.CreateAndPatchAll(typeof(ToggleNotificationsPlugin).Assembly);
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
        // Disable SolarPanelsIneffective

        //[HarmonyPatch(typeof(NotificationManager))]
        //[HarmonyPatch("Update")]
        //[HarmonyPatch(typeof(DiscoverableMessage))]
        //[HarmonyPatch("Update")]
        [HarmonyPatch(typeof(NotificationEvents))]
        [HarmonyPatch("Update")]

        static void Prefix(NotificationEvents __instance)
        {
            _solarPanelsIneffectiveNotificationHandle = null;
            PartIneffective = null;
        }
    }
}
