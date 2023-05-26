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

                if (pauseToggled)
                {
                    // Disable the game pause notifications
                    AssistantToTheAssistantPatchManager.isPauseVisible = true;
                    AssistantToTheAssistantPatchManager.isGamePaused = true;

                    notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
                }
                else
                {
                    // Enable the game pause notifications
                    AssistantToTheAssistantPatchManager.isPauseVisible = false;
                    AssistantToTheAssistantPatchManager.isGamePaused = false;

                    toggleStyle = TNBaseStyle.ToggleError;
                    toggleStyle.normal.textColor = Color.red;
                    notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, true);
                }
            }
            notificationToggle.ListGUI();
        }
        internal void Update()
        {
            OnGUI();
        }
    }

}

