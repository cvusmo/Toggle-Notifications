using KSP.Messages;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public interface IPageContent
    {
        string Name { get; }
        GUIContent Icon { get; }
        bool IsRunning { get; }
        bool IsActive { get; }
        bool UIVisible { get; set; }
        void OnGUI();
    }
}