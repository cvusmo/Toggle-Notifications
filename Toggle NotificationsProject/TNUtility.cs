using BepInEx.Logging;
using KSP.Messages;
using KSP.Game;
using ToggleNotifications;
using ToggleNotifications.TNTools.UI;
using UnityEngine;
using static ToggleNotifications.TNTools.UI.NotificationToggle;

namespace TNUtilities
{
    public class TNUtility
    {
        public List<string> NotificationList = new List<string>();
        private static TNUtility _instance;
        public static TNUtility Instance { get => _instance; }
        public static ToggleNotificationsUI instance;
        public static NotificationToggle toggleNotification;
        public ToggleNotificationsPlugin mainPlugin;
        public MessageCenterMessage Refreshing;
        public NotificationEvents RefreshingNotification;
        public bool RefreshNotification { get; set; }
        public static NotificationToggle toggleState;
        public static NotificationToggle notificationToggle;

        public ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("TNUtility");
        public TNUtility()
        {
            _instance = this;
            instance = ToggleNotificationsUI.Instance;
            toggleNotification = ToggleNotificationsUI.toggleNotification;
        }

        public bool RefreshState
        {
            get
            {
                RefreshNotifications();
                string[] notificationsToCheck = { "SolarPanelsIneffectiveMessage", "VesselLeftCommunicationRangeMessage", "VesselThrottleLockedDueToTimewarpingMessage", "CannotPlaceManeuverNodeWhileOutOfFuelMessage", "GamePauseToggledMessage" };

                foreach (string notificationName in notificationsToCheck)
                {
                    if (Enum.TryParse(notificationName, out NotificationType notificationType) && toggleState.GetNotificationState(notificationType))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool RefreshNotifications()
        {
            if (RefreshNotification)
            {
                NotificationList.Clear();
                foreach (NotificationType notificationType in System.Enum.GetValues(typeof(NotificationType)))
                {
                    if (notificationType != NotificationType.None)
                    {
                        NotificationList.Add(notificationType.ToString());
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void RefreshCurrentState()
        {
            bool isRefreshing = instance.Refreshing != null;
            bool isRefreshingNotification = instance.RefreshingNotification != null;

            // Use the values as needed
            Debug.Log($"UI Refreshing: {isRefreshing}");
            Debug.Log($"Notification Refreshing: {isRefreshingNotification}");
        }

        public static void DrawToggleButton(string toggleStr, NotificationType notificationType, int widthOverride = 0)
        {
            bool isOn = toggleNotification.GetNotificationState(notificationType);
            bool flag = UITools.SmallToggleButton(isOn, toggleStr, toggleStr, widthOverride);
            if (flag == isOn)
                return;
            toggleNotification.CheckCurrentState(notificationType, flag);
        }
    }
}
