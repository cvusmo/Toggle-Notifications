using KSP.Game;
using KSP.Messages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    public class BasePageContent : IPageContent
    {
        protected ToggleNotificationsPlugin mainPlugin;
        protected ToggleNotificationsUI MainUI;
        private bool uiVisible;
        public BasePageContent()
        {
            this.MainUI = ToggleNotificationsUI.Instance;
            this.mainPlugin = ToggleNotificationsPlugin.Instance;
        }
        protected MessageCenterMessage Refreshing => MainUI.Refreshing;
        protected NotificationEvents RefreshingNotification => MainUI.RefreshingNotification;
        public virtual string Name => throw new NotImplementedException();
        public virtual GUIContent Icon => throw new NotImplementedException();
        public bool IsRunning => false;
        public bool UIVisible { get => this.uiVisible; set => this.uiVisible = value; }
        public virtual bool IsActive => throw new NotImplementedException();
        public virtual void OnGUI() => throw new NotImplementedException();
    }
}