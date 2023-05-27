using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class GamePauseButton : MonoBehaviour
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;
        private bool pauseToggled;

        public GamePauseButton(ToggleNotificationsPlugin mainPlugin, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
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
                if (gamePauseToggle)
                {
                    AssistantToTheAssistantPatchManager.IsPauseVisible = true;
                    Debug.Log("Game Pause Notifications Enabled: " + AssistantToTheAssistantPatchManager.IsPauseVisible);
                }
                else
                {
                    AssistantToTheAssistantPatchManager.IsPauseVisible = false;
                    Debug.Log("Game Pause Notifications Disabled: " + AssistantToTheAssistantPatchManager.IsPauseVisible);
                }

                pauseToggled = gamePauseToggle;
                notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, AssistantToTheAssistantPatchManager.IsPauseVisible);
            }
        }

        private void Update()
        {
            OnGUI();
        }
    }
}
