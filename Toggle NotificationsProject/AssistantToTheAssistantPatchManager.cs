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
                return false;
            }
        }

        [HarmonyPatch(typeof(MessageCenter))]
        public static class MessageCenterPublishPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(MessageCenter.Publish), typeof(System.Type), typeof(MessageCenterMessage))]
            public static bool Prefix(System.Type type, MessageCenterMessage message)
            {
                if (type == typeof(PauseStateChangedMessage))
                {
                    PauseStateChangedMessage pauseMessage = (PauseStateChangedMessage)message;
                    if (pauseMessage.Paused)
                    {
                        Logger.LogInfo("Game is paused");
                        return true;
                    }
                    else
                    {
                        Logger.LogInfo("Game is unpaused");
                        return true;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(UIManager))]
        public static class SetPauseVisiblePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetPauseVisible")]
            public static bool Prefix(UIManager __instance, bool isVisible)
            {
                if (Logger != null)
                {
                    Logger.LogInfo("Prefix Loaded for SetPauseVisible");
                    Logger.LogInfo("IsVisible: " + isVisible);
                }

                if (isVisible)
                {
                    return false;
                }

                return true; 
            }
        }

        [HarmonyPatch(typeof(NotificationEvents))]
        public static class SolarPanelEventsPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SolarPanelsIneffectiveMessage")]

            public static bool Prefix(NotificationEvents __instance)
            {
                Logger.LogInfo("Prefix Loaded for SolarPanel NotificationEvents");
                return false;
            }
        }
        public static void ApplyPatches()
        {
            Harmony harmony = new Harmony("com.github.cvusmo.Toggle-Notifications");
            harmony.PatchAll(typeof(AssistantToTheAssistantPatchManager).Assembly);
        }
    }
}
