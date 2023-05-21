using KSP.Game;
using KSP.Messages;
using TNUtilities;
using ToggleNotifications.TNTools.UI;
using UnityEngine;
using static ToggleNotifications.TNTools.UI.NotificationToggle;

namespace ToggleNotifications
{
    internal class ToggleNotificationsUI
    {
        internal List<string> NotificationList = new List<string>();
        internal static ToggleNotificationsUI instance;
        internal static NotificationToggle toggleNotification;
        internal ToggleNotificationsPlugin mainPlugin;
        internal MessageCenterMessage Refreshing;
        internal NotificationEvents RefreshingNotification;
        private bool InitDone;
        private TabsUI tabs;
        internal bool RefreshNotification { get; set; }
        internal bool GamePausedGUI { get; set; }
        internal static ToggleNotificationsUI Instance => ToggleNotificationsUI.instance;

        internal ToggleNotificationsUI(ToggleNotificationsPlugin plugin, bool isGUIVisible)
        {
            instance = this;
            mainPlugin = plugin;
            tabs = new TabsUI();
            tabs.Init();
            mainPlugin.isGUIVisible = isGUIVisible;
        }

        internal void Update()
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
        internal static void DrawSoloToggle(string toggleStr, ref bool toggle)
        {
            GUILayout.Space((float)TNStyles.SpacingAfterSection);
            GUILayout.BeginHorizontal();
            toggle = GUILayout.Toggle(toggle, toggleStr, TNBaseStyle.Toggle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space((float)-TNStyles.SpacingAfterSection);
        }
        internal static bool DrawSoloToggle(string toggleStr, bool toggle, bool error = false)
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
        internal static void DrawEntry(string entryName, string value = "", string unit = "")
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
        internal static void DrawEntryButton(
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
        internal static void DrawEntry2Button(
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
        internal static void DrawToggleButtonWithLabel(
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
        internal static void DrawToggleButton(string txt, NotificationType notificationType, int widthOverride = 0)
        {
            bool isOn = toggleNotification.GetNotificationState(notificationType);
            bool flag = UITools.SmallToggleButton(isOn, txt, txt, widthOverride);
            if (flag == isOn)
                return;
            toggleNotification.CheckCurrentState(notificationType, flag);
        }

        internal static bool DrawToggleButton(string toggleStr, ref bool toggle)
        {
            GUILayout.Space(TNStyles.SpacingAfterSection);
            toggle = GUILayout.Toggle(toggle, toggleStr, TNBaseStyle.Toggle);
            GUILayout.FlexibleSpace();
            GUILayout.Space(-TNStyles.SpacingAfterSection);
            return toggle;
        }

        private int selectedRadioButton = 1;
        private int selectedRadioButton2 = 1;
        private int selectedRadioButton3 = 1;
        private int selectedRadioButton4 = 1;

        private void RadioButtonToggle(int toggleValue)
        {
            selectedRadioButton = toggleValue;

            if (selectedRadioButton == 1)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, true);
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.PauseStateChangedMessageToggle, true);
                ///PauseToggleConfig.Value = true;
            }
            else if (selectedRadioButton == 0)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.GamePauseToggledMessage, false);
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.PauseStateChangedMessageToggle, false);
            }
        }
        private void RadioButtonToggle2(int toggleValue)
        {
            selectedRadioButton2 = toggleValue;

            if (selectedRadioButton2 == 1)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, true);
                //SolarToggleConfig.Value = true;
            }
            else if (selectedRadioButton2 == 0)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
                //SolarToggleConfig.Value = false;
            }
        }

        private void RadioButtonToggle3(int toggleValue)
        {
            selectedRadioButton3 = toggleValue;

            if (selectedRadioButton3 == 1)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.VesselThrottleLockedDueToTimewarpingMessage, true);
            }
            else if (selectedRadioButton3 == 0)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.VesselThrottleLockedDueToTimewarpingMessage, false);
            }
        }

        private void RadioButtonToggle4(int toggleValue)
        {
            selectedRadioButton4 = toggleValue;

            if (selectedRadioButton4 == 1)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, true);
                //SolarToggleConfig.Value = true;
            }
            else if (selectedRadioButton4 == 0)
            {
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, false);
                //SolarToggleConfig.Value = false;
            }
        }


        internal void FillWindow(int windowID)
        {
            // Initialize the position of the buttons
            TopButtons.Init(mainPlugin.windowRect.width);

            GUILayout.BeginHorizontal();

            // MENU BAR
            GUILayout.FlexibleSpace();

            GUI.Label(new Rect(10f, 4f, 29f, 29f), (Texture)TNBaseStyle.Icon, TNBaseStyle.IconsLabel);
            Rect closeButtonPosition = new Rect(mainPlugin.windowRect.width - 10, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);

            if (TopButtons.Button(TNBaseStyle.Cross))
                mainPlugin.CloseWindow();

            GUILayout.Space(10);

            if (TopButtons.Button(TNBaseStyle.Gear))
            {
                // Handle the gear button action here if needed
            }

            GUILayout.EndHorizontal();

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            // Notification Toggle Buttons
            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            // Group 2: Toggle Buttons
            GUILayout.BeginVertical(GUILayout.Height(60));

            int buttonWidth = (int)(mainPlugin.windowRect.width - 12); // Subtract 3 on each side for padding

            bool radioButton1 = GUI.Toggle(new Rect(3, 56, buttonWidth, 20), selectedRadioButton == 1, "Game Pause", selectedRadioButton == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedRadioButton == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedRadioButton == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton1)
            {
                selectedRadioButton = 1;
            }
            else
            {
                selectedRadioButton = 0;
            }

            bool radioButton2 = GUI.Toggle(new Rect(3, 96, buttonWidth, 20), selectedRadioButton2 == 1, "Solar Panel Ineffective", selectedRadioButton2 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedRadioButton2 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedRadioButton2 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton2)
            {
                selectedRadioButton2 = 1;
            }
            else
            {
                selectedRadioButton2 = 0;
            }

            bool radioButton3 = GUI.Toggle(new Rect(3, 133, buttonWidth, 20), selectedRadioButton3 == 1, "Active", selectedRadioButton3 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedRadioButton3 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedRadioButton3 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton3)
            {
                selectedRadioButton3 = 1;
            }
            else
            {
                selectedRadioButton3 = 0;
            }

            bool radioButton4 = GUI.Toggle(new Rect(3, 173, buttonWidth, 20), selectedRadioButton4 == 1, "Active", selectedRadioButton4 == 0 ? TNBaseStyle.ToggleError : TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.normal.textColor = selectedRadioButton4 == 1 ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.ToggleError.normal.textColor = selectedRadioButton4 == 0 ? Color.red : ColorTools.ParseColor("#C0E2DC");
            if (radioButton4)
            {
                selectedRadioButton4 = 1;
            }
            else
            {
                selectedRadioButton4 = 0;
            }

            GUILayout.EndVertical();

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            // Group 3: Version Number
            GUIStyle nameLabelStyle = new GUIStyle()
            {
                border = new RectOffset(3, 3, 5, 5),
                padding = new RectOffset(3, 3, 4, 4),
                overflow = new RectOffset(0, 0, 0, 0),
                normal = { textColor = ColorTools.ParseColor("#C0C1E2") },
                alignment = TextAnchor.MiddleRight
            };

            GUILayout.FlexibleSpace();

            GUILayout.Label("v0.2.2", nameLabelStyle);

            GUILayout.Box(GUIContent.none, TNBaseStyle.Separator);

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0.0f, 0.0f, 10000f, 500f));

            mainPlugin.saverectpos();
        }
    }
}