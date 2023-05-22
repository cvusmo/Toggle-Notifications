namespace ToggleNotifications.TNTools
{
    internal class TNBaseSettings
    {
        internal static SettingsFile SFile;
        internal static string SettingsPath;

        internal static void Init(string settingsPath) => TNBaseSettings.SFile = new SettingsFile(settingsPath);

        internal static int WindowXPos
        {
            get => TNBaseSettings.SFile.GetInt(nameof(WindowXPos), 70);
            set => TNBaseSettings.SFile.SetInt(nameof(WindowXPos), value);
        }

        internal static int WindowYPos
        {
            get => TNBaseSettings.SFile.GetInt(nameof(WindowYPos), 50);
            set => TNBaseSettings.SFile.SetInt(nameof(WindowYPos), value);
        }

        internal static int MainTabIndex
        {
            get => TNBaseSettings.SFile.GetInt(nameof(MainTabIndex), 0);
            set => TNBaseSettings.SFile.SetInt(nameof(MainTabIndex), value);
        }
    }
}
