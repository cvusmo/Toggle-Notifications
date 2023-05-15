using BepInEx.Logging;
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
        public static ToggleNotificationsUI Instance { get { return instance; } }
        private static NotificationToggle toggleNotification;
        public ToggleNotificationsPlugin mainPlugin;
        public Selection Selection;
        private SetOption setOption;

        // Setup
        private Selection selection;
        public MessageCenterMessage Refreshing;
        public NotificationEvents RefreshingNotification;
        private static readonly ManualLogSource Logger;
        public static NotificationToggle currentState;
        private static readonly ToggleNotificationsUI instance;
        private static bool InitDone = false;
        private bool isGUIVisible = false;
        public bool GamePausedGUI { get; set; }


        TabsUI tabs = new TabsUI();
        public ToggleNotificationsUI(ToggleNotificationsPlugin plugin)
        {
            mainPlugin = plugin;
            selection = new Selection(mainPlugin);
        }
        public void Awake()
        {
            toggleNotification = ToggleNotificationsPlugin.Instance.notificationToggle; // Assign the toggleNotification instance
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
                if (currentState == null)
                {
                    RefreshNotifications();
                }
                string[] notificationsToCheck = { "SolarPanelsIneffectiveMessage", "VesselLeftCommunicationRangeMessage", "VesselThrottleLockedDueToTimewarpingMessage", "CannotPlaceManeuverNodeWhileOutOfFuelMessage", "GamePauseToggledMessage" };

                foreach (string notificationName in notificationsToCheck)
                {
                    if (Enum.TryParse(notificationName, out NotificationType notificationType) && currentState.GetNotificationState(notificationType))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public object SetOption { get; private set; }
        public bool RefreshNotifications()
        {
            if (ToggleNotificationsPlugin.Instance.notificationToggle != null)
            {
                foreach (var entry in ToggleNotificationsPlugin.Instance.notificationToggle.notificationStates)
                {
                    currentState.SetNotificationState(entry.Key, entry.Value);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        public void ProcessOption(SetOption option)
        {
            switch (option.option)
            {
                case "refreshState":
                    bool isRefreshed = RefreshState;
                    break;
                case "refreshNotifications":
                    bool isRefreshedNotification = RefreshNotifications();
                    break;
                default:
                    // handle unknown option
                    break;
            }
        }
        public void ShowGUI()
        {
            // Set GUI visibility flag to true
            isGUIVisible = true;

            // Define the GUI elements you want to display

            GUILayout.BeginVertical();

            GUILayout.Label("Toggle Notifications");

            bool gamePauseToggledMessage = toggleNotification.GetNotificationState(NotificationType.GamePauseToggledMessage);
            gamePauseToggledMessage = GUILayout.Toggle(gamePauseToggledMessage, "Enable Game Pause Notification");
            toggleNotification.SetNotificationState(NotificationType.GamePauseToggledMessage, gamePauseToggledMessage);

            bool solarPanelsIneffectiveMessage = toggleNotification.GetNotificationState(NotificationType.SolarPanelsIneffectiveMessage);
            solarPanelsIneffectiveMessage = GUILayout.Toggle(solarPanelsIneffectiveMessage, "Enable Solar Panels Ineffective");
            toggleNotification.SetNotificationState(NotificationType.SolarPanelsIneffectiveMessage, solarPanelsIneffectiveMessage);

            GUILayout.EndVertical();
        }
        public void HideGUI()
        {
            isGUIVisible = false;
        }
        public MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle currentState)
        {
            // Implement the logic to convert the current state to a MessageCenterMessage
            if (currentState.GetNotificationState(NotificationType.SolarPanelsIneffectiveMessage))
            {
                SolarPanelsIneffectiveMessage message = new SolarPanelsIneffectiveMessage();
                message.SentOn = currentState.SentOn;

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
            toggleNotification.SetNotificationState(notificationType, flag);
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
            if (InitDone)
                return;
            //tabs.pages.Add(new SolarPage(mainPlugin, notificationToggle));
            //tabs.pages.Add(new CommRangePage(mainPlugin, notificationToggle));
            //tabs.pages.Add(new ThrottlePage(mainPlugin, notificationToggle));
            //tabs.pages.Add(new NodePage(mainPlugin, notificationToggle));
            this.tabs.pages.Add((IPageContent)new GamePausedPage());
            //tabs.pages.Add(new ThrottlePage(mainPlugin, notificationToggle));
        }
        public void OnGUI()
        {
            this.CreateTabs();
            string situation = mainPlugin.CurrentState.ToString();
            string notificationList = string.Join(", ", toggleNotification.NotificationList); // Access the notification list from the toggleNotification instance
            GUILayout.Label($"Situation: {situation} {notificationList}");


            // Handle Selection
            if (this.Selection.ListGUI())
                return;

            // Refresh notifications and states
            TNUtility.RefreshNotifications();
            TNUtility.RefreshStates();

            this.tabs.OnGUI();

            // Process SetOption for Game Pause
            bool isGamePauseToggledSelected = this.Selection.IsGamePauseToggledSelected();
            SetOption setOption = new SetOption("GamePauseToggledMessage", isGamePauseToggledSelected ? "Enabled" : "Disabled");
            setOption.ProcessOption();

            GUILayout.Label("Game Pause Toggled:");
            GamePausedGUI = GUILayout.Toggle(GamePausedGUI, "Enable Game Pause");
        }
    }
}