using KSP.Messages;
using ToggleNotifications.Pages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class ToggleNotificationsUI : MonoBehaviour
    {
        internal ToggleNotificationsPlugin mainPlugin;
        private MessageCenter messageCenter;
        private Dictionary<NotificationType, bool> notificationStates = new Dictionary<NotificationType, bool>();
        private NotificationToggle notificationToggle;

        //pages
        private MainPage mainPage;
        private GearPage gearPage;

        public ToggleNotificationsUI(ToggleNotificationsPlugin mainPlugin, bool _isGUIenabled, MessageCenter messageCenter)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            notificationToggle = new NotificationToggle(mainPlugin, notificationStates);

            gearPage = new GearPage(mainPlugin, notificationToggle);
            mainPage = new MainPage(mainPlugin, messageCenter, notificationToggle);
        }

        internal void OnGUI()
        {
            mainPlugin.windowRect = GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                mainPlugin.windowRect,
                FillWindow,
                "<color=#696DFF>TOGGLE NOTIFICATIONS</color>",
                GUILayout.Height(0.0f),
                GUILayout.Width((float)mainPlugin.windowWidth),
                GUILayout.MinHeight(400)
            );
        }

        internal void FillWindow(int windowID)
        {
            TopButtons.Init(mainPlugin.windowRect.width);
            WindowTool.CheckMainWindowPos(ref mainPlugin.windowRect, mainPlugin.windowWidth);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUI.Label(new Rect(10f, 4f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            Rect closeButtonPosition = new Rect(mainPlugin.windowRect.width - 10, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);

            if (TopButtons.Button(TNBaseStyle.Cross))
                mainPlugin.CloseWindow();

            GUILayout.Space(10);

            if (TopButtons.Button(TNBaseStyle.Gear))
            {
                gearPage.ToggleVisibility();
                Debug.Log("Gear Button Status: " + gearPage.UIVisible);
            }

            GUILayout.EndHorizontal();

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            if (gearPage.UIVisible)
            {
                gearPage.OnGUI();
            }
            else
            {
                mainPage.OnGUI();
            }

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));

            mainPlugin.saverectpos();
        }

        private void DrawGearPage()
        {
            gearPage.OnGUI();
        }
    }
}
