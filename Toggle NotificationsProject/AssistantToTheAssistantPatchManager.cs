using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications
{
    internal static class AssistantToTheAssistantPatchManager
    {
        internal static ManualLogSource Logger { get; set; }
        internal static NotificationToggle NotificationToggle { get; set; }

        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class NotificationEventsPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("GamePauseToggledMessage")]
            internal static bool Prefix(NotificationEvents __instance)
            {
                Logger.LogInfo("Prefix Loaded for NotificationEvents");
                return false;
            }
        }

        [HarmonyPatch(typeof(MessageCenter))]
        internal static class MessageCenterPublishPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(MessageCenter.Publish), typeof(System.Type), typeof(MessageCenterMessage))]
            internal static bool Prefix(System.Type type, MessageCenterMessage message)
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
        internal static class SetPauseVisiblePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SetPauseVisible")]
            internal static bool Prefix(UIManager __instance, bool isVisible)
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
        //Solar Panel Patches
        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class SolarPanelsIneffectiveMessagePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SolarPanelsIneffectiveMessage")]
            internal static bool Prefix(NotificationEvents __instance, MessageCenterMessage msg)
            {
                if (Logger != null)
                {
                    Logger.LogInfo("Prefix Loaded for SolarPanelsIneffectiveMessage in NotificationEvents");
                }
                return false;
            }
        }
        internal static void ApplyPatches(NotificationToggle notificationToggle)
        {
            Harmony harmony = new Harmony("com.github.cvusmo.Toggle-Notifications");
            harmony.PatchAll(typeof(AssistantToTheAssistantPatchManager).Assembly);
        }
    }
}