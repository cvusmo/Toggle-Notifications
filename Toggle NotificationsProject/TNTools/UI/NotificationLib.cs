using HarmonyLib;
using KSP.Game;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class NotificationToggle
    {
        // Library of all notifications

        private readonly ToggleNotificationsPlugin pluginLib;
        public Dictionary<NotificationType, bool> GetCurrentState()
        {
            return notificationStates;
        }
        public Dictionary<NotificationType, bool> notificationStates;

        public bool SolarPanelsIneffectiveMessageToggle { get; set; }
        public bool VesselLeftCommunicationRangeMessageToggle { get; set; }
        public bool VesselThrottleLockedDueToTimewarpingMessageToggle { get; set; }
        public bool CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle { get; set; }
        public bool GamePauseToggledMessageToggle { get; set; }
        public long SentOn { get; internal set; }

        public NotificationToggle(ToggleNotificationsPlugin pluginLib, Dictionary<NotificationType, bool> notificationStates, bool solarPanelsIneffectiveMessageToggle, bool vesselLeftCommunicationRangeMessageToggle, bool vesselThrottleLockedDueToTimewarpingMessageToggle, bool cannotPlaceManeuverNodeWhileOutOfFuelMessageToggle, bool gamePauseToggledMessageToggle, long sentOn)
        {
            this.pluginLib = pluginLib;
            this.notificationStates = notificationStates;
            SolarPanelsIneffectiveMessageToggle = solarPanelsIneffectiveMessageToggle;
            VesselLeftCommunicationRangeMessageToggle = vesselLeftCommunicationRangeMessageToggle;
            VesselThrottleLockedDueToTimewarpingMessageToggle = vesselThrottleLockedDueToTimewarpingMessageToggle;
            CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle = cannotPlaceManeuverNodeWhileOutOfFuelMessageToggle;
            GamePauseToggledMessageToggle = gamePauseToggledMessageToggle;
            SentOn = sentOn;
        }
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

        public void UpdateCurrentStates()
        {
            pluginLib.UpdateCurrentStates();
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
        public void SetNotificationState(string notificationKey, bool currentState)
        {
            if (Enum.TryParse(notificationKey, out NotificationType notificationType))
            {
                notificationStates[notificationType] = currentState;

                switch (notificationType)
                {
                    case NotificationType.SolarPanelsIneffectiveMessage:
                        TNSettings.SolarPanelsIneffectiveMessage = currentState;
                        break;
                    case NotificationType.VesselLeftCommunicationRangeMessage:
                        TNSettings.VesselLeftCommunicationRangeMessage = currentState;
                        break;
                    case NotificationType.VesselThrottleLockedDueToTimewarpingMessage:
                        TNSettings.VesselThrottleLockedDueToTimewarpingMessage = currentState;
                        break;
                    case NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage:
                        TNSettings.CannotPlaceManeuverNodeWhileOutOfFuelMessage = currentState;
                        break;
                    case NotificationType.GamePauseToggledMessage:
                        TNSettings.GamePauseToggledMessage = currentState;
                        break;
                }
            }
            else
            {
                Debug.LogError($"Invalid notificationKey: {notificationKey}");
                UpdateCurrentStates();
            }
        }
        public void SetNotificationState(NotificationType notificationType, bool currentState)
        {
            notificationStates[notificationType] = currentState;
        }
        public void SetAllNotificationsState(bool currentState)
        {
            SetNotificationState("SolarPanelsIneffectiveMessage", currentState);
            SetNotificationState("VesselLeftCommunicationRangeMessage", currentState);
            SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", currentState);
            SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", currentState);
            SetNotificationState("GamePauseToggledMessage", currentState);
        }
        public void SetSolarPanelState(bool currentState)
        {
            SetNotificationState(NotificationType.SolarPanelsIneffectiveMessage, currentState);
        }
        public void SetCommunicationRangeState(bool currentState)
        {
            SetNotificationState(NotificationType.VesselLeftCommunicationRangeMessage, currentState);
        }
        public void SetThrottleLockedWarpState(bool currentState)
        {
            SetNotificationState(NotificationType.VesselThrottleLockedDueToTimewarpingMessage, currentState);
        }
        public void SetManeuverNodeOutOfFuelState(bool currentState)
        {
            SetNotificationState(NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, currentState);
        }
        public void SetGamePauseToggledState(bool currentState)
        {
            SetNotificationState(NotificationType.GamePauseToggledMessage, currentState);
        }
        // Notifications
        public enum NotificationType
        {
            SolarPanelsIneffectiveMessage,
            VesselLeftCommunicationRangeMessage,
            VesselThrottleLockedDueToTimewarpingMessage,
            CannotPlaceManeuverNodeWhileOutOfFuelMessage,
            GamePauseToggledMessage,
            None
        }
    }

    [HarmonyPatch(typeof(NotificationEvents))]
    public static class ToggleNotificationPatch
    {
        private static readonly NotificationToggle notificationToggle = new NotificationToggle();

        public static NotificationToggle NotificationToggle => notificationToggle;

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.SolarPanelsIneffectiveMessage))]
        public static bool SolarPanelsIneffectiveMessage(NotificationEvents __instance)
        {
            return !NotificationToggle.GetNotificationState("SolarPanelsIneffectiveMessage");
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.VesselLeftCommunicationRangeMessage))]
        public static bool VesselLeftCommunicationRangeMessage(NotificationEvents __instance)
        {
            return !NotificationToggle.GetNotificationState("VesselLeftCommunicationRangeMessage");
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.VesselThrottleLockedDueToTimewarpingMessage))]
        public static bool VesselThrottleLockedDueToTimewarpingMessage(NotificationEvents __instance)
        {
            return !NotificationToggle.GetNotificationState("VesselThrottleLockedDueToTimewarpingMessage");
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.CannotPlaceManeuverNodeWhileOutOfFuelMessage))]
        public static bool CannotPlaceManeuverNodeWhileOutOfFuelMessage(NotificationEvents __instance)
        {
            return !NotificationToggle.GetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage");
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(NotificationEvents.GamePauseToggledMessage))]
        public static bool GamePauseToggledMessage(NotificationEvents __instance)
        {
            return !NotificationToggle.GetNotificationState("GamePauseToggledMessage");
        }
    }
    public class NotificationTogglePage : BasePageContent
    {
        private readonly NotificationToggle notificationToggle;
        public NotificationTogglePage(ToggleNotificationsPlugin mainPlugin) : base(mainPlugin)
        {
            notificationToggle = ToggleNotificationsPlugin.Instance.notificationToggle;
            notificationToggle.SetAllNotificationsState(false);
            notificationToggle.SetSolarPanelState(true);
        }

        public override string Name => "Notification Toggle";

        public override GUIContent Icon => new GUIContent("Icon");

        public override bool IsActive => notificationToggle.GetNotificationState("SolarPanelsIneffectiveMessage");

        public override void OnGUI()
        {
            // Implement the code to show the notification toggle page
        }
        protected override void NotificationChange(NotificationToggle newToggle)
        {
            bool solarPanelNotification = newToggle.GetNotificationState("SolarPanelsIneffectiveMessage");
            if (solarPanelNotification)
            {
                SwitchToPage(0);
            }
            else
            {
                SwitchToPage(1);
            }
        }

        private void SwitchToPage(int v)
        {
            throw new NotImplementedException();
        }
    }
}