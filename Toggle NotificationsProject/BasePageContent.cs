using KSP.Game;
using KSP.Messages;
using TNUtilities;
using KSP.Sim;
using KSP.Sim.impl;
using SpaceWarp.API.Assets;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    public class BasePageContent : IPageContent
    {
        protected ToggleNotificationsPlugin mainPlugin;
        private ToggleNotificationsUI MainUI;
        protected NotificationToggle notificationToggle;
        protected WindowTool windowTool;
        private bool uiVisible;
        private Rect windowRect = Rect.zero;
        private int windowWidth = 250;
        internal bool _isGUIenabled = false;
        internal BasePageContent()
        {
            this.MainUI = ToggleNotificationsUI.instance;
            this.mainPlugin = ToggleNotificationsPlugin.Instance;
            this.windowTool = new WindowTool();
        }

        public virtual string Name => throw new NotImplementedException();
        public virtual GUIContent Icon => throw new NotImplementedException();
        public bool IsRunning => false;
        public bool UIVisible { get => this._isGUIenabled; set => this._isGUIenabled = value; }
        public bool IsActive => mainPlugin._interfaceEnabled;
        public virtual void OnGUI()
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
