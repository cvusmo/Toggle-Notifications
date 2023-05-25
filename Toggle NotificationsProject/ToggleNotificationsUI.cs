using KSP.Messages;
using KSP.Sim.Definitions;
using ToggleNotifications.Controller;
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
        private GamePauseButton gamePauseButton;
        private GearPage gearPage;

        //buttons
        internal int selectedSolarIneffective = 1;
        internal int selectedButton3 = 1;
        internal int selectedButton4 = 1;
        internal int selectedButton5 = 1;
        internal int selectedButton6 = 1;

        //toggles
        internal bool pauseToggled;
        internal bool toggledSolar;
        internal bool isToggled3;
        internal bool isToggled4;
        internal bool isToggled5;
        internal bool isToggled6;
        public ToggleNotificationsUI(ToggleNotificationsPlugin mainPlugin, bool _isGUIenabled, MessageCenter messageCenter)
        {
            instance = this;
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            gearPage = new GearPage();

            // Initialize variables
            partBehaviourModule = FindObjectOfType<PartBehaviourModule>();

            // Initialize NotificationToggle
            notificationToggle = new NotificationToggle(mainPlugin, notificationStates);

            bool toggledSolar = selectedSolarIneffective == 0;
            notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, toggledSolar);

            // Create the GamePauseButton with the notificationToggle instance
            gamePauseButton = new GamePauseButton(mainPlugin, messageCenter, notificationToggle);
        }

        private void Start()
        {
            messageCenter = FindObjectOfType<MessageCenter>();

            if (messageCenter == null)
            {
                Debug.LogError("messageCenter is null");
                return;
            }

            messageCenter.PersistentSubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);
        }

        //Solar Panel Notification
        private void SolarPanelsIneffectiveMessageCallback(MessageCenterMessage msg)
        {
            SolarPanelsIneffectiveMessage solarToggle = msg as SolarPanelsIneffectiveMessage;
            if (solarToggle != null)
            {
                if (selectedSolarIneffective == 1)
                {
                    mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
                    AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = false;
                }
                else if (selectedSolarIneffective == 0)
                {
                    mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, true);
                    AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = true;
                }
            }
        }

        private void ButtonToggle2(int toggledSolar)
        {
            if (mainPlugin == null || mainPlugin.notificationToggle == null)
            {
                Debug.LogError("mainPlugin or mainPlugin.notificationToggle is null ButtonToggle2");
                return;
            }

            if (messageCenter == null)
            {
                Debug.LogError("messageCenter is null. ButtonToggle2.");
                return;
            }

            if (selectedSolarIneffective == toggledSolar) { return; }

            selectedSolarIneffective = toggledSolar;

            if (selectedSolarIneffective == 1)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = false;
                messageCenter.Unsubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);

                Debug.Log($"Toggle 2 disabled");
                Debug.Log($"Initial isToggled2 value: {toggledSolar}");
                Debug.Log($"Initial isSolarPanelsEnabled value: {AssistantToTheAssistantPatchManager.isSolarPanelsEnabled}");
            }
            else if (selectedSolarIneffective == 0)
            {
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = true;

                // Unsubscribe from the SolarPanelsIneffectiveMessage
                messageCenter.Unsubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);

                // Create and publish the SolarPanelsIneffectiveMessage
                SolarPanelsIneffectiveMessage message = new SolarPanelsIneffectiveMessage();
                messageCenter.Publish<SolarPanelsIneffectiveMessage>(message);

                // Subscribe to the SolarPanelsIneffectiveMessage
                messageCenter.PersistentSubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);
                _onActionActivateMessageHandle = messageCenter.Subscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);

                // Set the GUI style for TNBaseStyle.Toggle
                TNBaseStyle.Toggle.normal.textColor = ColorTools.ParseColor("#C0C1E2");
                TNBaseStyle.ToggleError.normal.textColor = ColorTools.ParseColor("#C0E2DC");

                Debug.Log($"Toggle 2 enabled");
                Debug.Log($"Initial isToggled2 value: {toggledSolar}");
                Debug.Log($"Initial isSolarPanelsEnabled value: {AssistantToTheAssistantPatchManager.isSolarPanelsEnabled}");
            }
        } 
        internal void FillWindow(int windowID)
        {
            // Initialize the position of the buttons
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

            // Notification Toggle Buttons
            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            // Group 2: Toggle Buttons
            GUILayout.BeginVertical(GUILayout.Height(60));

            gamePauseButton.RenderToggle();

            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12); // Subtract 3 on each side for padding

            bool solarToggle = GUI.Toggle(new Rect(3, 96, buttonWidth, 20), AssistantToTheAssistantPatchManager.isSolarPanelsEnabled, "Solar Panel Ineffective",
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError);
            TNBaseStyle.Toggle.normal.textColor = AssistantToTheAssistantPatchManager.isSolarPanelsEnabled ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = !AssistantToTheAssistantPatchManager.isSolarPanelsEnabled ? Color.red : ColorTools.ParseColor("#C0E2DC");

            if (solarToggle != AssistantToTheAssistantPatchManager.isSolarPanelsEnabled)
            {
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = solarToggle;
                ButtonToggle2(AssistantToTheAssistantPatchManager.isSolarPanelsEnabled ? 1 : 0);
            }

            bool radioButton3 = GUI.Toggle(new Rect(3, 133, buttonWidth, 20), selectedButton3 == 1, "Out of Fuel (soon.tm)", selectedButton3 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedButton3 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedButton3 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton3)
            {
                selectedButton3 = 1;
            }
            else
            {
                selectedButton3 = 0;
            }

            bool radioButton4 = GUI.Toggle(new Rect(3, 173, buttonWidth, 20), selectedButton4 == 1, "No Electricity (soon.tm)", selectedButton4 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedButton4 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedButton4 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton4)
            {
                selectedButton4 = 1;
            }
            else
            {
                selectedButton4 = 0;
            }

            bool radioButton5 = GUI.Toggle(new Rect(3, 213, buttonWidth, 20), selectedButton5 == 1, "Out of Comms Range (soon.tm)", selectedButton5 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedButton5 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedButton5 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton5)
            {
                selectedButton5 = 1;
            }
            else
            {
                selectedButton5 = 0;
            }

            bool radioButton6 = GUI.Toggle(new Rect(3, 253, buttonWidth, 20), selectedButton6 == 1, "Dating Sim (soon.tm)", selectedButton6 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedButton6 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedButton6 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton6)
            {
                selectedButton6 = 1;
            }
            else
            {
                selectedButton6 = 0;
            }

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

            GUILayout.Label("v0.2.3", nameLabelStyle);

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));

            mainPlugin.saverectpos();
        }
    }
}
