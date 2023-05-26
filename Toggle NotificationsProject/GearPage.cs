using KSP;
using KSP.Messages;
using KSP.Messages.PropertyWatchers;
using SpaceWarp.API.Assets;
using ToggleNotifications.Controller;
using ToggleNotifications.TNTools;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class GearPage : BaseController
    {
        internal ToggleNotificationsPlugin mainPlugin;
        internal NotificationToggle notificationToggle;
        private static float pageHeight = 200f;


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
        internal static void CloseSettings()
        {
            GearPage instance = new GearPage();
            instance.uiVisible = false;
        }

        internal static void OnGUI(NotificationToggle notificationToggle)
        {
            GUILayout.BeginVertical(GUILayout.Height(pageHeight));
            if (UITools.miniButton("Close Settings"))
            {
                CloseSettings();
            }

            GUILayout.FlexibleSpace();
            UITools.Console("v0.9.0");
            GUILayout.FlexibleSpace();
            settings_mode = UITools.miniToggle(settings_mode, "Notification", "Open Notification States.");

            GUILayout.EndVertical();
            UITools.Separator();

            if (settings_mode)
            {
                notificationToggle.ListGUI();
            }
        }

    }
}
