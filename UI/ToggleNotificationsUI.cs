using BepInEx.Logging;
using KSP.Game;
using KSP.Messages;
using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using static ToggleNotifications.TNTools.UI.NotificationToggle;

namespace ToggleNotifications
{
    public class ToggleNotificationsUI
    {
        private Selection selection;
        public MessageCenterMessage isRefreshing;
        public NotificationEvents isRefreshingNotification;
        private static ManualLogSource Logger;
        public static NotificationToggle currentState;
        private static ToggleNotificationsUI instance;
        public ToggleNotificationsPlugin mainPlugin { get; }

        public Selection Selection { get { return selection; } }

        private static bool InitDone = false;


        public static ToggleNotificationsUI Instance { get { return instance; } }
        public ToggleNotificationsUI(ToggleNotificationsPlugin plugin)

        {
            instance = this;
            mainPlugin = plugin;
        }

        public bool RefreshState
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
                return false;
            }
        }

        public bool RefreshNotifications()
        {
            if (ToggleNotificationsPlugin.Instance.notificationToggle != null)
            {
                currentState = new NotificationToggle();

                foreach (var entry in ToggleNotificationsPlugin.Instance.notificationToggle.notificationStates)
                {
                    currentState.SetNotificationState(entry.Key.ToString(), entry.Value);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void ProcessOption(SetOption option)
        {
            switch (option.option)
            {
                case "refreshState":
                    bool isRefreshed = RefreshState;
                    break;
                case "refreshNotifications":
                    bool isRefreshedNotification = RefreshNotifications();
                    break;
                default:
                    // handle unknown option
                    break;
            }
        }

        TabsUI tabs = new TabsUI();
        private bool Loaded;
        private void OnEnable()
        {
            ToggleNotificationsPlugin.Instance.NotificationChange += NotificationChangeHandler;
        }
        private void OnDisable()
        {
            ToggleNotificationsPlugin.Instance.NotificationChange -= NotificationChangeHandler;
        }
        private void NotificationChangeHandler(NotificationToggle toggle)
        {
            bool solarPanelNotification = toggle.GetNotificationState("SolarPanelsIneffectiveMessage");
            SetSolarPanelState(solarPanelNotification);
        }
        private void NotificationChange(NotificationToggle toggle)
        {
            currentState = toggle;
            bool solarPanelNotification = currentState.GetNotificationState("SolarPanelsIneffectiveMessage");
            SetSolarPanelState(solarPanelNotification);
        }
        public void SetSolarPanelState(bool currentState)
        {
            mainPlugin.SetSolarPanelState(currentState); // Assuming mainPlugin is an instance of ToggleNotificationsPlugin
        }
        public void Update(ToggleNotificationsPlugin plugin)
        {
            if (!InitDone)
            {
                tabs.pages.Add(new SolarPage(mainPlugin));
                tabs.pages.Add(new CommRangePage(mainPlugin));
                tabs.pages.Add(new ThrottlePage(mainPlugin));
                tabs.pages.Add(new NodePage(mainPlugin));
                tabs.pages.Add(new GamePausedPage(mainPlugin));
                tabs.pages.Add(new ThrottlePage(mainPlugin));
                tabs.Init();
                InitDone = true;
            }

            Dictionary<NotificationType, bool> notificationStates = plugin.GetCurrentState();
            isRefreshing = RefreshState ? ConvertToMessageCenterMessage(currentState) : null;

            bool isRefreshedNotification = RefreshNotifications();

            if (isRefreshedNotification)
            {
                // Handle the refreshed notification
            }

            tabs.Update();
        }

        public MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle currentState)
        {
            // Implement the logic to convert the current state to a MessageCenterMessage
            // For example, if the current state represents the "SolarPanelsIneffectiveMessage":
            if (currentState.GetNotificationState("SolarPanelsIneffectiveMessage"))
            {
                SolarPanelsIneffectiveMessage message = new SolarPanelsIneffectiveMessage();
                message.SentOn = currentState.SentOn;

                // Set any other properties of the message as needed

                return message;
            }

            // If the notification name doesn't match any specific type, return null or a default message
            return null;
        }
    }
}

