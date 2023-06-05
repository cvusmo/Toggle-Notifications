using KSP.Messages;
using ToggleNotifications.Buttons;
using ToggleNotifications.Controller;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications.Pages
{
    internal class MainPage : BaseController
    {
        private MessageCenter messageCenter;
        private Dictionary<NotificationType, bool> notificationStates = new Dictionary<NotificationType, bool>();

        //buttons
        private GamePauseButton gamePauseButton;
        private SolarPanelButton solarPanelButton;
        private ElectricityButton outOfElectricityButton;
        private VesselLostControlButton vesselLostControlButton;
        private CommunicationRangeButton communicationRangeButton;
        private OutOfFuelButton outOfFuelButton;

        public MainPage(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            this.notificationToggle = notificationToggle;

            gamePauseButton = new GamePauseButton(mainPlugin, notificationToggle);
            solarPanelButton = new SolarPanelButton(mainPlugin, messageCenter, notificationToggle);
            outOfElectricityButton = new ElectricityButton(mainPlugin, messageCenter, notificationToggle);
            vesselLostControlButton = new VesselLostControlButton(mainPlugin, messageCenter, notificationToggle);
            communicationRangeButton = new CommunicationRangeButton(mainPlugin, messageCenter, notificationToggle);
            outOfFuelButton = new OutOfFuelButton(mainPlugin, messageCenter, notificationToggle);
        }

        public override void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(GUILayout.Height(60));

            gamePauseButton.OnGUI();
            solarPanelButton.OnGUI();
            outOfElectricityButton.OnGUI();
            vesselLostControlButton.OnGUI();
            communicationRangeButton.OnGUI();
            outOfFuelButton.OnGUI();

            GUILayout.EndVertical();

            // Group 3: Version Number
            GUIStyle nameLabelStyle = new GUIStyle()
            {
                border = new RectOffset(3, 3, 5, 5),
                padding = new RectOffset(3, 3, 4, 4),
                overflow = new RectOffset(0, 0, 0, 0),
                normal = { textColor = ColorTools.ParseColor("#C0C1E2") },
                alignment = TextAnchor.MiddleRight
            };

            GUILayout.FlexibleSpace();

            GUILayout.Label("v0.2.5", nameLabelStyle);

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            GUILayout.EndVertical();
        }
    }
}
