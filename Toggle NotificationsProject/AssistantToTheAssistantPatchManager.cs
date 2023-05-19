﻿using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using ToggleNotifications.TNTools;
using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications
{
    public static class AssistantToTheAssistantPatchManager
    {
        public static ManualLogSource Logger { get; set; }
        public static NotificationToggle NotificationToggle { get; set; }


        [HarmonyPatch(typeof(NotificationEvents))]
        public static class GamePauseTogglePatch
        {
            public static NotificationToggle NotificationToggle { get; internal set; }

            public static void Prefix(ref bool __instance, NotificationToggle notificationToggle)
            {
                bool gamePauseToggledMessageToggle = notificationToggle.GetNotificationState(NotificationType.GamePauseToggledMessage);

                if (!gamePauseToggledMessageToggle)
                {
                    __instance = true;
                }
                else
                {
                    __instance = false;
                }
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
        //Solar Panel Patches
        [HarmonyPatch(typeof(NotificationEvents))]
        public static class SolarPanelsIneffectiveMessagePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("SolarPanelsIneffectiveMessage")]
            public static bool Prefix(NotificationEvents __instance, MessageCenterMessage msg)
            {
                if (Logger != null)
                {
                    Logger.LogInfo("Prefix Loaded for SolarPanelsIneffectiveMessage in NotificationEvents");
                }
                return false;
            }
        }
        public static void ApplyPatches(NotificationToggle notificationToggle)
        {
            Harmony harmony = new Harmony("com.github.cvusmo.Toggle-Notifications");
            harmony.PatchAll(typeof(AssistantToTheAssistantPatchManager).Assembly);

            // Pass the NotificationToggle instance to the GamePauseTogglePatch class
            GamePauseTogglePatch.NotificationToggle = notificationToggle;
        }

    }
}