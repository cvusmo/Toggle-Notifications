using BepInEx.Logging;
using ToggleNotifications;
using ToggleNotifications.TNTools.UI;

namespace TNUtility
{
    public class TNUtility
    {
        private static TNUtility _instance;
        public static TNUtility Instance { get => _instance; }

        public ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("TNUtility");
        public static NotificationToggle currentState;
        public static NotificationToggle UpdateCurrentStates;
        public static string InputDisableWindowAbbreviation = "WindowAbbreviation";
        public static string InputDisableWindowName = "WindowName";
        public TNUtility()
        {
            _instance = this;
        }

        /// Refreshes the currentState.
        public static void RefreshNotifications()
        {
            if (ToggleNotificationsPlugin.Instance._notificationToggle != null)
            {
                currentState = ToggleNotificationsPlugin.Instance._notificationToggle;
            }
            else
            {
                Instance.Logger.LogWarning("NotificationEvents is null.");
            }
        }

        // Refreshes the currentState by calling a method in ToggleNotificationsPlugin.
        public static void RefreshStates()
        {
            if (UpdateCurrentStates != null)
            {
                UpdateCurrentStates = ToggleNotificationsPlugin.Instance._notificationToggle;
            }
            else
            {
                Instance.Logger.LogWarning("UpdateCurrentStates is null.");
            }
        }
    }
}