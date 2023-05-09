using ToggleNotifications.TNTools.UI;
using UnityEngine;
using static ToggleNotifications.ToggleNotificationsUI;

namespace ToggleNotifications.UI
{
    public class Selection : BasePageContent
    {
        readonly ToggleNotificationsPlugin Plugin;
        readonly ToggleNotificationsUI mainPlugin;

        private TabsUI tabs = new TabsUI();
        private Dictionary<NotificationType, (SetSelection selection, SetOption option)> selectionMap;
        public Selection(ToggleNotificationsUI mainPlugin, Type[] selectionTypes, Type[] optionTypes)
        {
            selectionMap = new Dictionary<NotificationType, (SetSelection selection, SetOption option)>();
            this.mainPlugin = mainPlugin;
            this.tabs = new TabsUI();
            //this.selectionTypes = selectionTypes;
            //this.optionTypes = optionTypes;

            for (int type = 0; type < selectionTypes.Length; type++)
            {
                var selection = (SetSelection)Activator.CreateInstance(selectionTypes[type], mainPlugin);
                var option = (SetOption)Activator.CreateInstance(optionTypes[type]);

                selectionMap.Add((NotificationType)type, (selection, option));

                tabs.AddTab(selectionTypes[type].Name);
                tabs.AddContent(selectionTypes[type].Name, selection.GetContent());
            }
        }
        public class GetOptionType
        {
            public NotificationType type;
            public bool isEnabled = true;
            public string description = "Description String selection.cs";
            public Dictionary<NotificationType, SetOption> OptionType = new Dictionary<NotificationType, SetOption>();

            private Dictionary<NotificationType, (SetSelection selection, SetOption option)> selectionMap;

            public GetOptionType(Dictionary<NotificationType, (SetSelection selection, SetOption option)> selectionMap)
            {
                this.selectionMap = selectionMap;
            }

            public void SetType(NotificationType type, bool isEnabled)
            {
                this.type = type;
                this.isEnabled = isEnabled;
                if (selectionMap.ContainsKey(type))
                {
                    description = selectionMap[type].option.setOptionsList(isEnabled ? NotificationStatus.Enabled.ToString() : NotificationStatus.Disabled.ToString());
                }
            }
        }
        public enum NotificationType
        {
            SolarType,
            CommRange,
            ThrottleLocked,
            ManeuverNodeOutOfFuel,
            GamePauseToggled,
            AllNotifications
        }

        public enum NotificationStatus
        {
            Enabled,
            Disabled,
        }

        public interface SetSelection
        {
            string GetOptionType();
            void SelectionGUI();
            bool listGUI();
            enum NotificationType();
        }

        public class SolarPanelSelection : SetSelection
        {
            private ToggleNotificationsUI mainPlugin;
            private SolarOption solarOption = new SolarOption();
            private NotificationType type = new SolarType();

            public SolarPanelSelection(ToggleNotificationsUI mainPlugin)
            {
                this.mainPlugin = mainPlugin;
                NotificationType.SolarType = solarType;
            }
            public void SetOption(NotificationToggle SolarPanelsIneffectiveMessageToggle)
            {
                public string setOptionsList(string value)
                {
                    // Implement the setOptionsList method here
                    return "";
                }
            }

            public void GetOptionType(NotificationType type)
            {
                solarType = type;
                mainPlugin.GetOptionType(type);
            }
            
            public string GetContent()
            {
                // Return the content for the Solar selection here
                return "Solar selection content";
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
                bool active = solarType == type;

                bool result = UITools.SmallToggleButton(active, txt, txt);
                if (result != active)
                {
                    if (!active)
                        solarType = type;
                    else
                        solarType = SolarType.None;

                    // Enable or disable the corresponding notification in the mainPlugin
                    switch (solarType)
                    {
                        case SolarType.Enabled:
                            ToggleNotificationsPlugin.Instance.EnableNotification("SolarPanelsIneffectiveMessage");
                            mainPlugin.selection.SetSolarType(SolarType.Enabled);
                            break;
                        case SolarType.Disabled:
                            ToggleNotificationsPlugin.Instance.DisableNotification("SolarPanelsIneffectiveMessage");
                            mainPlugin.selection.SetSolarType(SolarType.Disabled);
                            break;
                    }
                }
            }
            public void SelectionGUI()
            {
                SolarTypeSelectionGUI();
            }
            public void DrawContents()
            {
                base.DrawContents();
                var selection = selectionMap[NotificationType.Solar].selection;
                selection.SolarTypeSelectionGUI();
            }

            public bool listGUI()
            {
                // Implement the Solar selection list GUI here
                return false;
            }
        }
        public class CommRangeSelection : SetSelection
        {
            private ToggleNotificationsUI mainPlugin;
            private CommRangeOption commRangeOption = new CommRangeOption();
            private CommRangeType commRangeType = new CommRangeType();

            public CommRangeSelection(ToggleNotificationsUI mainPlugin)
            {
                this.mainPlugin = mainPlugin;
            }

            public void SetCommRangeType(CommRangeType type)
            {
                commRangeType = type;
                mainPlugin.SetCommRangeType(type);
            }

            public string GetContent()
            {
                // Return the content for the CommRange selection here
                return "CommRange selection content";
            }

