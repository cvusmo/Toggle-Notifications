using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications
{
    public static class AssistantToTheAssistantPatchManager
    {
        public static ManualLogSource Logger { get; set; }
        public static NotificationToggle NotificationToggle { get; set; }

        [HarmonyPatch(typeof(NotificationEvents))]
        public static class NotificationEventsPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("GamePauseToggledMessage")]
            public static bool Prefix(NotificationEvents __instance)
            {
                Logger.LogInfo("Prefix Loaded for NotificationEvents");
                // Perform your logic here
                return false;
            }
        }

        [HarmonyPatch(typeof(PauseStateChangedMessage))]
        public static class PauseStateChangedPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("PauseStateChangedMessage")]
            public static bool Prefix(PauseStateChangedMessage __instance)
            {
                Logger.LogInfo("Prefix Loaded for PauseStateChangedMessage");
                Logger.LogInfo("Paused: " + __instance.Paused);
                // Perform your logic here
                return false;
            }
        }

        [HarmonyPatch(typeof(UIManager))]
        public static class SetPauseVisiblePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetPauseVisible")]
            public static void Prefix(UIManager __instance, bool isVisible)
            {
                Logger.LogInfo("Prefix Loaded for SetPauseVisible");
                Logger.LogInfo("IsVisible: " + isVisible);
                // Perform your logic here
            }
        }

        public static void ApplyPatches()
        {
            Harmony harmony = new Harmony("com.github.cvusmo.Toggle-Notifications");
            harmony.PatchAll(typeof(AssistantToTheAssistantPatchManager).Assembly);
        }
    }
}
