using BepInEx.Configuration;
using ToggleNotifications.TNTools;

namespace ToggleNotifications
{
    internal class TNSettings
    {
        private static ConfigFile s_config_file;
        internal static void Init(string configurationpath)
        {
            s_config_file = new ConfigFile(configurationpath, true);
        }

        internal static T GetValue<T>(string key, T defaultValue)
        {
            return s_config_file.Bind("", key, defaultValue).Value;
        }

        internal static void SetValue<T>(string key, T value)
        {
            s_config_file.Bind("", key, value).Value = value;
            s_config_file.Save();
        }

        internal static bool GetNotificationState(NotificationType notificationType)
        {
            return GetValue(notificationType.ToString(), true);
        }

        internal static void SetNotificationState(NotificationType notificationType, bool value)
        {
            SetValue(notificationType.ToString(), value);
        }
    }
}
