using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications
{
    public static class AssistantToTheAssistantPatchManager
    {
        [HarmonyPatch(typeof(NotificationEvents))]
        [HarmonyPatch(typeof(ToggleNotificationPatch))]
        public static class ToggleNotificationPatch
        {
            private static readonly ToggleNotificationsPlugin mainPlugin;
            public static NotificationToggle NotificationToggle { get; }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NotificationEvents.SolarPanelsIneffectiveMessage))]
            public static bool SolarPanelsIneffectiveMessage(NotificationEvents __instance)
            {
                return !NotificationToggle.GetNotificationState(NotificationToggle.NotificationType.SolarPanelsIneffectiveMessage);
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NotificationEvents.VesselThrottleLockedDueToTimewarpingMessage))]
            public static bool VesselThrottleLockedDueToTimewarpingMessage(NotificationEvents __instance)
            {
                return !NotificationToggle.GetNotificationState(NotificationToggle.NotificationType.VesselThrottleLockedDueToTimewarpingMessage);
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NotificationEvents.CannotPlaceManeuverNodeWhileOutOfFuelMessage))]
            public static bool CannotPlaceManeuverNodeWhileOutOfFuelMessage(NotificationEvents __instance)
            {
                return !NotificationToggle.GetNotificationState(NotificationToggle.NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage);
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NotificationEvents.GamePauseToggledMessage))]
            public static bool GamePauseToggledMessage(NotificationEvents __instance)
            {
                return !NotificationToggle.GetNotificationState(NotificationToggle.NotificationType.GamePauseToggledMessage);
            }

            // You can add more Harmony patches for other notification events here
        }

        [HarmonyPatch(typeof(GameStateMachine), "OnStateChange")]
        [HarmonyPatch(typeof(DisablePauseGUIPatch))]
        public static class DisablePauseGUIPatch
        {
            private static void Prefix(GameStateMachine __instance, GameStateChangedMessage stateChangedMessage)
            {
                if (__instance.Paused)
                {
                    UIManager uiManager = __instance.Game.UI;
                    uiManager.HidePause();
                    uiManager.SetPauseVisible(true);
                }
            }
        }

        [HarmonyPatch(typeof(NotificationUIAlert))]
        [HarmonyPatch(typeof(UIAlertPatch))]
        public static class UIAlertPatch


        {
            private static readonly NotificationToggle notificationToggle;

            public static NotificationToggle NotificationToggle => notificationToggle;

            [HarmonyPrefix]
            [HarmonyPatch("ShowNotification")]
            public static bool ShowNotification(ref string notificationId)
            {
                if (!NotificationToggle.GetNotificationState(NotificationToggle.NotificationType.GamePauseToggledMessage))
                {
                    if (notificationId == "GamePauseToggledMessage")
                        return false;
                }

                return true;
            }
        }
    }
}
