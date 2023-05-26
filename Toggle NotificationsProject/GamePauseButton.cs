using KSP.Messages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class GamePauseButton : MonoBehaviour
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;
        private MessageCenter messageCenter;
        private bool pauseToggled;

        public GamePauseButton(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            this.notificationToggle = notificationToggle;

            pauseToggled = true;

            messageCenter.Subscribe<GamePauseToggledMessage>(GamePauseToggledMessageCallback);
        }

        private void GamePauseToggledMessageCallback(MessageCenterMessage msg)
        {
            GamePauseToggledMessage pauseToggledMessage = msg as GamePauseToggledMessage;
            if (pauseToggledMessage != null)
            {

                // Update the pauseToggled value based on external changes
                pauseToggled = !pauseToggledMessage.IsPaused;
            }
        }

        public void OnGUI()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12);
            Rect gamePauseToggleRect = new Rect(3, 56, buttonWidth, 20);

            GUIStyle toggleStyle = AssistantToTheAssistantPatchManager.isGamePaused ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = AssistantToTheAssistantPatchManager.isGamePaused ? ColorTools.ParseColor("#C0C1E2") : Color.red;

            toggleStyle.normal.textColor = textColor;

            bool gamePauseToggle = GUI.Toggle(gamePauseToggleRect, AssistantToTheAssistantPatchManager.isGamePaused, "Game Pause", toggleStyle);

            if (gamePauseToggle != AssistantToTheAssistantPatchManager.isGamePaused)
            {
                mainPlugin.EnableGamePauseNotification(!gamePauseToggle);
                AssistantToTheAssistantPatchManager.isGamePaused = gamePauseToggle;

                notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, !gamePauseToggle);
            }
            else if (!AssistantToTheAssistantPatchManager.isGamePaused)
            {
                // Apply the ToggleError style if the button is still disabled
                toggleStyle = TNBaseStyle.ToggleError;
                toggleStyle.normal.textColor = Color.red;

                // Disable the game pause notifications
                mainPlugin.EnableGamePauseNotification(true);
                notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
            }
        }
    }
}
