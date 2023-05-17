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


        // Assuming you have some default values for these.
        public virtual string Name => throw new NotImplementedException();
        public virtual GUIContent Icon => throw new NotImplementedException();
        MessageCenterMessage IPageContent.ConvertToMessageCenterMessage(NotificationToggle toggleState)
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
        public virtual bool IsActive
        {
            get { return uiVisible; }
        }
        public virtual void OnGUI()
        {
            // Define your GUI elements and interactions here
            GUILayout.Label("Toggle Notifications");

            if (GUILayout.Button("Click Me"))
            {
                // Handle button click
                Debug.Log("Button Clicked!");
            }
        }

    }
}