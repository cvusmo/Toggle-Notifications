// Purpose of the NotificationToggle class is to control and display options for when a notification type should occur.
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class NotificationToggle
    {
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
            notificationStates[notificationType] = flag;
            if (AssistantToTheAssistantPatchManager.NotificationToggle != null)
            {
                AssistantToTheAssistantPatchManager.NotificationToggle.CheckCurrentState(notificationType, flag);
            }
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
                        mainPlugin.LogCurrentState(); // Update the current state in the plugin
                    }
                }
            }

            if (mainPlugin.isGUIVisible)
            {
                mainPlugin.MainUI.ShowGUI(); // Call ShowGUI method to display the GUI elements
            }
            else
            {
                mainPlugin.MainUI.HideGUI(); // Call HideGUI method to hide the GUI elements
            }

            return true; // Indicate that a change has occurred
        }
    }
}
