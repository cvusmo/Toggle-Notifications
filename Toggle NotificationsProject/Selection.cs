using ToggleNotifications.TNTools.UI;

using UnityEngine;
using static ToggleNotifications.TNTools.UI.NotificationToggle;

namespace ToggleNotifications
{
    public class Selection
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;

        public Selection(ToggleNotificationsPlugin mainPlugin)
        {
            this.mainPlugin = mainPlugin;
            notificationToggle = mainPlugin.notificationToggle;
        }

        private bool selecting;
        public static bool TargetSelection;

        public object setOption { get; private set; }
        public ToggleNotificationsPlugin MainPlugin { get; }

        public bool IsSolarPanelSelected() => notificationToggle.GetNotificationState(NotificationType.SolarPanelsIneffectiveMessage);
        public bool IsCommRangeSelected() => notificationToggle.GetNotificationState(NotificationType.VesselLeftCommunicationRangeMessage);
        public bool IsThrottleLockedSelected() => notificationToggle.GetNotificationState(NotificationType.VesselThrottleLockedDueToTimewarpingMessage);
        public bool IsGamePauseToggledSelected() => notificationToggle.GetNotificationState(NotificationType.GamePauseToggledMessage);
        private void ListSelection()
        {
            // GetNotificationState
            bool isSolarPanelSelected = notificationToggle.GetNotificationState(NotificationType.SolarPanelsIneffectiveMessage);
            bool isCommRangeSelected = notificationToggle.GetNotificationState(NotificationType.VesselLeftCommunicationRangeMessage);
            bool isThrottleLockedSelected = notificationToggle.GetNotificationState(NotificationType.VesselThrottleLockedDueToTimewarpingMessage);
            bool isGamePauseToggledSelected = notificationToggle.GetNotificationState(NotificationType.GamePauseToggledMessage);

            // SetNotificationState
            notificationToggle.SetNotificationState(NotificationType.SolarPanelsIneffectiveMessage, isSolarPanelSelected);
            notificationToggle.SetNotificationState(NotificationType.VesselLeftCommunicationRangeMessage, isCommRangeSelected);
            notificationToggle.SetNotificationState(NotificationType.VesselThrottleLockedDueToTimewarpingMessage, isThrottleLockedSelected);
            notificationToggle.SetNotificationState(NotificationType.GamePauseToggledMessage, isGamePauseToggledSelected);

            // Handle the selection of notification events
            // ...
        }
        internal bool ListGUI()
        {
            if (!selecting)
                return false;

            GUILayout.Label("Notification Options:");

            // Toggle options for each notification
            GUILayout.BeginVertical();

            bool isSolarPanelSelected = notificationToggle.GetNotificationState(NotificationType.SolarPanelsIneffectiveMessage);
            isSolarPanelSelected = GUILayout.Toggle(isSolarPanelSelected, "Solar Panels Ineffective");
            SetOption solarPanelsOption = new SetOption(NotificationType.SolarPanelsIneffectiveMessage.ToString(), isSolarPanelSelected ? "Enabled" : "Disabled");
            if (GUILayout.Button(solarPanelsOption.SetOptionsList(isSolarPanelSelected ? "Enabled" : "Disabled")))
            {
                solarPanelsOption.ProcessOption();
            }

            bool isCommRangeSelected = notificationToggle.GetNotificationState(NotificationType.VesselLeftCommunicationRangeMessage);
            isCommRangeSelected = GUILayout.Toggle(isCommRangeSelected, "Vessel Left Communication Range");
            SetOption commRangeOption = new SetOption(NotificationType.VesselLeftCommunicationRangeMessage.ToString(), isCommRangeSelected ? "Enabled" : "Disabled");
            if (GUILayout.Button(commRangeOption.SetOptionsList(isCommRangeSelected ? "Enabled" : "Disabled")))
            {
                commRangeOption.ProcessOption();
            }

            bool isThrottleLockedSelected = notificationToggle.GetNotificationState(NotificationType.VesselThrottleLockedDueToTimewarpingMessage);
            isThrottleLockedSelected = GUILayout.Toggle(isThrottleLockedSelected, "Vessel Throttle Locked Due to Timewarping");
            SetOption throttleLockedOption = new SetOption(NotificationType.VesselThrottleLockedDueToTimewarpingMessage.ToString(), isThrottleLockedSelected ? "Enabled" : "Disabled");
            if (GUILayout.Button(throttleLockedOption.SetOptionsList(isThrottleLockedSelected ? "Enabled" : "Disabled")))
            {
                throttleLockedOption.ProcessOption();
            }

            bool isGamePauseToggledSelected = notificationToggle.GetNotificationState(NotificationType.GamePauseToggledMessage);
            isGamePauseToggledSelected = GUILayout.Toggle(isGamePauseToggledSelected, "Game Pause Toggled");
            SetOption gamePauseOption = new SetOption(NotificationType.GamePauseToggledMessage.ToString(), isGamePauseToggledSelected ? "Enabled" : "Disabled");
            if (GUILayout.Button(gamePauseOption.SetOptionsList(isGamePauseToggledSelected ? "Enabled" : "Disabled")))
            {
                gamePauseOption.ProcessOption();
            }

            GUILayout.EndVertical();

            if (GUILayout.Button("Close"))
                selecting = false;

            return true;
        }


        public void TargetSelectionGUI()
        {
            GUILayout.Toggle(false, "Select Notifications"); // Toggle button for selecting notifications
        }
    }
}
