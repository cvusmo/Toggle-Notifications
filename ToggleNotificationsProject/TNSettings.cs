using BepInEx.Configuration;

namespace ToggleNotifications
{
    public class TNSettings
    {
        private static ConfigFile s_config_file;
        public static void Init(string configurationpath)
        {
            s_config_file = new ConfigFile(configurationpath, true);
        }

        public static T GetValue<T>(string key, T defaultValue)
        {
            return s_config_file.Bind("", key, defaultValue).Value;
        }

        public static void SetValue<T>(string key, T value)
        {
            s_config_file.Bind("", key, value).Value = value;
            s_config_file.Save();
        }

        public static bool GetNotificationState(NotificationType notificationType)
        {
            return GetValue(notificationType.ToString(), true);
        }

        public static void SetNotificationState(NotificationType notificationType, bool value)
        {
            SetValue(notificationType.ToString(), value);
        }
    }
}
