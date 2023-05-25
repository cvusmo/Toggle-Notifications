using KSP.Messages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class GamePauseButton
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;
        private MessageCenter messageCenter;
        private SubscriptionHandle gamePauseHandle;
        private bool pauseToggled;
        private bool gamePauseNotificationEnabled;

        public GamePauseButton(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            this.notificationToggle = notificationToggle;

            pauseToggled = true;
            gamePauseNotificationEnabled = true;

            // Subscribe to the GamePauseToggledMessage and define the callback logic
            gamePauseHandle = messageCenter.Subscribe<GamePauseToggledMessage>(GamePauseToggledMessageCallback);
        }

        public void GamePauseToggledMessageCallback(MessageCenterMessage msg)
        {
            GamePauseToggledMessage pauseToggledMessage = msg as GamePauseToggledMessage;
            if (pauseToggledMessage != null)
            {
                gamePauseNotificationEnabled = pauseToggledMessage.IsPaused;
                RenderToggle();
            }
        }

        public void RenderToggle()
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

                // Update the notification state
                notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, pauseToggled);
            }
        }
    }
}
