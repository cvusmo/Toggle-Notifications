using KSP.Messages;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public interface IPageContent
    {
        MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle toggleState);
        public string Name { get; }
        public GUIContent Icon { get; }
        public bool IsRunning { get; }
        public bool IsActive { get; }
        public bool UIVisible { get; set; }
        void OnGUI();
    }
}