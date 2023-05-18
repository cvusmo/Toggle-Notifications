using SpaceWarp.API.Assets;
using UnityEngine;

namespace ToggleNotifications
{
    public class MainPage : BasePageContent
    {
        private readonly Texture2D tabIcon = AssetManager.GetAsset<Texture2D>($"togglenotifications/images/tn_icon_50.png");
        public override string Name => "Toggle Notifications";
        public override GUIContent Icon => new GUIContent((Texture) this.tabIcon, "Toggle Notifications");
        public override bool IsActive => true;

        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Toggle Notifications");
            ToggleNotificationsUI.DrawToggleButton("Enable", NotificationType.GamePauseToggledMessage);
            ToggleNotificationsUI.DrawToggleButton("Disable", NotificationType.GamePauseToggledMessage);
            ToggleNotificationsUI.DrawToggleButton("Enable", NotificationType.SolarPanelsIneffectiveMessage);
            ToggleNotificationsUI.DrawToggleButton("Disable", NotificationType.SolarPanelsIneffectiveMessage);
        }
    }
}

