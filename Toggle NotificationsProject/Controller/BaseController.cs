﻿using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using UnityEngine;

namespace ToggleNotifications.Controller
{
    public class BaseController : IPageContent
    {
        protected ToggleNotificationsPlugin mainPlugin;
        protected NotificationToggle notificationToggle;
        protected WindowTool windowTool;

        public bool uiVisible = false;
        private bool isActive = false;
        private bool isRunning = false;

        internal BaseController()
        {
            mainPlugin = ToggleNotificationsPlugin.Instance;
            windowTool = new WindowTool();
        }

        public string name = "No Name";
        public string Name => name;
        public bool need_update => uiVisible || isRunning;

        public virtual bool IsRunning => isRunning;

        public bool IsActive => isActive;

        public bool UIVisible
        {
            get => uiVisible;
            set => uiVisible = value;
        }
        public virtual GUIContent Icon => Icon;
        public virtual void onReset()
        {
        }
        public virtual void OnGUI()
        {
            if (isActive)
            {
                uiVisible = true;
            }
        }

        public virtual void Update()
        {
        }

        public virtual void LateUpdate()
        {
        }

        public virtual void FixedUpdate()
        {
        }
    }
}
