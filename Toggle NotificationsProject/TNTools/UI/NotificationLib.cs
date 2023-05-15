using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class NotificationToggle
    {
        private readonly ToggleNotificationsPlugin mainPlugin;
        public Dictionary<NotificationType, bool> notificationStates;
        public bool SolarPanelsIneffectiveMessageToggle { get; set; }
        public bool VesselLeftCommunicationRangeMessageToggle { get; set; }
        public bool VesselThrottleLockedDueToTimewarpingMessageToggle { get; set; }
        public bool CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle { get; set; }
        public bool GamePauseToggledMessageToggle { get; set; }
        public long SentOn { get; internal set; }

        //LIST of Notifications
        public List<string> NotificationList { get; } = new List<string>();
        public NotificationToggle(ToggleNotificationsPlugin mainPlugin, Dictionary<NotificationType, bool> notificationStates, bool solarPanelsIneffectiveMessageToggle, bool vesselLeftCommunicationRangeMessageToggle, bool vesselThrottleLockedDueToTimewarpingMessageToggle, bool cannotPlaceManeuverNodeWhileOutOfFuelMessageToggle, bool gamePauseToggledMessageToggle, long sentOn)
        {
            this.mainPlugin = mainPlugin;
            this.notificationStates = notificationStates;
            SolarPanelsIneffectiveMessageToggle = solarPanelsIneffectiveMessageToggle;
            VesselLeftCommunicationRangeMessageToggle = vesselLeftCommunicationRangeMessageToggle;
            VesselThrottleLockedDueToTimewarpingMessageToggle = vesselThrottleLockedDueToTimewarpingMessageToggle;
            CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle = cannotPlaceManeuverNodeWhileOutOfFuelMessageToggle;
            GamePauseToggledMessageToggle = gamePauseToggledMessageToggle;
            SentOn = sentOn;

            // Add the names of the notifications to the list
            NotificationList.Add("Solar Panels Ineffective");
            NotificationList.Add("Vessel Left Communication Range");
            NotificationList.Add("Vessel Throttle Locked Due to Timewarping");
            NotificationList.Add("Cannot Place Maneuver Node While Out of Fuel");
            NotificationList.Add("Game Pause Toggled");
        }

        public bool GetNotificationState(NotificationType notificationType)
        {
            return notificationStates.TryGetValue(notificationType, out bool state) ? state : false;
        }

        public void SetNotificationState(NotificationType notificationType, bool currentState)
        {
            notificationStates[notificationType] = currentState;

            switch (notificationType)
            {
                case NotificationType.SolarPanelsIneffectiveMessage:
                    SolarPanelsIneffectiveMessageToggle = currentState;
                    break;
                case NotificationType.VesselLeftCommunicationRangeMessage:
                    VesselLeftCommunicationRangeMessageToggle = currentState;
                    break;
                case NotificationType.VesselThrottleLockedDueToTimewarpingMessage:
                    VesselThrottleLockedDueToTimewarpingMessageToggle = currentState;
                    break;
                case NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage:
                    CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle = currentState;
                    break;
                case NotificationType.GamePauseToggledMessage:
                    GamePauseToggledMessageToggle = currentState;
                    break;
            }
        }
        public void UpdateCurrentStates()
        {
            mainPlugin.SetNotificationState(NotificationType.SolarPanelsIneffectiveMessage, SolarPanelsIneffectiveMessageToggle);
            mainPlugin.SetNotificationState(NotificationType.VesselLeftCommunicationRangeMessage, VesselLeftCommunicationRangeMessageToggle);
            mainPlugin.SetNotificationState(NotificationType.VesselThrottleLockedDueToTimewarpingMessage, VesselThrottleLockedDueToTimewarpingMessageToggle);
            mainPlugin.SetNotificationState(NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle);
            mainPlugin.SetNotificationState(NotificationType.GamePauseToggledMessage, GamePauseToggledMessageToggle);
        }
        public bool ListGUI()
        {
            GUI.enabled = true;

            GUILayout.Label("Notification Options:");

            SolarPanelsIneffectiveMessageToggle = GUILayout.Toggle(SolarPanelsIneffectiveMessageToggle, "Solar Panels Ineffective");
            VesselLeftCommunicationRangeMessageToggle = GUILayout.Toggle(VesselLeftCommunicationRangeMessageToggle, "Vessel Left Communication Range");
            VesselThrottleLockedDueToTimewarpingMessageToggle = GUILayout.Toggle(VesselThrottleLockedDueToTimewarpingMessageToggle, "Vessel Throttle Locked Due to Timewarping");
            CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle = GUILayout.Toggle(CannotPlaceManeuverNodeWhileOutOfFuelMessageToggle, "Cannot Place Maneuver Node While Out of Fuel");
            GamePauseToggledMessageToggle = GUILayout.Toggle(GamePauseToggledMessageToggle, "Game Pause Toggled");

            // Update the notification states based on the toggle options
            UpdateCurrentStates();

            return true; // Indicate that a change has occurred
        }
        public enum NotificationType
        {
            SolarPanelsIneffectiveMessage,
            VesselLeftCommunicationRangeMessage,
            VesselThrottleLockedDueToTimewarpingMessage,
            CannotPlaceManeuverNodeWhileOutOfFuelMessage,
            GamePauseToggledMessage,
            None
        }
    }
}