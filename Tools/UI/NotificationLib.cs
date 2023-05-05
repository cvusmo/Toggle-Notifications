using HarmonyLib;
using System;
using System.Collections.Generic;
using KSP.Game;

namespace ToggleNotifications.Tools.UI
{
    public class NotificationToggle
    {
        // Library of all notifications
        public Dictionary<NotificationType, bool> notificationStates;

        public bool SolarPanelsIneffectiveMessageToggle { get; set; }
        public bool VesselLeftCommunicationRangeMessageToggle { get; set; }
        public bool VesselThrottleLockedDueToTimewarpingMessageToggle { get; set; }
        public bool CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle { get; set; }
        public bool GamePauseToggledMessageToggle { get; set; }

        public NotificationToggle()
        {
            notificationStates = new Dictionary<NotificationType, bool>
            {
                { NotificationType.SolarPanelsIneffectiveMessage, false },
                { NotificationType.VesselLeftCommunicationRangeMessage, false },
                { NotificationType.VesselThrottleLockedDueToTimewarpingMessage, false },
                { NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, false },
                { NotificationType.GamePauseToggledMessage, false }
            };
        }

        public bool GetNotificationState(string notificationKey)
        {
            switch (notificationKey)
            {
                case "SolarPanelsIneffectiveMessage":
                    return TNSettings.SolarPanelsIneffectiveMessage;
                case "VesselLeftCommunicationRangeMessage":
                    return TNSettings.VesselLeftCommunicationRangeMessage;
                case "VesselThrottleLockedDueToTimewarpingMessage":
                    return TNSettings.VesselThrottleLockedDueToTimewarpingMessage;
                case "CannotPlaceManeuverNodeWhileOutOfFuelMessage":
                    return TNSettings.CannotPlaceManeuverNodeWhileOutOfFuelMessage;
                case "GamePauseToggledMessage":
                    return TNSettings.GamePauseToggledMessage;
                default:
                    return false;
            }
        }

        public void SetNotificationState(string notificationKey, bool state)
        {
            switch (notificationKey)
            {
                case "SolarPanelsIneffectiveMessage":
                    TNSettings.SolarPanelsIneffectiveMessage = state;
                    break;
                case "VesselLeftCommunicationRangeMessage":
                    TNSettings.VesselLeftCommunicationRangeMessage = state;
                    break;
                case "VesselThrottleLockedDueToTimewarpingMessage":
                    TNSettings.VesselThrottleLockedDueToTimewarpingMessage = state;
                    break;
                case "CannotPlaceManeuverNodeWhileOutOfFuelMessage":
                    TNSettings.CannotPlaceManeuverNodeWhileOutOfFuelMessage = state;
                    break;
                case "GamePauseToggledMessage":
                    TNSettings.GamePauseToggledMessage = state;
                    break;
            }
        }

        // Notifications
        public enum NotificationType
        {
            SolarPanelsIneffectiveMessage,
            VesselLeftCommunicationRangeMessage,
            VesselThrottleLockedDueToTimewarpingMessage,
            CannotPlaceManeuverNodeWhileOutOfFuelMessage,
            GamePauseToggledMessage
        }
    }

    [HarmonyPatch(typeof(NotificationEvents))]
    public static class ToggleNotificationPatch
    {
        private static NotificationToggle _notificationToggle = new NotificationToggle();

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.SolarPanelsIneffectiveMessage))]
        public static bool SolarPanelsIneffectiveMessage(NotificationEvents __instance)
        {
            return !_notificationToggle.GetNotificationState("SolarPanelsIneffectiveMessage");
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.VesselLeftCommunicationRangeMessage))]
        public static bool VesselLeftCommunicationRangeMessage(NotificationEvents __instance)
        {
            return !_notificationToggle.GetNotificationState("VesselLeftCommunicationRangeMessage");
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.VesselThrottleLockedDueToTimewarpingMessage))]
        public static bool VesselThrottleLockedDueToTimewarpingMessage(NotificationEvents __instance)
        {
            return !_notificationToggle.GetNotificationState("VesselThrottleLockedDueToTimewarpingMessage");
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.CannotPlaceManeuverNodeWhileOutOfFuelMessage))]
        public static bool CannotPlaceManeuverNodeWhileOutOfFuelMessage(NotificationEvents __instance)
        {
            return !_notificationToggle.GetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage");
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.GamePauseToggledMessage))]
        public static bool GamePauseToggledMessage(NotificationEvents __instance)
        {
            return !_notificationToggle.GetNotificationState("GamePauseToggledMessage");
        }
    }
}