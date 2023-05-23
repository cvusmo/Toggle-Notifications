using KSP.Messages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class GamePauseButton
    {
        private ToggleNotificationsPlugin mainPlugin;
        private MessageCenter messageCenter;
        private SubscriptionHandle gamePauseHandle;
        private bool pauseToggled;

        public GamePauseButton(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            pauseToggled = false;

            // Subscribe to the GamePauseToggledMessage and define the callback logic
            gamePauseHandle = messageCenter.Subscribe<GamePauseToggledMessage>(GamePauseToggledMessageCallback);
        }

        private void GamePauseToggledMessageCallback(MessageCenterMessage msg)
        {
            GamePauseToggledMessage pauseToggledMessage = msg as GamePauseToggledMessage;
            if (pauseToggledMessage != null)
            {
                // Update the notification display based on the selectedGamePause value
                if (pauseToggledMessage.IsPaused)
                {
                    mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
                    AssistantToTheAssistantPatchManager.isGamePaused = false;
                    

                    pauseToggled = true;

                    // Set the GUI style for TNBaseStyle.Toggle
                    TNBaseStyle.Toggle.normal.textColor = ColorTools.ParseColor("#C0C1E2");
                    TNBaseStyle.ToggleError.normal.textColor = ColorTools.ParseColor("#C0E2DC");
                }
                else
                {
                    mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, true);
                    AssistantToTheAssistantPatchManager.isGamePaused = true;
                    

                    pauseToggled = false;

                    // Set the GUI style for TNBaseStyle.ToggleError
                    TNBaseStyle.Toggle.normal.textColor = ColorTools.ParseColor("#C0E2DC");
                    TNBaseStyle.ToggleError.normal.textColor = Color.red;
                }
            }
        }

        public void RenderToggle()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12); // Subtract 3 on each side for padding
            Rect gamePauseToggleRect = new Rect(3, 56, buttonWidth, 20);

            GUIStyle toggleStyle = pauseToggled ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = pauseToggled ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");

            bool gamePauseToggle = GUI.Toggle(gamePauseToggleRect, pauseToggled, "Game Pause", toggleStyle);
            toggleStyle.normal.textColor = textColor;

            if (gamePauseToggle != pauseToggled)
            {
                pauseToggled = gamePauseToggle;
                GamePauseButtonToggle(pauseToggled ? 1 : 0);
            }
        }

        private void GamePauseButtonToggle(int toggleValue)
        {
            // Handle the toggle action
            if (mainPlugin == null || mainPlugin.notificationToggle == null)
            {
                Debug.LogError("mainPlugin or mainPlugin.notificationToggle is null GamePauseButtonToggle");
                return;
            }

            if (messageCenter == null)
            {
                Debug.LogError("messageCenter is null. GamePauseButtonToggle.");
                return;
            }

            if (toggleValue == 1)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
                AssistantToTheAssistantPatchManager.isGamePaused = false;

                Debug.Log($"Toggle 1 disabled");
                Debug.Log($"Initial isToggled value: {toggleValue}");
                Debug.Log($"Initial isGamePaused value: {AssistantToTheAssistantPatchManager.isGamePaused}");
            }
            else if (toggleValue == 0)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, true);
                AssistantToTheAssistantPatchManager.isGamePaused = true;
                

                Debug.Log($"Toggle 1 enabled");
                Debug.Log($"Initial isToggled value: {toggleValue}");
                Debug.Log($"Initial isGamePaused value: {AssistantToTheAssistantPatchManager.isGamePaused}");

            }
        }
    }
}