using BepInEx.Logging;
using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using UnityEngine;

namespace ToggleNotifications
{

    public class ToggleNotificationsUI
    {
        private static ToggleNotificationsUI _instance;
        public static ToggleNotificationsUI Instance { get => _instance; }

        public Selection SelectionInstance { get; set; }

        public ToggleNotificationsUI(ToggleNotificationsPlugin main_plugin)
        {
            _instance = this;

            Type[] selectionTypes = new Type[]
            {
            typeof(SolarSelection),
            typeof(CommRangeSelection),
            typeof(ThrottleLockedSelection),
            typeof(ManeuverNodeOutOfFuelSelection),
            typeof(GamePauseToggledSelection),
            typeof(AllNotificationsSelection)
            };

            Type[] optionTypes = new Type[]
            {
            typeof(SolarOption),
            typeof(CommRangeOption),
            typeof(ThrottleLockedOption),
            typeof(ManeuverNodeOutOfFuelOption),
            typeof(GamePauseToggledOption),
            typeof(AllNotificationsOption)
            };

            SelectionInstance = new Selection(this, selectionTypes, optionTypes, main_plugin);
        }

        private static ManualLogSource Logger;
        public static NotificationToggle currentState;
        public bool RefreshState()
        {
            if (currentState == null)
            {
                RefreshNotifications();
            }
            string[] notificationsToCheck = { "SolarPanelsIneffectiveMessage", "VesselLeftCommunicationRangeMessage", "VesselThrottleLockedDueToTimewarpingMessage", "CannotPlaceManeuverNodeWhileOutOfFuelMessage", "GamePauseToggledMessage" };

            foreach (string notificationName in notificationsToCheck)
            {
                if (currentState.GetNotificationState(notificationName))
                {
                    return true;
                }
            }
            // None of the notifications are active, so return false
            return false;
        }

        public bool RefreshNotifications()
        {
            if (ToggleNotificationsPlugin.Instance._notificationToggle != null)
            {
                currentState = ToggleNotificationsPlugin.Instance._notificationToggle;
                return true;
            }
            else
            {
                // Logger.LogWarning("NotificationToggle is null.");
                return false;
            }
        }

        TabsUI tabs = new TabsUI();
        public void Update()
        {
            if (init_done)
            {
                RefreshState();
                tabs.Update();
            }
        }

        public ManeuverType maneuver_type = ManeuverType.None;

        public static TimeRef time_ref = TimeRef.None;

        public void SetManeuverType(ManeuverType type)
        {
            maneuver_type = type;
            maneuver_type_desc = BurnTimeOption.Instance.setOptionsList(type);
        }

        public string maneuver_type_desc;


        ToggleNotificationsPlugin plugin;

        public ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("ToggleNotifications");

        //BodySelection body_selection;
        //BurnTimeOption burn_options;

        int spacingAfterEntry = 5;

        private void DrawEntryButton(string entryName, ref bool button, string buttonStr, string value, string unit = "")
        {
            GUILayout.BeginHorizontal();
            UITools.Label(entryName);
            GUILayout.FlexibleSpace();
            if (UITools.SmallButton(buttonStr))
            {
                button = !button;
                onClick?.Invoke(button);
            }
            UITools.Label(value);
            GUILayout.Space(5);
            UITools.Label(unit);
            GUILayout.EndHorizontal();
            GUILayout.Space(spacingAfterEntry);
        }

        private void DrawEntry2Button(string entryName, ref bool button1, string button1Str, ref bool button2, string button2Str, string value, string unit = "")
        {
            GUILayout.BeginHorizontal();
            UITools.Console(entryName);
            GUILayout.FlexibleSpace();
            button1 = UITools.SmallButton(button1Str);
            button2 = UITools.SmallButton(button2Str);
            UITools.Console(value);
            GUILayout.Space(5);
            UITools.Console(unit);
            GUILayout.EndHorizontal();
            GUILayout.Space(spacingAfterEntry);
        }

        private double DrawLabelWithTextField(string entryName, double value, string unit = "")
        {
            GUILayout.BeginHorizontal();
            UITools.Label(entryName);
            GUILayout.Space(10);

            value = UI_Fields.DoubleField(entryName, value);

            GUILayout.Space(3);
            UITools.Label(unit);
            GUILayout.EndHorizontal();

            GUILayout.Space(spacingAfterEntry);
            return value;
        }

        //public double DrawToggleButtonWithTextField(string runString, OtherType type, double value, string unit = "", string stopString = "")
        // {
        // GUILayout.BeginHorizontal();
        // if (stopString.Length < 1)
        //   stopString = runString;


        // DrawToggleButton(runString, type);
        // GUILayout.Space(10);

        //  value = UI_Fields.DoubleField(runString, value);

        //   GUILayout.Space(3);
        // UI_Too   ls.Label(unit);
        // GUILayout.EndHorizontal();

        // GUILayout.Space(spacingAfterEntry);
        // return value;
        // }

        public enum SolarType
        {
            Enabled,
            Disabled,
        }
        public void SolarTypeSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawToggleButton("Enabled", SolarType.Enabled);
            DrawToggleButton("Disabled", SolarType.Disabled);

            GUILayout.EndHorizontal();
        }

