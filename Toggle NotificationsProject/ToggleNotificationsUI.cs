using KSP.Messages;
using KSP.Sim.Definitions;
using ToggleNotifications.Buttons;
using ToggleNotifications.Controller;
using ToggleNotifications.Pages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class ToggleNotificationsUI : MonoBehaviour
    {
        internal ToggleNotificationsPlugin mainPlugin;
        internal static ToggleNotificationsUI instance;
        private MessageCenter messageCenter;
        private PartBehaviourModule partBehaviourModule;
        private SubscriptionHandle _onActionActivateMessageHandle;
        private Dictionary<NotificationType, bool> notificationStates = new Dictionary<NotificationType, bool>();
        private NotificationToggle notificationToggle;

        //controllers
        private ButtonController buttonController;
        private BaseController baseController;

        //pages
        private GearPage gearPage;

        //buttons
        private GamePauseButton gamePauseButton;
        private SolarPanelButton solarPanelButton;
        private ElectricityButton outOfElectricityButton;
        private VesselLostControlButton vesselLostControlButton;
        private CommunicationRangeButton communicationRangeButton;
        private OutOfFuelButton outOfFuelButton;


        internal int selectedButton6 = 1;

        internal bool isToggled6;

        //options
        public void SetShowOptions(bool show)
        {
            GearPage.showOptions = show;
        }
        public ToggleNotificationsUI(ToggleNotificationsPlugin mainPlugin, bool _isGUIenabled, MessageCenter messageCenter)
        {
            instance = this;
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            gearPage = new GearPage();

            partBehaviourModule = FindObjectOfType<PartBehaviourModule>();

            notificationToggle = new NotificationToggle(mainPlugin, notificationStates);

            gamePauseButton = new GamePauseButton(mainPlugin, notificationToggle);
            solarPanelButton = new SolarPanelButton(mainPlugin, messageCenter, notificationToggle);
            outOfElectricityButton = new ElectricityButton(mainPlugin, messageCenter, notificationToggle);
            vesselLostControlButton = new VesselLostControlButton(mainPlugin, messageCenter, notificationToggle);
            communicationRangeButton = new CommunicationRangeButton(mainPlugin, messageCenter, notificationToggle);
            outOfFuelButton = new OutOfFuelButton(mainPlugin, messageCenter, notificationToggle);
        }
        internal void FillWindow(int windowID)
        {
            TopButtons.Init(mainPlugin.windowRect.width);

            GUILayout.BeginHorizontal();

            // MENU BAR
            GUILayout.FlexibleSpace();

            GUI.Label(new Rect(10f, 4f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            Rect closeButtonPosition = new Rect(mainPlugin.windowRect.width - 10, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);

            if (TopButtons.Button(TNBaseStyle.Cross))
                mainPlugin.CloseWindow();

            GUILayout.Space(10);

            if (TopButtons.Button(TNBaseStyle.Gear))
            {
                gearPage.UIVisible = !gearPage.UIVisible;
                Debug.Log("Gear Button Status: " + gearPage.UIVisible);
            }

            GUILayout.EndHorizontal();

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            // Group 2: Toggle Buttons
            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();
            
            GUILayout.BeginVertical(GUILayout.Height(60));

            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12); // Subtract 3 on each side for padding
            //int buttonHeight = 20;
            //int buttonSpacing = 10;
            //int currentY = 56; 

            gamePauseButton.OnGUI();
            //currentY += buttonHeight + buttonSpacing;

            solarPanelButton.OnGUI();
            //currentY += buttonHeight + buttonSpacing;

            outOfElectricityButton.OnGUI();
            //currentY += buttonHeight + buttonSpacing;

            vesselLostControlButton.OnGUI();
            //currentY += buttonHeight + buttonSpacing;

            communicationRangeButton.OnGUI();
            //currentY += buttonHeight + buttonSpacing;

            outOfFuelButton.OnGUI();
            //currentY += buttonHeight + buttonSpacing;


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

            GUILayout.Label("v0.2.4", nameLabelStyle);

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));

            //mainPlugin.windowRect.height = currentY + buttonHeight + buttonSpacing;
            mainPlugin.saverectpos();
        }

    }
}