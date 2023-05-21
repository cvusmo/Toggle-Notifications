using KSP.Messages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class ToggleNotificationsUI : MonoBehaviour
    {
        internal static ToggleNotificationsUI instance;
        internal static NotificationToggle toggleNotification;
        internal ToggleNotificationsPlugin mainPlugin;
        internal ToggleNotificationsUI(ToggleNotificationsPlugin mainPlugin, bool isGUIVisible, MessageCenter messageCenter)
        {
            if (mainPlugin != null)
            {
                instance = this;
                this.mainPlugin = mainPlugin;
                this.messageCenter = messageCenter;
            }
            else
            {
                Debug.LogError("ToggleNotificationsPlugin instance is null. ToggleNotificationUI");
            }
        }

        private MessageCenter messageCenter;
        private void Start()
        {
            messageCenter = FindObjectOfType<MessageCenter>();

            if (messageCenter == null)
            {
                Debug.LogError("messageCenter is null");
            }
        }

        private int selectedButton1 = 1;
        private int selectedButton2 = 1;
        private int selectedButton3 = 1;
        private int selectedButton4 = 1;

        private bool isGamePaused; // Initial toggle state
        //private bool isPauseVisible;
        //private bool isPausePublish;
        private void ButtonToggle1(int toggleValue)
        {
            if (mainPlugin == null || mainPlugin.notificationToggle == null)
            {
                Debug.LogError("mainPlugin or mainPlugin.notificationToggle is null");
                return;
            }

            if (messageCenter == null)
            {
                Debug.LogError("messageCenter is null");
                return;
            }


            selectedButton1 = toggleValue;

            if (selectedButton1 == 1)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, true);
                AssistantToTheAssistantPatchManager.isGamePaused = true;
                AssistantToTheAssistantPatchManager.isPauseVisible = true;
                AssistantToTheAssistantPatchManager.isPausePublish = true;
                GamePauseToggledMessage message = new GamePauseToggledMessage();
                message.IsPaused = true;

                // Publish the message using the MessageCenter instance
                messageCenter.Publish(message);
            }
            else if (selectedButton1 == 0)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
                AssistantToTheAssistantPatchManager.isGamePaused = false;
                AssistantToTheAssistantPatchManager.isPauseVisible = false;
                AssistantToTheAssistantPatchManager.isPausePublish = false;
                GamePauseToggledMessage message = new GamePauseToggledMessage();
                message.IsPaused = false;

                messageCenter.Publish(message);
            }


        }
        private void ButtonToggle2(int toggleValue)
        {
            selectedButton2 = toggleValue;

            if (selectedButton2 == 1)
            {
                //mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, true);
            }
            else if (selectedButton2 == 0)
            {
                //mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
            }
        }

        private void ButtonToggle3(int toggleValue)
        {
            selectedButton3 = toggleValue;

            if (selectedButton3 == 1)
            {
                // mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, true);
            }
            else if (selectedButton3 == 0)
            {
                // mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
            }
        }
        private void ButtonToggle4(int toggleValue)
        {
            selectedButton4 = toggleValue;

            if (selectedButton4 == 1)
            {
                //mainPlugin.notificationToggle.CheckCurrentState(NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, true);
            }
            else if (selectedButton4 == 0)
            {
                //mainPlugin.notificationToggle.CheckCurrentState(NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, false);
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
                // Handle the gear button action here if needed
            }

            GUILayout.EndHorizontal();

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            // Notification Toggle Buttons
            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            // Group 2: Toggle Buttons
            GUILayout.BeginVertical(GUILayout.Height(60));

            int buttonWidth = (int)(mainPlugin.windowRect.width - 12); // Subtract 3 on each side for padding

            bool gamePauseToggle = GUI.Toggle(new Rect(3, 56, buttonWidth, 20), isGamePaused, "Game Pause", selectedButton2 == 1 ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError);
            TNBaseStyle.Toggle.normal.textColor = selectedButton1 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedButton1 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (gamePauseToggle != isGamePaused)
            {
                isGamePaused = gamePauseToggle;
                ButtonToggle1(gamePauseToggle ? 1 : 0);
            }

            bool radioButton2 = GUI.Toggle(new Rect(3, 96, buttonWidth, 20), selectedButton2 == 1, "Solar Panel Ineffective", selectedButton2 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedButton2 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedButton2 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton2)
            {
                selectedButton2 = 1;
            }
            else
            {
                selectedButton2 = 0;
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

            GUILayout.EndVertical();

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

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

            GUILayout.Label("v0.2.2", nameLabelStyle);

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));

            mainPlugin.saverectpos();
        }
    }
}