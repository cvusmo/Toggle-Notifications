using ToggleNotifications.TNTools.UI;
using static ToggleNotifications.TNTools.UI.NotificationToggle;
using ToggleNotifications;
using ToggleNotifications.TNTools;

namespace ToggleNotifications.UI
{
    public class Selection
    {
        private ToggleNotificationsUI mainPlugin;
        private NotificationToggle currentState;

        public Selection(ToggleNotificationsUI mainPlugin)
        {
            this.mainPlugin = mainPlugin;
        }

        public bool IsSolarPanelSelected()
        {
            if (ToggleNotificationsPlugin.Instance.notificationToggle != null)
            {
                currentState = ToggleNotificationsPlugin.Instance.notificationToggle;
                return currentState.GetNotificationState("SolarPanelsIneffectiveMessage");
            }
            else
            {
                // Logger.LogWarning("NotificationToggle is null.");
                return false;
            }
        }

        public bool IsCommRangeSelected()
        {
            if (ToggleNotificationsPlugin.Instance.notificationToggle != null)
            {
                currentState = ToggleNotificationsPlugin.Instance.notificationToggle;
                return currentState.GetNotificationState("VesselLeftCommunicationRangeMessage");
            }
            else
            {
                // Logger.LogWarning("NotificationToggle is null.");
                return false;
            }
        }

        public bool IsThrottleLockedSelected()
        {
            if (ToggleNotificationsPlugin.Instance.notificationToggle != null)
            {
                currentState = ToggleNotificationsPlugin.Instance.notificationToggle;
                return currentState.GetNotificationState("VesselThrottleLockedDueToTimewarpingMessage");
            }
            else
            {
                // Logger.LogWarning("NotificationToggle is null.");
                return false;
            }
        }

        public bool IsGamePauseToggledSelected()
        {
            if (ToggleNotificationsPlugin.Instance.notificationToggle != null)
            {
                currentState = ToggleNotificationsPlugin.Instance.notificationToggle;
                return currentState.GetNotificationState("GamePauseToggledMessage");
            }
            else
            {
                // Logger.LogWarning("NotificationToggle is null.");
                return false;
            }
        }

        public bool AreAllNotificationsSelected()
        {
            if (ToggleNotificationsPlugin.Instance.notificationToggle != null)
            {
                currentState = ToggleNotificationsPlugin.Instance.notificationToggle;
                string[] notificationsToCheck = { "SolarPanelsIneffectiveMessage", "VesselLeftCommunicationRangeMessage", "VesselThrottleLockedDueToTimewarpingMessage", "CannotPlaceManeuverNodeWhileOutOfFuelMessage", "GamePauseToggledMessage" };
                foreach (string notificationName in notificationsToCheck)
                {
                    if (!currentState.GetNotificationState(notificationName))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                // Logger.LogWarning("NotificationToggle is null.");
                return false;
            }
        }
    }

    public class SetOption
    {
        private ToggleNotificationsUI mainPlugin;
        private NotificationToggle currentState;
        private NotificationToggle toggleNotification;

        public string option;
        public string value;

        public SetOption(string option, string value)
        {
            this.option = option;
            this.value = value;
        }

        public string SetOptionsList(string value)
        {
            if (value == "Enabled")
            {
                return "Disable";
            }
            else
            {
                return "Enable";
            }
        }
        public void AddOption(NotificationType type, SetOption option)
        {
            //
        }
        public void ProcessOption()
        {
            switch (option)
            {
                case "SolarPanelsIneffectiveMessage":
                    bool solarPanelState = !ToggleNotificationsPlugin.Instance.notificationToggle.GetNotificationState("SolarPanelsIneffectiveMessage");
                    ToggleNotificationsPlugin.Instance.notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", solarPanelState);
                    break;
                case "VesselLeftCommunicationRangeMessage":
                    bool commRangeState = !ToggleNotificationsPlugin.Instance.notificationToggle.GetNotificationState("VesselLeftCommunicationRangeMessage");
                    ToggleNotificationsPlugin.Instance.notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", commRangeState);
                    break;
                case "VesselThrottleLockedDueToTimewarpingMessage":
                    bool throttleLockedState = !ToggleNotificationsPlugin.Instance.notificationToggle.GetNotificationState("VesselThrottleLockedDueToTimewarpingMessage");
                    ToggleNotificationsPlugin.Instance.notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", throttleLockedState);
                    break;
                case "GamePauseToggledMessage":
                    bool gamePauseState = !ToggleNotificationsPlugin.Instance.notificationToggle.GetNotificationState("GamePauseToggledMessage");
                    ToggleNotificationsPlugin.Instance.notificationToggle.SetNotificationState("GamePauseToggledMessage", gamePauseState);
                    break;
                case "AllNotifications":
                    string[] notificationsToCheck = { "SolarPanelsIneffectiveMessage", "VesselLeftCommunicationRangeMessage", "VesselThrottleLockedDueToTimewarpingMessage", "CannotPlaceManeuverNodeWhileOutOfFuelMessage", "GamePauseToggledMessage" };
                    bool newState = !ToggleNotificationsPlugin.Instance;
                    foreach (string notificationName in notificationsToCheck)
                    {
                        ToggleNotificationsPlugin.Instance.notificationToggle.SetNotificationState(notificationName, newState);
                    }
                    break;
                default:
                    // handle unknown option
                    break;
            }
        }

        private bool RefreshState
        {
            get
            {
                if (currentState == null)
                {
                    RefreshNotifications();
                }
                string[] notificationsToCheck = { "SolarPanelsIneffectiveMessage", "VesselLeftCommunicationRangeMessage", "VesselThrottleLockedDueToTimewarpingMessage", "CannotPlaceManeuverNodeWhileOutOfFuelMessage", "GamePauseToggledMessage" };

                foreach (string notificationName in notificationsToCheck)
                {
                    if (currentState.GetNotificationState(notificationName))
                    {
                        return true;
                    }
                }
                // None of the notifications are active, so return false
                return false;
            }
        }

        private bool RefreshNotifications()
        {
            if (ToggleNotificationsPlugin.Instance.notificationToggle != null)
            {
                currentState = ToggleNotificationsPlugin.Instance.notificationToggle;
                return true;
            }
            else
            {
                // Logger.LogWarning("NotificationToggle is null.");
                return false;
            }
        }
    }
}