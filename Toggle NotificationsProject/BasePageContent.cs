using KSP.Game;
using KSP.Messages;
using SpaceWarp.API.Assets;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    public class BasePageContent : IPageContent
    {
        public BasePageContent()
        {
            this.MainUI = ToggleNotificationsUI.Instance;
            this.Plugin = ToggleNotificationsPlugin.Instance;
        }

        protected ToggleNotificationsPlugin Plugin;
        protected ToggleNotificationsUI MainUI;
        protected MessageCenterMessage Refreshing => MainUI.Refreshing;
        protected NotificationEvents RefreshingNotification => MainUI.RefreshingNotification;

        public virtual string Name => throw new NotImplementedException();
        public virtual GUIContent Icon => throw new NotImplementedException();
        public bool IsRunning => false;
        bool uiVisible;
        public bool UIVisible { get => this.uiVisible; set => this.uiVisible = value; }
        public virtual bool IsActive => throw new NotImplementedException();
        MessageCenterMessage IPageContent.ConvertToMessageCenterMessage(NotificationToggle toggleState)
        {
            return null;
        }
        public virtual void OnGUI()
        {
            throw new NotImplementedException();
        }
    }
    public class ToggleNotificationsPage : BasePageContent
    {
        public override string Name => "Toggle Notifications";
        readonly Texture2D tabIcon = AssetManager.GetAsset<Texture2D>($"{ToggleNotificationsPlugin.Instance.SpaceWarpMetadata.ModID}/images/tn_icon_50.png");
        public override GUIContent Icon => new(tabIcon, "Toggle Notifications");
        public override bool IsActive => true;
        //public int PageIndex { get; set; }
        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Toggle Notifications");
            //Option.Instance.ToggleNotificationsGUI();
            // MainUI.DrawToggleButton("Enable", NotificationType.GamePauseToggledMessage);
        }
    }
}

