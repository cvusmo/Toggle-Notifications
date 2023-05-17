using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    //handles the individual notification states, 
    public class NotificationToggle
    {
        public enum NotificationType
        {
            SolarPanelsIneffectiveMessage,
            VesselThrottleLockedDueToTimewarpingMessage,
            CannotPlaceManeuverNodeWhileOutOfFuelMessage,
            GamePauseToggledMessage,
            None
        }

        private readonly ToggleNotificationsPlugin mainPlugin;
        public Dictionary<NotificationType, bool> notificationStates;

        public long SentOn { get; internal set; }
        public List<string> NotificationList { get; } = new List<string>();

        public NotificationToggle(ToggleNotificationsPlugin mainPlugin, Dictionary<NotificationType, bool> notificationStates)
        {
            this.mainPlugin = mainPlugin;
            this.notificationStates = notificationStates;
            SentOn = 0;

            foreach (NotificationType notificationType in System.Enum.GetValues(typeof(NotificationType)))
            {
                if (notificationType != NotificationType.None)
                {
                    NotificationList.Add(notificationType.ToString());
                }
            }
        }

        public bool GetNotificationState(NotificationType notificationType)
        {
            return notificationStates.TryGetValue(notificationType, out bool state) ? state : false;
        }

        public void CheckCurrentState(NotificationType notificationType, bool flag)
        {
            bool solarPanelsIneffectiveMessageToggle = GetNotificationState(NotificationType.SolarPanelsIneffectiveMessage);
            bool gamePauseToggledMessageToggle = GetNotificationState(NotificationType.GamePauseToggledMessage);
        }

        public bool ListGUI()
        {
            GUI.enabled = true;

            GUILayout.Label("Notification Options:");

            foreach (NotificationType notificationType in System.Enum.GetValues(typeof(NotificationType)))
            {
                if (notificationType != NotificationType.None)
                {
                    bool toggleState = GetNotificationState(notificationType);
                    bool newToggleState = GUILayout.Toggle(toggleState, notificationType.ToString());
                    if (newToggleState != toggleState)
                    {
                        notificationStates[notificationType] = newToggleState;
                        mainPlugin.CheckCurrentState(); // Update the current state in the plugin
                    }
                }
            }

            return true; // Indicate that a change has occurred
        }
    }
}
