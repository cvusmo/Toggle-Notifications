using BepInEx.Logging;
using KSP.Messages;
using KSP.Game;
using ToggleNotifications;
using ToggleNotifications.TNTools.UI;
using UnityEngine;
using static ToggleNotifications.TNTools.UI.NotificationToggle;

namespace TNUtilities
{
    internal class TNUtility
    {
        internal List<string> NotificationList = new List<string>();
        private static TNUtility _instance;
        internal static TNUtility Instance { get => _instance; }
        internal static ToggleNotificationsUI instance;
        internal static NotificationToggle toggleNotification;
        internal ToggleNotificationsPlugin mainPlugin;
        internal MessageCenterMessage Refreshing;
        internal NotificationEvents RefreshingNotification;
        internal bool RefreshNotification { get; set; }
        internal static NotificationToggle toggleState;
        internal static NotificationToggle notificationToggle;

        internal ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("TNUtility");
        internal TNUtility()
        {
            _instance = this;
            instance = ToggleNotificationsUI.Instance;
            toggleNotification = ToggleNotificationsUI.toggleNotification;
        }

        internal bool RefreshState
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

        internal bool RefreshNotifications()
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

        internal void RefreshCurrentState()
        {
            bool isRefreshing = instance.Refreshing != null;
            bool isRefreshingNotification = instance.RefreshingNotification != null;

            // Use the values as needed
            Debug.Log($"UI Refreshing: {isRefreshing}");
            Debug.Log($"Notification Refreshing: {isRefreshingNotification}");
        }

        internal static void DrawToggleButton(string toggleStr, NotificationType notificationType, int widthOverride = 0)
        {
            bool isOn = toggleNotification.GetNotificationState(notificationType);
            bool flag = UITools.SmallToggleButton(isOn, toggleStr, toggleStr, widthOverride);
            if (flag == isOn)
                return;
            toggleNotification.CheckCurrentState(notificationType, flag);
        }
    }
}
