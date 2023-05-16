namespace ToggleNotifications.TNTools
{
    public class TNBaseSettings
    {
        public static SettingsFile SFile = null;
        public static string SettingsPath;

        public static void Init(string settingsPath)
        {
            SFile = new SettingsFile(settingsPath);
        }
        public static int WindowXPos
        {
            get => SFile.GetInt("WindowXPos", 70);
            set { SFile.SetInt("WindowXPos", value); }
        }

        public static int WindowYPos
        {
            get => SFile.GetInt("WindowYPos", 50);
            set { SFile.SetInt("WindowYPos", value); }
        }

        public static int MainTabIndex
        {
            get { return SFile.GetInt("MainTabIndex", 0); }
            set { SFile.SetInt("MainTabIndex", value); }
        }
    }
}