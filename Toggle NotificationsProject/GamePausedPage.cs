using SpaceWarp.API.Assets;
using UnityEngine;
using static ToggleNotifications.TNTools.UI.NotificationToggle;

namespace ToggleNotifications
{
    public class GamePausedPage : BasePageContent
    {
        private readonly Texture2D _tabIcon = AssetManager.GetAsset<Texture2D>(ToggleNotificationsPlugin.Instance.SpaceWarpMetadata.ModID + "/images/OTM_50_Moon.png");

        private bool isActive;

        public override string Name => "Game Paused";

        public override GUIContent Icon => new GUIContent((Texture)this._tabIcon, "Game Paused Options");

        public override bool IsActive => isActive;

        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Game Pause Notification");

            // Get the current state of the GamePauseToggledMessage
            bool gamePauseToggledMessage = ToggleNotificationsPlugin.Instance.notificationToggle.GetNotificationState(NotificationType.GamePauseToggledMessage);

            // Draw a toggle button for the GamePauseToggledMessage
            gamePauseToggledMessage = GUILayout.Toggle(gamePauseToggledMessage, "Moon Return");

            // Update the notification state based on the toggle button
            ToggleNotificationsPlugin.Instance.notificationToggle.SetNotificationState(NotificationType.GamePauseToggledMessage, gamePauseToggledMessage);

            // Update the GamePausedGUI property in the MainUI
            this.MainUI.GamePausedGUI = gamePauseToggledMessage;

            // Update the isActive flag based on the GamePauseToggledMessage state
            isActive = gamePauseToggledMessage;
        }
    }
}
