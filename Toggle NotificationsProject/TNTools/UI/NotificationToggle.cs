using ToggleNotifications.PatchManager;
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

            if (currentFlag != flag)
            {
                notificationStates[notificationType] = flag;

                if (flag && !NotificationList.Contains(notificationType.ToString()))
                {
                    NotificationList.Add(notificationType.ToString());
                }
                else if (!flag && NotificationList.Contains(notificationType.ToString()))
                {
                    NotificationList.Remove(notificationType.ToString());
                }
            }
        }
        public int GetNotificationCount()
        {
            return notificationStates.Count(kv => kv.Value);
        }
        internal bool ListGUI()
        {
            GUILayout.Label("Notification Options:");

            Dictionary<NotificationType, Action<bool>> propertyMap = new Dictionary<NotificationType, Action<bool>>()
            {
                { NotificationType.SolarPanelsIneffectiveMessage, newToggleState => AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = newToggleState },
                { NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, newToggleState => AssistantToTheAssistantPatchManager.isOutOfFuelEnabled = newToggleState },
                { NotificationType.GamePauseToggledMessage, newToggleState => AssistantToTheAssistantPatchManager.isPauseVisible = newToggleState },
                { NotificationType.VesselOutOfElectricity, newToggleState => AssistantToTheAssistantPatchManager.isElectricalEnabled = newToggleState },
                { NotificationType.VesselLostControlMessage, newToggleState => AssistantToTheAssistantPatchManager.isLostControlEnabled = newToggleState },
                { NotificationType.VesselLeftCommunicationRangeMessage, newToggleState => AssistantToTheAssistantPatchManager.isCommRangeEnabled = newToggleState },
                { NotificationType.CannotChangeNodeWhileOutOfFuelMessage, newToggleState => AssistantToTheAssistantPatchManager.isOutOfFuelEnabled = newToggleState }
            };

            foreach (KeyValuePair<NotificationType, bool> kvp in notificationStates)
            {
                NotificationType notificationType = kvp.Key;
                bool toggleState = kvp.Value;

                bool newToggleState = GUILayout.Toggle(toggleState, notificationType.ToString(), GUILayout.ExpandWidth(false));

                if (newToggleState != toggleState)
                {
                    notificationStates[notificationType] = newToggleState;

                    if (propertyMap.ContainsKey(notificationType))
                    {
                        propertyMap[notificationType].Invoke(newToggleState);
                    }
                }
            }
            return true;
        }
    }
}
