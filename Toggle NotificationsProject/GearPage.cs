using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class GearPage : IPageContent
    {
        internal ToggleNotificationsPlugin mainPlugin;
        private bool uiVisible = false;
        private bool isActive = false;
        private Rect settingsWindowRect;
        private bool isFoldoutExpanded = false;

        private const int GearWindowID = 1000; // Unique window ID for the GearPage

        public string Name => "Gear Page";
        public GUIContent Icon => new GUIContent();
        public bool IsRunning => false;
        public bool IsActive => isActive;

        public bool UIVisible
        {
            get => uiVisible;
            set => uiVisible = value;
        }

        public void OnGUI()
        {
            if (IsActive && UIVisible == true)
            {
                settingsWindowRect = GUI.Window(GearWindowID, settingsWindowRect, DrawSettingsWindow, "Settings");
            }
            else
            {
                UIVisible = false; // Ensure UIVisible is set to false when not active or visible
            }
        }

        void DrawSettingsWindow(int gearWindowID)
        {
            isFoldoutExpanded = GUI.Toggle(new Rect(10, 30, 150, 30), isFoldoutExpanded, "Foldout Title", TNBaseStyle.FoldoutClose);
            if (isFoldoutExpanded)
            {
                float mainWidth = mainPlugin.windowRect.width;
                float mainHeight = mainPlugin.windowRect.height;
                float windowWidth = 250f;
                float windowHeight = 250f;
                float windowX = mainPlugin.windowRect.x + mainWidth + 10f;
                float windowY = mainPlugin.windowRect.y;

                settingsWindowRect = new Rect(windowX, windowY, windowWidth, windowHeight);

                GUI.BeginGroup(settingsWindowRect, TNBaseStyle.Skin.box);
                SettingsWindowContent(gearWindowID);
                GUI.EndGroup();
            }
        }

        void SettingsWindowContent(int gearWindowID)
        {
            GUI.Label(new Rect(10, 10, 200, 30), "Settings Window", TNBaseStyle.Label);
            // Add your settings window contents here
            GUI.DragWindow();
        }
    }
}
