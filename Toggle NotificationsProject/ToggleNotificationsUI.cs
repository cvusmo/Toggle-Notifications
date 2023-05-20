using KSP.Game;
using KSP.Messages;
using TNUtilities;
using ToggleNotifications.TNTools.UI;
using UnityEngine;
using static ToggleNotifications.TNTools.UI.NotificationToggle;

namespace ToggleNotifications
{
    public class ToggleNotificationsUI
    {
        public List<string> NotificationList = new List<string>();
        public static ToggleNotificationsUI instance;
        public static NotificationToggle toggleNotification;
        public ToggleNotificationsPlugin mainPlugin;
        public MessageCenterMessage Refreshing;
        public NotificationEvents RefreshingNotification;
        public static NotificationToggle toggleState;
        private bool InitDone;
        private bool isGUIVisible;
        private TabsUI tabs;
        public bool RefreshNotification { get; set; }
        public bool GamePausedGUI { get; set; }
        public static ToggleNotificationsUI Instance => ToggleNotificationsUI.instance;
        public ToggleNotificationsUI(ToggleNotificationsPlugin plugin, bool isGUIVisible)
        {
            instance = this;
            mainPlugin = plugin;
            tabs = new TabsUI();
            tabs.Init();
            this.isGUIVisible = isGUIVisible;
        }
        public void Update()
        {
            if (!InitDone)
            {
                this.Refreshing = null;
                this.RefreshingNotification = null;
                tabs.Init();
                InitDone = true;
            }

            if (RefreshNotification)
            {
                TNUtility.Instance.RefreshNotifications();
            }

            tabs.Update();
        }
        public static void DrawSoloToggle(string toggleStr, ref bool toggle)
        {
            GUILayout.Space((float)TNStyles.SpacingAfterSection);
            GUILayout.BeginHorizontal();
            toggle = GUILayout.Toggle(toggle, toggleStr, TNBaseStyle.Toggle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)-TNStyles.SpacingAfterSection);
        }
        public static bool DrawSoloToggle(string toggleStr, bool toggle, bool error = false)
        {
            GUILayout.Space((float)TNStyles.SpacingAfterSection);
            GUILayout.BeginHorizontal();
            if (error)
            {
                GUILayout.Toggle(toggle, toggleStr, TNBaseStyle.ToggleError);
                toggle = false;
            }
            else
                toggle = GUILayout.Toggle(toggle, toggleStr, TNBaseStyle.Toggle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)-TNStyles.SpacingAfterSection);
            return toggle;
        }
        public static void DrawEntry(string entryName, string value = "", string unit = "")
        {
            GUILayout.BeginHorizontal();
            UITools.Label(entryName);
            if (value.Length > 0)
            {
                GUILayout.FlexibleSpace();
                UITools.Label(value);
                if (unit.Length > 0)
                {
                    GUILayout.Space(5f);
                    UITools.Label(unit);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space((float)TNStyles.SpacingAfterEntry);
        }
        public static void DrawEntryButton(
            string entryName,
            ref bool button,
            string buttonStr,
            string value,
            string unit = "")
        {
            GUILayout.BeginHorizontal();
            UITools.Label(entryName);
            GUILayout.FlexibleSpace();
            button = UITools.CtrlButton(buttonStr);
            UITools.Label(value);
            GUILayout.Space(5f);
            UITools.Label(unit);
            GUILayout.EndHorizontal();
            GUILayout.Space((float)TNStyles.SpacingAfterEntry);
        }
        public static void DrawEntry2Button(
            string entryName,
            ref bool button1,
            string button1Str,
            ref bool button2,
            string button2Str,
            string value,
            string unit = "",
            string divider = "")
        {
            GUILayout.BeginHorizontal();
            UITools.Console(entryName);
            GUILayout.FlexibleSpace();
            button1 = UITools.CtrlButton(button1Str);
            if (divider.Length > 0)
                UITools.Console(divider);
            button2 = UITools.CtrlButton(button2Str);
            UITools.Console(value);
            GUILayout.Space(5f);
            UITools.Console(unit);
            GUILayout.EndHorizontal();
            GUILayout.Space((float)TNStyles.SpacingAfterEntry);
        }
        public static void DrawToggleButtonWithLabel(
            string runString,
            NotificationType type,
            string label = "",
            string unit = "",
            int widthOverride = 0)
        {
            GUILayout.BeginHorizontal();
            DrawToggleButton(runString, type, widthOverride);
            GUILayout.Space(10f);
            UITools.Label(label, TNBaseStyle.NameLabelStyle);
            GUILayout.Space(3f);
            UITools.Label(unit, TNBaseStyle.UnitLabelStyle);
            GUILayout.EndHorizontal();
            GUILayout.Space((float)TNStyles.SpacingAfterTallEntry);
        }
        public static void DrawToggleButton(string txt, NotificationType notificationType, int widthOverride = 0)
        {
            bool isOn = toggleNotification.GetNotificationState(notificationType);
            bool flag = UITools.SmallToggleButton(isOn, txt, txt, widthOverride);
            if (flag == isOn)
                return;
            toggleNotification.CheckCurrentState(notificationType, flag);
        }

        public static bool DrawToggleButton(string toggleStr, ref bool toggle)
        {
            GUILayout.Space(TNStyles.SpacingAfterSection);
            toggle = GUILayout.Toggle(toggle, toggleStr, TNBaseStyle.Toggle);
            GUILayout.FlexibleSpace();
            GUILayout.Space(-TNStyles.SpacingAfterSection);
            return toggle;
        }
        public bool OnGUI()
        {

            TNUtility.Instance.RefreshNotifications();
            TNUtility.Instance.RefreshCurrentState();
            return true;
        }
    }
}