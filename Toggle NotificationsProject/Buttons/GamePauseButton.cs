using ToggleNotifications.PatchManager;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications.Buttons
{
    internal class GamePauseButton : MonoBehaviour
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;

        public GamePauseButton(ToggleNotificationsPlugin mainPlugin, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.notificationToggle = notificationToggle;
        }

        internal void OnGUI()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12);
            Rect gamePauseToggleRect = new Rect(3, 60, buttonWidth, 20);

            GUIStyle toggleStyle = AssistantToTheAssistantPatchManager.IsPauseVisible ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = AssistantToTheAssistantPatchManager.IsPauseVisible ? ColorTools.ParseColor("#C0C1E2") : Color.red;

            toggleStyle.normal.textColor = textColor;

            bool gamePauseToggle = GUI.Toggle(gamePauseToggleRect, AssistantToTheAssistantPatchManager.IsPauseVisible, "Game Pause", toggleStyle);

            if (gamePauseToggle != AssistantToTheAssistantPatchManager.IsPauseVisible)
            {
                AssistantToTheAssistantPatchManager.IsPauseVisible = gamePauseToggle;
                Debug.Log("Game Pause Notifications " + (AssistantToTheAssistantPatchManager.IsPauseVisible ? "Enabled" : "Disabled"));
                notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, AssistantToTheAssistantPatchManager.IsPauseVisible);
            }
        }

        private void Update()
        {
            OnGUI();
        }
    }
}
