using KSP.Messages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    public class BasePageContent : MonoBehaviour, IPageContent
    {
        public NotificationToggle currentState;
        public static ToggleNotificationsPlugin mainPlugin;
        public bool IsRunning => false;
        public bool UIVisible { get; set; } = true;

        public BasePageContent(ToggleNotificationsPlugin mainPlugin)
        {
            BasePageContent.mainPlugin = mainPlugin;
        }
        MessageCenterMessage IPageContent.ConvertToMessageCenterMessage(NotificationToggle currentState)
        {
            throw new NotImplementedException();
        }
        protected virtual NotificationToggle GetCurrentState()
        {
            NotificationToggle currentState = new NotificationToggle();

            bool solarPanelNotification = ToggleNotificationsPlugin.Instance.notificationToggle.GetNotificationState("SolarPanelsIneffectiveMessage");
            currentState.SetNotificationState("SolarPanelsIneffectiveMessage", solarPanelNotification);

            bool commRangeNotification = ToggleNotificationsPlugin.Instance.notificationToggle.GetNotificationState("CommunicationRangeNotification");
            currentState.SetNotificationState("CommunicationRangeNotification", commRangeNotification);

            bool throttleLockNotification = ToggleNotificationsPlugin.Instance.notificationToggle.GetNotificationState("ThrottleLockNotification");
            currentState.SetNotificationState("ThrottleLockNotification", throttleLockNotification);

            return currentState;
        }

        protected virtual void NotificationChange(NotificationToggle toggle)
        {
            bool solarPanelNotification = toggle.GetNotificationState("SolarPanelsIneffectiveMessage");
            if (solarPanelNotification)
            {
                SwitchToPage(0);
            }
            else
            {
                SwitchToPage(1);
            }
        }

        private void SwitchToPage(int nextPageIndex)
        {
            // Logic to switch to the specified page
            // For example, you could activate/deactivate GameObjects or update the contents of a UI element
        }

        public virtual string Name => throw new System.NotImplementedException();

        public virtual GUIContent Icon => throw new System.NotImplementedException();

        public virtual bool IsActive => throw new System.NotImplementedException();
        public virtual void OnGUI()
        {
            throw new System.NotImplementedException();
        }
    }
    public class SolarPage : BasePageContent, IPageContent
    {
        private Action<bool> setSolarPanelState;
        public SolarPage(ToggleNotificationsPlugin mainPlugin) : base(mainPlugin)
        {
            // Enable or disable the plugin for solar panels
            bool enablePluginForSolarPanels = mainPlugin.solarPanelState; // Use the value of solarPanelState property
            mainPlugin.notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", enablePluginForSolarPanels);
        }

        public SolarPage(ToggleNotificationsPlugin mainPlugin, Action<bool> setSolarPanelState) : base(mainPlugin)
        {
            this.setSolarPanelState = setSolarPanelState ?? throw new ArgumentNullException(nameof(setSolarPanelState));
        }

        public MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle currentState)
        {
            SolarPanelsIneffectiveMessage message = new SolarPanelsIneffectiveMessage();
            message.SentOn = currentState.SentOn;

            return message;
        }

        public string Name => "Solar Notification";

        public override GUIContent Icon => new GUIContent("Solar");

        public override bool IsActive
        {
            get
            {
                // Update the current state of the solar panel notification
                UpdateCurrentStates();
                return mainPlugin.solarPanelState;
            }
        }

        public bool IsRunning
        {
            //
            get
            {
                UpdateCurrentStates();
                return true;
            }
        }

        public bool UIVisible
        {
            //
            get
            {
                UpdateCurrentStates();
                return true;
            }

            set { UpdateCurrentStates(); }
        }

        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Solar Notification");

            // Display the current state of the solar panel notification
            GUILayout.Label("Solar Panel Notification State: " + (IsActive ? "Enabled" : "Disabled"));

            // Add a toggle button that allows the user to enable or disable the notification
            mainPlugin.solarPanelState = GUILayout.Toggle(mainPlugin.solarPanelState, "Enable Solar Panel Notification");

            // Update the current state of the solar panel notification
            UpdateCurrentStates();
        }

        private void UpdateCurrentStates()
        {
            mainPlugin.notificationToggle.SetNotificationState("SolarPanelsIneffectiveMessage", mainPlugin.solarPanelState);
        }
    }
    public class CommRangePage : BasePageContent, IPageContent
    {
        private Action<bool> setCommunicationRangeState;
        public CommRangePage(ToggleNotificationsPlugin mainPlugin) : base(mainPlugin)
        {
            // Enable or disable the plugin for communication range
            bool enablePluginForCommRange = mainPlugin.commRangeState; // Use the value of commRangeState property
            mainPlugin.notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", enablePluginForCommRange);
        }
        public CommRangePage(ToggleNotificationsPlugin mainPlugin, Action<bool> setCommunicationRangeState) : base(mainPlugin)
        {
            this.setCommunicationRangeState = setCommunicationRangeState ?? throw new ArgumentNullException(nameof(setCommunicationRangeState));
        }

        public string Name => "Communications Notification";

        public MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle currentState)
        {
            VesselLeftCommunicationRangeMessage message = new VesselLeftCommunicationRangeMessage();
            message.SentOn = currentState.SentOn;

            return message;
        }

        public override GUIContent Icon => new GUIContent("Comm");

        public override bool IsActive
        {
            get
            {
                // Update the current state of the communication range notification
                UpdateCurrentStates();
                return mainPlugin.commRangeState;
            }
        }

        public bool IsRunning
        {
            get
            {
                // Update the current state of the communication range notification
                UpdateCurrentStates();
                return true;
            }
        }

        public bool UIVisible
        {
            get
            {
                // Update the current state of the communication range notification
                UpdateCurrentStates();
                return true;
            }
            set
            {
                // Update the current state of the communication range notification
                UpdateCurrentStates();
            }
        }

        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Communication Range Notification");

            // Display the current state of the communication range notification
            GUILayout.Label("Communication Range Notification State: " + (IsActive ? "Enabled" : "Disabled"));

            // Add a toggle button that allows the user to enable or disable the notification
            mainPlugin.commRangeState = GUILayout.Toggle(mainPlugin.commRangeState, "Enable Communication Range Notification");

            // Update the current state of the communication range notification
            UpdateCurrentStates();
        }

        private void UpdateCurrentStates()
        {
            mainPlugin.notificationToggle.SetNotificationState("VesselLeftCommunicationRangeMessage", mainPlugin.commRangeState);
        }
    }
    public class ThrottlePage : BasePageContent, IPageContent
    {
        private Action<bool> setThrottleLockedWarpState;
        public ThrottlePage(ToggleNotificationsPlugin mainPlugin) : base(mainPlugin)
        {
            // Enable or disable the plugin for throttle lock
            bool enablePluginForThrottleLock = mainPlugin.throttleLockedWarpState; // Use the value of throttleLockedWarpState property
            mainPlugin.notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", enablePluginForThrottleLock);
        }
        public ThrottlePage(ToggleNotificationsPlugin mainPlugin, Action<bool> setThrottleLockedWarpState) : base(mainPlugin)
        {
            this.setThrottleLockedWarpState = setThrottleLockedWarpState ?? throw new ArgumentNullException(nameof(setThrottleLockedWarpState));
        }
        public string Name => "Throttle Notification";

        public MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle currentState)
        {
            VesselThrottleLockedDueToTimewarpingMessage message = new VesselThrottleLockedDueToTimewarpingMessage();
            message.SentOn = currentState.SentOn;

            return message;
        }

        public override GUIContent Icon => new GUIContent("Throttle");

        public override bool IsActive
        {
            get
            {
                // Update the current state of the throttle lock notification
                UpdateCurrentStates();
                return mainPlugin.throttleLockedWarpState;
            }
        }

        public bool IsRunning
        {
            get
            {
                // Update the current state of the throttle lock notification
                UpdateCurrentStates();
                return true;
            }
        }

        public bool UIVisible
        {
            get
            {
                // Update the current state of the throttle lock notification
                UpdateCurrentStates();
                return true;
            }
            set
            {
                // Update the current state of the throttle lock notification
                UpdateCurrentStates();
            }
        }

        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Throttle Lock Notification");

            // Display the current state of the throttle lock notification
            GUILayout.Label("Throttle Lock Notification State: " + (IsActive ? "Enabled" : "Disabled"));

            // Add a toggle button that allows the user to enable or disable the notification
            mainPlugin.throttleLockedWarpState = GUILayout.Toggle(mainPlugin.throttleLockedWarpState, "Enable Throttle Lock Notification");

            // Update the current state of the throttle lock notification
            UpdateCurrentStates();
        }

        private void UpdateCurrentStates()
        {
            mainPlugin.notificationToggle.SetNotificationState("VesselThrottleLockedDueToTimewarpingMessage", mainPlugin.throttleLockedWarpState);
        }
    }
    public class NodePage : BasePageContent, IPageContent
    {
        private Action<bool> setManeuverNodeOutOfFuelState;
        public NodePage(ToggleNotificationsPlugin mainPlugin) : base(mainPlugin)
        {
            bool enablePluginForManeuverNodeOutOfFuelState = mainPlugin.maneuverNodeOutOfFuelState; // Use the value of maneuverNodeOutOfFuelState property
            mainPlugin.notificationToggle.SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", enablePluginForManeuverNodeOutOfFuelState);
        }

        public NodePage(ToggleNotificationsPlugin mainPlugin, Action<bool> setManeuverNodeOutOfFuelState) : base(mainPlugin)
        {
            this.setManeuverNodeOutOfFuelState = setManeuverNodeOutOfFuelState ?? throw new ArgumentNullException(nameof(setManeuverNodeOutOfFuelState));
        }
        public string Name => "Maneuver Node";

        public MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle currentState)
        {
            CannotPlaceManeuverNodeWhileOutOfFuelMessage message = new CannotPlaceManeuverNodeWhileOutOfFuelMessage();
            message.SentOn = currentState.SentOn;

            return message;
        }

        public override GUIContent Icon => new GUIContent("Node");

        public override bool IsActive
        {
            get
            {
                // Update the current state of the maneuver node notification
                UpdateCurrentStates();
                return mainPlugin.maneuverNodeOutOfFuelState;
            }
        }

        public bool IsRunning
        {
            get
            {
                // Update the current state of the maneuver node notification
                UpdateCurrentStates();
                return true;
            }
        }

        public bool UIVisible
        {
            get
            {
                // Update the current state of the maneuver node notification
                UpdateCurrentStates();
                return true;
            }
            set
            {
                // Update the current state of the maneuver node notification
                UpdateCurrentStates();
            }
        }

        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Maneuver Node Notification");

            // Display the current state of the maneuver node notification
            GUILayout.Label("Maneuver Node Notification State: " + (IsActive ? "Enabled" : "Disabled"));

            // Add a toggle button that allows the user to enable or disable the notification
            mainPlugin.maneuverNodeOutOfFuelState = GUILayout.Toggle(mainPlugin.maneuverNodeOutOfFuelState, "Enable Maneuver Node Notification");

            // Update the current state of the maneuver node notification
            UpdateCurrentStates();
        }

        private void UpdateCurrentStates()
        {
            mainPlugin.notificationToggle.SetNotificationState("CannotPlaceManeuverNodeWhileOutOfFuelMessage", mainPlugin.maneuverNodeOutOfFuelState);
        }
    }
    public class GamePausedPage : BasePageContent, IPageContent
    {
        private Action<bool> setGamePauseToggledState;
        public GamePausedPage(ToggleNotificationsPlugin mainPlugin) : base(mainPlugin)
        {
            bool enablePluginForPauseToggleState = mainPlugin.pauseToggleState; // Use the value of pauseToggleState property
            mainPlugin.notificationToggle.SetNotificationState("GamePauseToggledMessage", enablePluginForPauseToggleState);
        }
        public GamePausedPage(ToggleNotificationsPlugin mainPlugin, Action<bool> setGamePauseToggledState) : base(mainPlugin)
        {
            this.setGamePauseToggledState = setGamePauseToggledState ?? throw new ArgumentNullException(nameof(setGamePauseToggledState));
        }
        public string Name => "Game Paused";

        public MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle currentState)
        {
            GamePauseToggledMessage message = new GamePauseToggledMessage();
            message.SentOn = currentState.SentOn;

            return message;
        }

        public override GUIContent Icon => new GUIContent("G");

        public override bool IsActive
        {
            get
            {
                // Update the current state of the game pause notification
                UpdateCurrentStates();
                return mainPlugin.pauseToggleState;
            }
        }

        public bool IsRunning
        {
            get
            {
                // Update the current state of the game pause notification
                UpdateCurrentStates();
                return true;
            }
        }

        public bool UIVisible
        {
            get
            {
                // Update the current state of the game pause notification
                UpdateCurrentStates();
                return true;
            }
            set
            {
                // Update the current state of the game pause notification
                UpdateCurrentStates();
            }
        }

        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Game Pause Notification");

            // Display the current state of the game pause notification
            GUILayout.Label("Game Pause Notification State: " + (IsActive ? "Enabled" : "Disabled"));

            // Add a toggle button that allows the user to enable or disable the notification
            mainPlugin.pauseToggleState = GUILayout.Toggle(mainPlugin.pauseToggleState, "Enable Game Pause Notification");

            // Update the current state of the game pause notification
            UpdateCurrentStates();
        }

        private void UpdateCurrentStates()
        {
            mainPlugin.notificationToggle.SetNotificationState("GamePauseToggledMessage", mainPlugin.pauseToggleState);
        }
    }

}