            public void CommRangeTypeSelectionGUI()
            {
                mainPlugin.CommRangeTypeSelectionGUI();
            }

            public void SelectionGUI()
            {
                SetCommRangeType(commRangeType.Enabled); //enables
                mainPlugin.CommRangeTypeSelectionGUI();
                SetCommRangeType(commRangeType.Disabled);
                mainPlugin.CommRangeTypeSelectionGUI();
            }

            public bool listGUI()
            {
                // Implement the CommRange selection list GUI here
                return false;
            }
        }
        public class ThrottleLockedSelection : SetSelection
        {
            private ToggleNotificationsUI mainPlugin;
            private ThrottleLockedOption ThrottleLockedOption = new ThrottleLockedOption();
            private ThrottleLockedType ThrottleLockedType = new ThrottleLockedType();

            public ThrottleLockedSelection(ToggleNotificationsUI mainPlugin)
            {
                this.mainPlugin = mainPlugin;
            }

            public void SetThrottleLockedType(ThrottleLockedType type)
            {
                ThrottleLockedType = type;
                mainPlugin.SetThrottleLockedType(type);
            }

            public string GetContent()
            {
                // Return the content for the ThrottleLocked selection here
                return "ThrottleLocked selection content";
            }

            public void ThrottleLockedTypeSelectionGUI()
            {
                mainPlugin.ThrottleLockedTypeSelectionGUI();
            }

            public void SelectionGUI()
            {
                SetThrottleLockedType(ThrottleLockedType.Enabled); //enables
                mainPlugin.ThrottleLockedTypeSelectionGUI();
                SetThrottleLockedType(ThrottleLockedType.Disabled);
                mainPlugin.ThrottleLockedTypeSelectionGUI();
            }

            public bool listGUI()
            {
                // Implement the ThrottleLocked selection list GUI here
                return false;
            }
        }
        public class ManeuverNodeOutOfFuelSelection : SetSelection
        {
            public string GetContent()
            {
                // Return the content for the ManeuverNodeOutOfFuel selection here
                return "ManeuverNodeOutOfFuel selection content";
            }

            public void SelectionGUI()
            {
                // Implement the ManeuverNodeOutOfFuel selection GUI here
            }

            public bool listGUI()
            {
                // Implement the ManeuverNodeOutOfFuel selection list GUI here
                return false;
            }
        }
        public class GamePauseToggledSelection : SetSelection
        {
            public string GetContent()
            {
                // Return the content for the GamePauseToggled selection here
                return "GamePauseToggled selection content";
            }

            public void SelectionGUI()
            {
                // Implement the GamePauseToggled selection GUI here
            }

            public bool listGUI()
            {
                // Implement the GamePauseToggled selection list GUI here
                return false;
            }
        }
        public class AllNotificationsSelection : SetSelection
        {
            public string GetContent()
            {
                // Return the content for the AllNotifications selection here
                return "AllNotifications content";
            }

            public void SelectionGUI()
            {
                // Implement the AllNotifications selection GUI here
            }

            public bool listGUI()
            {
                // Implement the AllNotifications selection list GUI here
                return false;
            }
        }
        public interface SetOption
        {
            string setOptionsList(string value);
        }
        public class CommRangeOption : SetOption
        {
            public string setOptionsList(string value)
            {
                // Implement the setOptionsList method here
                return "";
            }
        }
        public class ThrottleLockedOption : SetOption
        {
            public string setOptionsList(string value)
            {
                // Implement the setOptionsList method here
                return "";
            }
        }
        public class ManeuverNodeOutOfFuelOption : SetOption
        {
            public string setOptionsList(string value)
            {
                // Implement the setOptionsList method here
                return "";
            }
        }
        public class GamePauseToggledOption : SetOption
        {
            public string setOptionsList(string value)
            {
                // Implement the setOptionsList method here
                return "";
            }
        }
        public class AllNotificationsOption : SetOption
        {
            public string setOptionsList(string value)
            {
                // Implement the setOptionsList method here
                return "";
            }
        }
        void SelectionGUI()
        {
            foreach (var item in selectionMap)
            {
                var selection = item.Value.selection;
                selection.SetSolarType(SolarType.Enabled); // Enabled
                selection.SelectionGUI();
                selection.SetSolarType(SolarType.Disabled); // Disabled
                selection.SelectionGUI();
                selection.SetCommRangeType(CommRangeType.Enabled); // Enabled
                selection.SelectionGUI();
                selection.SetCommRangeType(CommRangeType.Disabled); // Disabled
                selection.SelectionGUI();
                selection.SetThrottleLockedType(ThrottleLockedType.Enabled); // Enabled
                selection.SelectionGUI();
                selection.SetThrottleLockedType(ThrottleLockedType.Disabled); // Disabled
                selection.SelectionGUI();
                selection.SetGamePauseToggledSelection(GamePauseToggledSelection.Enabled); // Enabled
                selection.SelectionGUI();
                selection.SetGamePauseToggledSelection(GamePauseToggledSelection.Disabled); // Disabled
                selection.SelectionGUI();
                selection.SetAllNotificationsSelection(AllNotificationsSelection.Enabled); // Enabled
                selection.SelectionGUI();
                selection.SetAllNotificationsSelection(AllNotificationsSelection.Disabled); // Disabled

            }
        }
    }
}