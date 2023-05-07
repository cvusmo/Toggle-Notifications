using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;

namespace ToggleNotifications.UI
{
    public class Selection
    {
        private ToggleNotificationsUI ui;
        private TabsUI tabs = new TabsUI();
        private Dictionary<NotificationType, (ISelection selection, IOption option)> selectionMap;

        public Selection(ToggleNotificationsUI ui, Type[] selectionTypes, Type[] optionTypes, ToggleNotificationsPlugin plugin)
        {
            this.ui = ui;
            selectionMap = new Dictionary<NotificationType, (ISelection selection, IOption option)>();

            for (int i = 0; i < selectionTypes.Length; i++)
            {
                var selection = (ISelection)Activator.CreateInstance(selectionTypes[i], plugin);
                var option = (IOption)Activator.CreateInstance(optionTypes[i]);

                selectionMap.Add((NotificationType)i, (selection, option));

                var tab = new TabUI(selectionTypes[i].Name);
                tabs.AddTab(tab);
                tab.AddContent(selection.GetContent());
            }
        }

        public enum NotificationType
        {
            Solar,
            CommRange,
            ThrottleLocked,
            ManeuverNodeOutOfFuel,
            GamePauseToggled,
            AllNotifications
        }

        public class GetType
        {
            public NotificationType type;
            public bool isEnabled = true;
            public string description = "";

            public void SetType(NotificationType type, bool isEnabled)
            {
                this.type = type;
                this.isEnabled = isEnabled;
                if (selectionMap.ContainsKey(type))
                {
                    description = selectionMap[type].option.setOptionsList(isEnabled ? OptionType.Enable : OptionType.Disable);
                }
            }
        }
    }