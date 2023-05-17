using BepInEx.Logging;
using KSP.Game;
using KSP.Messages;
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
        private static readonly ManualLogSource Logger;
        public static NotificationToggle toggleState;
        private bool InitDone;
        private bool isGUIVisible = false;
        TabsUI tabs = new TabsUI();
        public bool RefreshNotification { get; set; }
        public bool GamePausedGUI { get; set; }
        public static ToggleNotificationsUI Instance => ToggleNotificationsUI.instance;

        public ToggleNotificationsUI(ToggleNotificationsPlugin plugin)
        {
            ToggleNotificationsUI.instance = this;
            plugin = mainPlugin;
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

            tabs.Update();
        }

        public bool RefreshState
        {
            get
            {
                RefreshNotifications(); // Call RefreshNotifications to update the notification states
                string[] notificationsToCheck = { "SolarPanelsIneffectiveMessage", "VesselLeftCommunicationRangeMessage", "VesselThrottleLockedDueToTimewarpingMessage", "CannotPlaceManeuverNodeWhileOutOfFuelMessage", "GamePauseToggledMessage" };

                foreach (string notificationName in notificationsToCheck)
                {
                    if (Enum.TryParse(notificationName, out NotificationType notificationType) && toggleState.GetNotificationState(notificationType))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        public bool RefreshNotifications()
        {
            if (RefreshNotification)
            {
                NotificationList.Clear(); // Clear the existing list
                foreach (NotificationType notificationType in System.Enum.GetValues(typeof(NotificationType)))
                {
                    if (notificationType != NotificationType.None)
                    {
                        NotificationList.Add(notificationType.ToString());
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        public void CheckCurrentState()
        {
            // Check the refreshing state of the UI
            bool isRefreshing = Refreshing != null;

            // Check the refreshing state of notifications
            bool isRefreshingNotification = RefreshingNotification != null;

            // Use the values as needed
            Debug.Log($"UI Refreshing: {isRefreshing}");
            Debug.Log($"Notification Refreshing: {isRefreshingNotification}");
        }
        public void ShowGUI()
        {
            isGUIVisible = true;

            GUILayout.BeginVertical();
            GUILayout.Label("Toggle Notifications");

            bool gamePauseToggledMessage = toggleNotification.GetNotificationState(NotificationType.GamePauseToggledMessage);
            gamePauseToggledMessage = GUILayout.Toggle(gamePauseToggledMessage, "Enable Game Pause Notification");
            toggleNotification.CheckCurrentState(NotificationType.GamePauseToggledMessage, gamePauseToggledMessage);

            bool solarPanelsIneffectiveMessage = toggleNotification.GetNotificationState(NotificationType.SolarPanelsIneffectiveMessage);
            solarPanelsIneffectiveMessage = GUILayout.Toggle(solarPanelsIneffectiveMessage, "Enable Solar Panels Ineffective");
            toggleNotification.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, solarPanelsIneffectiveMessage);

            // Add other GUI elements specific to your mod here

            GUILayout.EndVertical();
        }
        public void HideGUI()
        {
            isGUIVisible = false;
        }
        public MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle toggleState)
        {
            // Implement the logic to convert the current state to a MessageCenterMessage
            if (toggleState.GetNotificationState(NotificationType.SolarPanelsIneffectiveMessage))
            {
                SolarPanelsIneffectiveMessage message = new SolarPanelsIneffectiveMessage();
                message.SentOn = toggleState.SentOn;

                // Set any other properties of the message as needed

                return null;
            }

            // If the notification name doesn't match any specific type, return null or a default message
            return null;
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
        public static double DrawEntryTextField(
            string entryName,
            double value,
            string unit = "",
            GUIStyle thisStyle = null)
        {
            if (!UIFields.InputFields.Contains(entryName))
                UIFields.InputFields.Add(entryName);
            GUILayout.BeginHorizontal();
            if (thisStyle != null)
                UITools.Label(entryName, TNBaseStyle.Label);
            else
                UITools.Label(entryName);
            GUILayout.FlexibleSpace();
            GUI.SetNextControlName(entryName);
            value = UIFields.DoubleField(entryName, value, thisStyle ?? TNBaseStyle.TextInputStyle);
            GUILayout.Space(3f);
            if (thisStyle != null)
                UITools.Label(unit);
            else
                UITools.Label(entryName);
            GUILayout.EndHorizontal();
            GUILayout.Space((float)TNStyles.SpacingAfterTallEntry);
            return value;
        }
        public static double DrawLabelWithTextField(string entryName, double value, string unit = "")
        {
            GUILayout.BeginHorizontal();
            UITools.Label(entryName);
            GUILayout.Space(10f);
            value = UIFields.DoubleField(entryName, value);
            GUILayout.Space(3f);
            UITools.Label(unit);
            GUILayout.EndHorizontal();
            GUILayout.Space((float)TNStyles.SpacingAfterEntry);
            return value;
        }
        public static double DrawToggleButtonWithTextField(
            string runString,
            NotificationType type,
            double value,
            string unit = "",
            bool parseAsTime = false)
        {
            GUILayout.BeginHorizontal();
            ToggleNotificationsUI.DrawToggleButton(runString, type);
            GUILayout.Space(10f);
            //value = UIFields.DoubleField(runString, value, parseAsTime: parseAsTime);
            GUILayout.Space(3f);
            UITools.Label(unit, TNBaseStyle.UnitLabelStyle);
            GUILayout.EndHorizontal();
            GUILayout.Space((float)TNStyles.SpacingAfterEntry);
            return value;
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
        private void CreateTabs()
        {
            //tabs.Pages.Add(new ToggleNotificationsPage { PageIndex = 0 });
            //tabs.Pages.Add(new ToggleNotificationsPage { PageIndex = 1 });
        }
        public void OnGUI()
        {
            // Refresh notifications and states
            RefreshNotifications();
            bool refreshState = RefreshState;
            GUILayout.Label("Game Pause Toggled:");
            GamePausedGUI = GUILayout.Toggle(GamePausedGUI, "Enable Game Pause");
        }
    }
}