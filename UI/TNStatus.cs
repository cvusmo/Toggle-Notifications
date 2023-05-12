using BepInEx.Configuration;
using BepInEx.Logging;
using KSP.Game;
using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using UnityEngine;

namespace ToggleNotifications;

public class TNStatus
{
    // Status of last Toggle Notification function
    public enum Status
    {
        ENABLED,
        DISABLED,

    }

    static public Status togglestatus = Status.ENABLED; // Everyone starts out this way...

    static string statusText;
    static double statusTime = 0; // UT of last togglestatus update

    static ConfigEntry<string> initialStatusText;
    static ConfigEntry<double> statusPersistence;
    static ConfigEntry<double> statusFadeTime;


    static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("FPStatus");

    public static void ENABLED(string txt)
    {
        set(Status.ENABLED, txt);
        Logger.LogWarning(txt);
    }

    public static void DISABLED(string txt)
    {
        set(Status.DISABLED, txt);
        Logger.LogError(txt);
    }

    static void set(Status togglestatus, string txt)
    {
        TNStatus.togglestatus = togglestatus;
        statusText = txt;
        double UT = GameManager.Instance.Game.UniverseModel.UniversalTime;
        statusTime = UT + statusPersistence.Value;
    }

    public static void Init(ToggleNotificationsPlugin plugin)
    {
        statusPersistence = plugin.Config.Bind<double>("Status Settings Section", "Satus Hold Time", 20, "Controls time delay (in seconds) before togglestatus beings to fade");
        statusFadeTime = plugin.Config.Bind<double>("Status Settings Section", "Satus Fade Time", 20, "Controls the time (in seconds) it takes for togglestatus to fade");
        initialStatusText = plugin.Config.Bind<string>("Status Settings Section", "Initial Status", "Virgin", "Controls the togglestatus reported at startup prior to the first command");

        // Set the initial and Default values based on config parameters. These don't make sense to need live update, so there're here instead of useing the configParam.Value elsewhere
        statusText = initialStatusText.Value;
    }

    public static void DrawUI(double UT)
    {
        // Indicate togglestatus of last GUI function
        float transparency = 1;

        var status_style = TNStyles.Status;
        //if (togglestatus == Status.VIRGIN)
        //    status_style = FPStyles.label;  
        if (togglestatus == Status.ENABLED)
            status_style.normal.textColor = new Color(0, 1, 0, transparency); // TNStyles.phase_ok;
        if (togglestatus == Status.DISABLED)
            status_style.normal.textColor = new Color(1, 1, 0, transparency); // TNStyles.phase_warning;

        UITools.Separator();
        TNStyles.DrawSectionHeader("Status:", statusText, 60, status_style);
    }
}