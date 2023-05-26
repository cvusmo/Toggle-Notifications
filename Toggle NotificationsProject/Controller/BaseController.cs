using System.Drawing;
using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using UnityEngine;

namespace ToggleNotifications.Controller
{
    public class BaseController : IPageContent
    {
        public bool uiVisible = false;
        private bool isActive = false;
        private bool isRunning = false;

        public string name = "No Name";
        public string Name => this.name;
        public bool need_update => this.uiVisible || this.isRunning;

        public virtual bool IsRunning => isRunning;

        public bool IsActive => isActive;

        public bool UIVisible
        {
            get => this.uiVisible;
            set => this.uiVisible = value;
        }
        public virtual GUIContent Icon => this.Icon;
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
