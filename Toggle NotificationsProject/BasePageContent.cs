using KSP.Game;
using KSP.Messages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    public class BasePageContent : IPageContent
    {
        protected ToggleNotificationsUI MainUI;
        protected ToggleNotificationsPlugin Plugin;
        private bool uiVisible;
        public BasePageContent()
        {
            MainUI = ToggleNotificationsUI.Instance;
            this.Plugin = ToggleNotificationsPlugin.Instance;
        }
        protected MessageCenterMessage Refreshing => MainUI.Refreshing;
        protected NotificationEvents RefreshingNotification => MainUI.RefreshingNotification;
        public void CheckCurrentState()
        {
            // Check the refreshing state of the UI
            bool isRefreshing = Refreshing != null;

            // Check the refreshing state of notifications
            bool isRefreshingNotification = RefreshingNotification != null;

            // Use the values as needed
            Debug.Log($"UI Refreshing: {isRefreshing}");
            Debug.Log($"Notification Refreshing: {isRefreshingNotification}");
        }

        // Assuming you have some default values for these.
        public virtual string Name => throw new NotImplementedException();
        public virtual GUIContent Icon => throw new NotImplementedException();
        MessageCenterMessage IPageContent.ConvertToMessageCenterMessage(NotificationToggle currentState)
        {
            //throw new NotImplementedException();
            return null;
        }
        public bool IsRunning => false;
        public bool UIVisible
        {
            get => this.uiVisible;
            set => this.uiVisible = value;
        }
        public virtual bool IsActive => throw new NotImplementedException();
        public virtual void OnGUI() => throw new NotImplementedException();
    }
}