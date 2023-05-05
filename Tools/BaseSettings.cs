using UnityEngine;
using BepInEx.Configuration;

namespace ToggleNotifications.Tools
{
    public class BaseSettings : TNSettings
    {
        public static SettingsFile sfile;
        public static string s_settings_path;
        public static void Init(string settings_path)
        {
            sfile = new SettingsFile();
        }
        public static int window_x_pos
        {
            get => sfile.GetInt("window_x_pos", 70);
            set { sfile.SetInt("window_x_pos", value); }
        }

        public static int window_y_pos
        {
            get => sfile.GetInt("window_y_pos", 50);
            set { sfile.SetInt("window_y_pos", value); }
        }

        public static int main_tab_index
        {
            get { return sfile.GetInt("main_tab_index", 0); }
            set { sfile.SetInt("main_tab_index", value); }
        }
    }
}