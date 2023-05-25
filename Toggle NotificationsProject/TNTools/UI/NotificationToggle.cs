using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class NotificationToggle
    {
        internal readonly ToggleNotificationsPlugin mainPlugin;
        internal Dictionary<NotificationType, bool> notificationStates;
        public long SentOn { get; internal set; }
        internal List<string> NotificationList { get; } = new List<string>();

        internal NotificationToggle(ToggleNotificationsPlugin mainPlugin, Dictionary<NotificationType, bool> notificationStates)
        {
            this.mainPlugin = mainPlugin;
            this.notificationStates = notificationStates;
            SentOn = 0;

            foreach (NotificationType notificationType in System.Enum.GetValues(typeof(NotificationType)))
            {
                NotificationList.Add(notificationType.ToString());
            }
        }

        internal bool GetNotificationState(NotificationType notificationType)
        {
            return notificationStates.TryGetValue(notificationType, out bool state) ? state : false;
        }

        internal void CheckCurrentState(NotificationType notificationType, bool flag)
        {
            bool currentFlag = GetNotificationState(notificationType);

            // Only update the state if the button is currently enabled and the user toggles it to disabled
            if (currentFlag && !flag)
            {
                notificationStates[notificationType] = false;

                if (NotificationList.Contains(notificationType.ToString()))
                {
                    NotificationList.Remove(notificationType.ToString());
                }
            }
        }
        internal bool ListGUI()
        {
            GUI.enabled = true;

            GUILayout.Label("Notification Options:");

            foreach (NotificationType notificationType in System.Enum.GetValues(typeof(NotificationType)))
            {
                if (notificationType == NotificationType.None)
                {
                    continue;
                }

                bool toggleState = GetNotificationState(notificationType);
                bool newToggleState = GUILayout.Toggle(toggleState, notificationType.ToString());
                if (newToggleState != toggleState)
                {
                    notificationStates[notificationType] = newToggleState;
                }
            }

            return true; // Indicate that a change has occurred
        }

    }
}
