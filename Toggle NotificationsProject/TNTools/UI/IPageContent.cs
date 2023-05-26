using UnityEngine;

namespace ToggleNotifications.UI
{
    internal interface IPageContent
    {
        internal string Name { get; }
        internal bool IsRunning { get; }
        internal bool IsActive { get; }
        internal bool UIVisible { get; set; }
        internal void OnGUI();
    }
}