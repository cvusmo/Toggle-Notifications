using SpaceWarp.API.Assets;
using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using UnityEngine;

namespace ToggleNotifications
{
    public class BasePageContent : IPageContent
    {
        protected ToggleNotificationsPlugin mainPlugin;
        protected NotificationToggle notificationToggle;
        protected WindowTool windowTool;
        internal BasePageContent()
        {
            this.mainPlugin = ToggleNotificationsPlugin.Instance;
            this.windowTool = new WindowTool();
        }

        public virtual string Name => throw new System.NotImplementedException();
        public virtual GUIContent Icon => throw new System.NotImplementedException();
        public bool IsRunning => false;
        public bool UIVisible { get; set; }
        public bool IsActive => mainPlugin._interfaceEnabled;
        public virtual void OnGUI()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ToggleNotificationsPage : BasePageContent
    {
        public override string Name => "Toggle Notifications";
        readonly Texture2D tabIcon = AssetManager.GetAsset<Texture2D>($"{ToggleNotificationsPlugin.Instance.SpaceWarpMetadata.ModID}/images/tn_icon_50.png");
        public override GUIContent Icon => new(tabIcon, "Toggle Notifications");

        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Toggle Notifications");
            //Option.Instance.ToggleNotificationsGUI();
            // MainUI.DrawToggleButton("Enable", NotificationType.GamePauseToggledMessage);
        }
    }
}
