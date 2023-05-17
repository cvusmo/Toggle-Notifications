using BepInEx.Logging;
using ToggleNotifications;
using ToggleNotifications.TNTools.UI;

namespace TNUtilities
{
    //provides utility methods to interact with and manage those notification states.
    public class TNUtility
    {
        private static TNUtility _instance;
        public static TNUtility Instance { get => _instance; }

        public ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("TNUtility");
        public static NotificationToggle toggleState;
        public static NotificationToggle notificationToggle;

        public TNUtility()
        {
            _instance = this;
        }

        public static void RefreshNotificationsUtil()
        {
            if (ToggleNotificationsUI.toggleNotification != null)
            {
                toggleState = ToggleNotificationsUI.toggleNotification;
            }
            else
            {
                Instance.Logger.LogWarning("ToggleNotification is null.");
            }
        }

        public static void RefreshStatesUtil()
        {
            if (ToggleNotificationsUI.toggleState != null)
            {
                toggleState = ToggleNotificationsUI.toggleState;
            }
            else
            {
                Instance.Logger.LogWarning("ToggleState is null.");
            }
        }

        public void CheckCurrentStateUtil()
        {
            bool isRefreshing = ToggleNotificationsUI.instance.Refreshing != null;
            bool isRefreshingNotification = ToggleNotificationsUI.instance.RefreshingNotification != null;

            // Use the values as needed
            Logger.LogInfo($"UI Refreshing: {isRefreshing}");
            Logger.LogInfo($"Notification Refreshing: {isRefreshingNotification}");
        }
    }
}
