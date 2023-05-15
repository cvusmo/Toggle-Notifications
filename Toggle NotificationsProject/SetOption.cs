using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications
{
    public class SetOption
    {
        public string option;
        public string value;

        public SetOption(string option, string value)
        {
            this.option = option;
            this.value = value;
        }

        public string SetOptionsList(string value) => value == "Enabled" ? "Disable" : "Enable";

        public void ProcessOption()
        {
            var notificationToggle = ToggleNotificationsPlugin.Instance.notificationToggle;

            if (notificationToggle != null)
            {
                switch (option)
                {
                    case "SolarPanelsIneffectiveMessage":
                        notificationToggle.SetNotificationState(NotificationToggle.NotificationType.SolarPanelsIneffectiveMessage, !notificationToggle.GetNotificationState(NotificationToggle.NotificationType.SolarPanelsIneffectiveMessage));
                        break;
                    case "VesselLeftCommunicationRangeMessage":
                        notificationToggle.SetNotificationState(NotificationToggle.NotificationType.VesselLeftCommunicationRangeMessage, !notificationToggle.GetNotificationState(NotificationToggle.NotificationType.VesselLeftCommunicationRangeMessage));
                        break;
                    case "VesselThrottleLockedDueToTimewarpingMessage":
                        notificationToggle.SetNotificationState(NotificationToggle.NotificationType.VesselThrottleLockedDueToTimewarpingMessage, !notificationToggle.GetNotificationState(NotificationToggle.NotificationType.VesselThrottleLockedDueToTimewarpingMessage));
                        break;
                    case "GamePauseToggledMessage":
                        notificationToggle.SetNotificationState(NotificationToggle.NotificationType.GamePauseToggledMessage, !notificationToggle.GetNotificationState(NotificationToggle.NotificationType.GamePauseToggledMessage));
                        break;
                    case "AllNotifications":
                        NotificationToggle.NotificationType[] notificationsToCheck = {
                            NotificationToggle.NotificationType.SolarPanelsIneffectiveMessage,
                            NotificationToggle.NotificationType.VesselLeftCommunicationRangeMessage,
                            NotificationToggle.NotificationType.VesselThrottleLockedDueToTimewarpingMessage,
                            NotificationToggle.NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage,
                            NotificationToggle.NotificationType.GamePauseToggledMessage
                        };
                        bool newState = !notificationToggle.GetNotificationState(notificationsToCheck[0]);

                        foreach (NotificationToggle.NotificationType notificationType in notificationsToCheck)
                        {
                            notificationToggle.SetNotificationState(notificationType, newState);
                        }
                        break;
                    default:
                        // handle unknown option
                        break;
                }
            }
        }
    }
}
