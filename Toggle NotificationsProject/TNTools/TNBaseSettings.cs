namespace ToggleNotifications.TNTools
{
    public class TNBaseSettings
    {
        public static SettingsFile SFile;
        public static string SettingsPath;

        public static void Init(string settingsPath) => TNBaseSettings.SFile = new SettingsFile(settingsPath);

        public static int WindowXPos
        {
            get => TNBaseSettings.SFile.GetInt(nameof(WindowXPos), 70);
            set => TNBaseSettings.SFile.SetInt(nameof(WindowXPos), value);
        }

        public static int WindowYPos
        {
            get => TNBaseSettings.SFile.GetInt(nameof(WindowYPos), 50);
            set => TNBaseSettings.SFile.SetInt(nameof(WindowYPos), value);
        }

        public static int MainTabIndex
        {
            get => TNBaseSettings.SFile.GetInt(nameof(MainTabIndex), 0);
            set => TNBaseSettings.SFile.SetInt(nameof(MainTabIndex), value);
        }
    }
}
