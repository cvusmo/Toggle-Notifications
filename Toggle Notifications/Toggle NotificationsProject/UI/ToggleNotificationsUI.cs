using BepInEx.Logging;
using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using UnityEngine;
using static ToggleNotifications.UI.Selection;

namespace ToggleNotifications
{
    public class ToggleNotificationsUI
    {
        private SolarPanelSelection solarSelection;
        private CommRangeSelection commRangeSelection;
        private ThrottleLockedSelection throttleLockedSelection;
        private GamePauseToggledSelection gamePauseToggledSelection;
        private AllNotificationsSelection allNotificationsSelection;

        private static ManualLogSource Logger;
        public static NotificationToggle currentState;

        private static ToggleNotificationsUI instance;

        public ToggleNotificationsPlugin Plugin { get; }
        public class SetSelection
        {
            public SolarPanelSelection solarPanelSelection { get; }
            public CommRangeSelection commRangeSelection { get; }
            public ThrottleLockedSelection throttleLockedSelection { get; }
            public GamePauseToggledSelection gamePauseToggledSelection { get; }
            public AllNotificationsSelection allNotificationsSelection { get; }

            public SetSelection(ToggleNotificationsUI mainPlugin)
            {
                solarPanelSelection = (SolarPanelSelection)Activator.CreateInstance(typeof(SolarPanelSelection), mainPlugin);
                commRangeSelection = (CommRangeSelection)Activator.CreateInstance(typeof(CommRangeSelection), mainPlugin);
                throttleLockedSelection = (ThrottleLockedSelection)Activator.CreateInstance(typeof(ThrottleLockedSelection), mainPlugin);
                gamePauseToggledSelection = new GamePauseToggledSelection();
                allNotificationsSelection = new AllNotificationsSelection();
            }
        }
        public static ToggleNotificationsUI Instance { get { return instance; } }

        public ToggleNotificationsUI(ToggleNotificationsPlugin mainPlugin, SetSelection selection, SetOption option)
        {
            instance = this;
            this.Plugin = mainPlugin;
            this.SetSelection = selection;
            this.SetOption = option;
        }

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
            if (currentState == null)
                currentState = null;
            var state = ToggleNotificationsPlugin.Instance.currentState;
            if (init_done)
            {
                RefreshState();
                tabs.Update();
            }
        }

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
        public enum CommRangeType
        {
            Enabled,
            Disabled,
        }
        public void CommRangeTypeSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawToggleButton("Enable", CommRangeType.Enabled);
            DrawToggleButton("Disable", CommRangeType.Disabled);

            GUILayout.EndHorizontal();
        }
        public void DrawCommRangeButton(string txt, CommRangeType this_comrange_type, bool shortDingus = false)
        {
            bool active = commRangeType == this_comrange_type;
            bool result = UITools.SmallToggleButton(active, txt, txt, shortDingus);
            if (result != active) { SetCommRangeType(result ? this_comrange_type : CommRangeType.Enabled); }
        }

        public enum ThrottleLockedType
        {
            Enabled,
            Disabled,
        }
        public void ThrottleLockedWarpTypeSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawThrottleLockedButton("Enable", ThrottleLockedType.Enabled);
            DrawThrottleLockedButton("Disable", ThrottleLockedType.Disabled);

            GUILayout.EndHorizontal();
        }
        public void DrawThrottleLockedButton(string txt, ThrottleLockedType this_throttlelocked_type, bool shortDingus = false)
        {
            bool active = throttlelocked_type == this_throttlelocked_type;
            bool result = UITools.SmallToggleButton(active, txt, txt, shortDingus);
            if (result != active) { SetThrottleLockedType(result ? this_throttlelocked_type : ThrottleLockedType.None); }
        }

        public enum MNType
        {
            Enabled,
            Disabled,
        }
        public void ManeuverNodeOutOfFuelSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawMNOutOfFuelButton("Enable", MNType.Enabled);
            DrawMNOutOfFuelButton("Disable", MNType.Disabled);

            GUILayout.EndHorizontal();
        }

        public void DrawMNOutOfFuelButton(string txt, MNType type)
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

        public enum GPausedType
        {
            Enabled,
            Disabled,
        }
        public void GamePauseToggledSelectionGUI()
        {
            GUILayout.BeginHorizontal();

            DrawGPausedButton("Enabled", GPausedType.Enabled);
            DrawGPausedButton("Disabled", GPausedType.Disabled);

            GUILayout.EndHorizontal();
        }

        public void DrawGPausedButton(string txt, GPausedType type)
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
        public void DrawAllNotificationsButton(string txt, AllNotificationsType type)
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

        private var isRefreshing;
        public async void OnGUI()
        {
            createTabs();
            tabs.onGUI();

            if (currentState.listGui())
                return;

            if (burn_options.listGui())
                return;

            foreach (SetSelection selection in selectionList)
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