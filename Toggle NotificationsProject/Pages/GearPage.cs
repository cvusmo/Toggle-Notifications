using ToggleNotifications.Controller;
using ToggleNotifications.TNTools;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications.Pages
{
    internal class GearPage : BaseController
    {
        internal ToggleNotificationsPlugin mainPlugin;
        internal NotificationToggle notificationToggle;
        private static float pageHeight = 100f;
        private static float buttonHeight = 20f;
        private bool uiVisible = false;
        private bool isActive = false;
        internal static bool showOptions = false;
        internal string Name => "Gear Page";
        internal GUIContent Icon => new GUIContent();
        internal bool IsRunning => false;
        internal bool IsActive => isActive;

        internal bool UIVisible
        {
            get => uiVisible;
            set => uiVisible = value;
        }

        internal static bool settings_mode
        {
            get => TNBaseSettings.SFile.GetBool(nameof(settings_mode), false);
            set => TNBaseSettings.SFile.SetBool(name: nameof(settings_mode), value);
        }
        public GearPage(ToggleNotificationsPlugin mainPlugin, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.notificationToggle = notificationToggle;
        }
        internal void CloseSettings()
        {
            GearPage instance = new GearPage(mainPlugin, notificationToggle);
            instance.uiVisible = false;
        }
        internal static float GetContentHeight(NotificationToggle notificationToggle)
        {
            float contentHeight = pageHeight;

            if (settings_mode)
            {
                contentHeight += buttonHeight * notificationToggle.GetNotificationCount();
            }

            return contentHeight;
        }

        internal void OnGUI(int windowID)
        {
            GUILayout.BeginVertical(GUILayout.Height(pageHeight));

            if (GUILayout.Button("Close Settings"))
            {
                uiVisible = false;
            }

            GUILayout.FlexibleSpace();
            settings_mode = GUILayout.Toggle(settings_mode, "Notification", GUILayout.ExpandWidth(false));

            if (settings_mode)
            {
                notificationToggle.ListGUI();
            }

            GUILayout.EndVertical();
            GUILayout.Space(5);

            GUI.DragWindow();
        }
    }
}
