namespace ToggleNotifications.TNTools
{
    public class TNBaseSettings
    {
        public static SettingsFile SFile;
        public static string SettingsPath;

        public static void Init(string settingsPath) => TNBaseSettings.SFile = new SettingsFile(settingsPath);
        public static int WindowXPos
        {
            get => SFile.GetInt(nameof(WindowXPos), 70);
            set => SFile.SetInt(nameof(WindowXPos), value);
        }

        public static int WindowYPos
        {
            get => SFile.GetInt(nameof(WindowYPos), 50);
            set => SFile.SetInt(nameof(WindowYPos), value);
        }

        public static int MainTabIndex
        {
            get => SFile.GetInt(nameof(MainTabIndex), 0);
            set => SFile.SetInt(nameof(MainTabIndex), value);
        }
    }
}