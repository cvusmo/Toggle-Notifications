using ToggleNotifications.Controller;
using ToggleNotifications.PatchManager;
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
            uiVisible = false;
        }

        internal void ToggleVisibility()
        {
            uiVisible = !uiVisible;
        }
        internal static float GetContentHeight(NotificationToggle notificationToggle)
        {
            float contentHeight = pageHeight;

            if (settings_mode)
            {
                contentHeight += buttonHeight * notificationToggle.GetNotificationCount();
            }

            float minHeight = 50;
            float maxHeight = 200;

            contentHeight = Mathf.Clamp(contentHeight, minHeight, maxHeight);

            return contentHeight;
        }
        internal void OnGUI()
        {
            GUILayout.BeginVertical(GUILayout.Height(pageHeight));

            if (uiVisible)
            {
                if (GUILayout.Button("Close Settings"))
                {
                    CloseSettings();
                }

                GUILayout.FlexibleSpace();

                GUILayout.Label("Settings:");
                settings_mode = GUILayout.Toggle(settings_mode, "Notification", GUILayout.ExpandWidth(false));

                if (settings_mode)
                {
                    notificationToggle.notificationStates = new Dictionary<NotificationType, bool>()
{
                        { NotificationType.SolarPanelsIneffectiveMessage, AssistantToTheAssistantPatchManager.isSolarPanelsEnabled },
                        { NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, AssistantToTheAssistantPatchManager.isOutOfFuelEnabled },
                        { NotificationType.GamePauseToggledMessage, AssistantToTheAssistantPatchManager.isPauseVisible },
                        { NotificationType.VesselOutOfElectricity, AssistantToTheAssistantPatchManager.isElectricalEnabled },
                        { NotificationType.VesselLostControlMessage, AssistantToTheAssistantPatchManager.isLostControlEnabled },
                        { NotificationType.VesselLeftCommunicationRangeMessage, AssistantToTheAssistantPatchManager.isCommRangeEnabled },
                        { NotificationType.CannotChangeNodeWhileOutOfFuelMessage, AssistantToTheAssistantPatchManager.isOutOfFuelEnabled }
                        };
                    notificationToggle.ListGUI();
                }
            }
            GUILayout.EndVertical();
            GUILayout.Space(5);
        }
    }
}
