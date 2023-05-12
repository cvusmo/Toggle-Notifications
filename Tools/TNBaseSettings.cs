

namespace ToggleNotifications.TNTools
{
    public class TNBaseSettings : TNSettings
    {
        public static SettingsFile sfile = null;
        public static string SettingsPath;
        public static void Init(string settingsPath)
        {
            sfile = new SettingsFile(settingsPath);
        }
        public static int WindowXPos
        {
            get => sfile.GetInt("WindowXPos", 70);
            set { sfile.SetInt("WindowXPos", value); }
        }

        public static int WindowYPos
        {
            get => sfile.GetInt("WindowYPos", 50);
            set { sfile.SetInt("WindowYPos", value); }
        }

        public static int MainTabIndex
        {
            get { return sfile.GetInt("MainTabIndex", 0); }
            set { sfile.SetInt("MainTabIndex", value); }
        }
    }
}
