using BepInEx.Logging;

namespace TNUtility;

public class TNUtility
{
    private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("ToggleNotification.Utility");
    public static string InputDisableWindowAbbreviation = "_WindowAbbreviation";
    public static string InputDisableWindowName = "_WindowName";

}