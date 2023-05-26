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
            GamePauseToggledMessage gamePauseToggledMessage = msg as GamePauseToggledMessage;
            if (gamePauseToggledMessage != null)
            {
                pauseToggled = gamePauseToggledMessage.IsPaused;
            }
        }

        internal void OnGUI()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12);
            Rect gamePauseToggleRect = new Rect(3, 56, buttonWidth, 20);

            GUIStyle toggleStyle = pauseToggled ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = pauseToggled ? ColorTools.ParseColor("#C0C1E2") : Color.red;

            toggleStyle.normal.textColor = textColor;

            bool gamePauseToggle = GUI.Toggle(gamePauseToggleRect, pauseToggled, "Game Pause", toggleStyle);

            if (gamePauseToggle != pauseToggled)
            {
                pauseToggled = gamePauseToggle;

                GamePauseToggledMessage gamePauseMessage = new GamePauseToggledMessage();
                gamePauseMessage.IsPaused = pauseToggled;
                messageCenter.Publish(gamePauseMessage);

                if (pauseToggled)
                {
                    // Enable the game pause notifications
                    AssistantToTheAssistantPatchManager.isPauseVisible = false;
                    Debug.Log("Game Pause Notifications Enabled: " + AssistantToTheAssistantPatchManager.isPauseVisible);
                }
                else
                {
                    // Disable the game pause notifications
                    AssistantToTheAssistantPatchManager.isPauseVisible = true;
                    Debug.Log("Game Pause Notifications Disabled: " + AssistantToTheAssistantPatchManager.isPauseVisible);
                }

                notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, AssistantToTheAssistantPatchManager.isPauseVisible);
            }
        }

        internal void Update()
        {
            OnGUI();
        }
    }
}