        public void DrawToggleButton(string txt, SolarType type)
        {
            bool active = solar_type == type;

            bool result = UITools.SmallToggleButton(active, txt, txt);
            if (result != active)
            {
                if (!active)
                    solar_type = type;
                else
                    solar_type = SolarType.None;
            }
        }

        public enum ComRangeType
        {
            Enabled,
            Disabled,
        }
        public void CommRangeTypeSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawToggleButton("Enable", ComRangeType.Enabled);
            DrawToggleButton("Disable", ComRangeType.Disabled);

            GUILayout.EndHorizontal();
        }

        public void DrawToggleButton(string txt, ComRangeType type)
        {
            bool active = comrange_type == type;

            bool result = UITools.SmallToggleButton(active, txt, txt);
            if (result != active)
            {
                if (!active)
                    comrange_type = type;
                else
                    comrange_type = ComRangeType.None;
            }
        }

        public enum ThrottleLockedType
        {
            Enabled,
            Disabled,
        }
        public void ThrottleLockedWarpTypeSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawToggleButton("Enable", ThrottleLockedType.Enabled);
            DrawToggleButton("Disable", ThrottleLockedType.Disabled);

            GUILayout.EndHorizontal();
        }
        public void DrawToggleButton(string txt, ThrottleLockedType type)
        {
            bool active = throttlelocked_type == type;

            bool result = UITools.SmallToggleButton(active, txt, txt);
            if (result != active)
            {
                if (!active)
                    throttlelocked_type = type;
                else
                    throttlelocked_type = ThrottleLockedType.None;
            }
        }

        public enum MNType
        {
            Enabled,
            Disabled,
        }
        public void ManeuverNodeOutOfFuelSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawToggleButton("Enable", MNType.Enabled);
            DrawToggleButton("Disable", MNType.Disabled);

            GUILayout.EndHorizontal();
        }

        public void DrawToggleButton(string txt, MNType type)
        {
            bool active = mn_type == type;

            bool result = UITools.SmallToggleButton(active, txt, txt);
            if (result != active)
            {
                if (!active)
                    mn_type = type;
                else
                    mn_type = MNType.None;
            }
        }

        public enum GPaused
        {
            Enabled,
            Disabled,
        }
        public void GamePauseToggledSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawToggleButton("Enabled", GPausedType.Enabled);
            DrawToggleButton("Disabled", GPausedType.Disabled);

            GUILayout.EndHorizontal();
        }

        public void DrawToggleButton(string txt, GPausedType type)
        {
            bool active = gpaused_type == type;

            bool result = UITools.SmallToggleButton(active, txt, txt);
            if (result != active)
            {
                if (!active)
                    gpaused_type = type;
                else
                    gpaused_type = GPausedType.None;
            }
        }

        public enum AllNotificationsType
        {
            Enabled,
            Disabled,
        }
        public void AllNotificationSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawToggleButton("Enable", AllNotificationsType.Enabled);
            DrawToggleButton("Disable", AllNotificationsType.Disabled);

            GUILayout.EndHorizontal();
        }
        public void DrawToggleButton(string txt, AllNotificationsType type)
        {
            bool active = allnotifications_type == type;

            bool result = UITools.SmallToggleButton(active, txt, txt);
            if (result != active)
            {
                if (!active)
                    allnotifications_type = type;
                else
                    allnotifications_type = AllNotificationsType.None;
            }
        }

        bool init_done = false;

        void createTabs()
        {
            if (!init_done)
            {
                tabs.pages.Add(new SolarPage());
                tabs.pages.Add(new CommRangePage());
                tabs.pages.Add(new ThrottlePage());
                tabs.pages.Add(new NodePage());
                tabs.pages.Add(new AllNotificationsPage());
                tabs.pages.Add(new GamePausedPage());

                tabs.Init();

                init_done = true;
            }
        }

        void createTabs()
        {
            if (!init_done)
            {
                _tabs.pages.Add(new TabPage("Solar Type", SolarTypeSelectionGUI));
                _tabs.pages.Add(new TabPage("Communication Range", CommRangeTypeSelectionGUI));
                _tabs.pages.Add(new TabPage("Throttle Locked Warp", ThrottleLockedWarpTypeSelectionGUI));
                _tabs.pages.Add(new TabPage("Maneuver Node Out of Fuel", ManeuverNodeOutOfFuelSelectionGUI));
                _tabs.pages.Add(new TabPage("Game Pause Toggled", GamePauseToggledSelectionGUI));
                _tabs.pages.Add(new TabPage("All Notifications", AllNotificationSelectionGUI));

                tabs.Init();

                init_done = true;
            }
        }

        var isRefreshing;
        public async void OnGUI()
        {
            createTabs();
            tabs.onGUI();

            foreach (ISelection selection in selectionList)
            {
                if (selection.listGUI())
                {
                    return;
                }

                selection.SelectionGUI();
                tabs.onGUI();
            }

            if (!isRefreshing)
            {
                isRefreshing = true;
                await Task.Run(() => RefreshState());
                isRefreshing = false;
            }

        }
        private void DrawGUIStatus(double UT)
        {
            TNStatus.DrawUI(UT);
        }
    }

}