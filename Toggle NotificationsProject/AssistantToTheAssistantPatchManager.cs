using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using System.Collections.Generic;
using System.Reflection.Emit;
using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications
{
    internal static class AssistantToTheAssistantPatchManager
    {
        internal static ManualLogSource Logger { get; set; }
        internal static NotificationToggle NotificationToggle { get; set; } 

        //patch states
        private static bool isPatchEnabled = true;
        internal static bool isGamePaused = false;

        private static IEnumerable<CodeInstruction> TranspilerLogic(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_I4_0)
                {
                    // Replace the false value with our local variable
                    codes[i].opcode = OpCodes.Ldsfld;
                    codes[i].operand = AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(isGamePaused));
                }
                else if (codes[i].opcode == OpCodes.Ldc_I4_1)
                {
                    // Replace the true value with the inverted local variable
                    codes[i].opcode = OpCodes.Ldsfld;
                    codes[i].operand = AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(isGamePaused));
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldc_I4_0));
                    codes.Insert(i + 2, new CodeInstruction(OpCodes.Ceq));
                }
            }

            return codes.AsEnumerable();
        }

        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class NotificationEventsPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("GamePauseToggledMessage")]
            internal static bool Prefix(GamePauseToggledMessage __instance)
            {
                __instance.IsPaused = isGamePaused; // Update the IsPaused field with the game pause state
                return true; // Continue with the original method
            }
        }

        [HarmonyPatch(typeof(MessageCenter))]
        internal static class MessageCenterPublishPatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch(nameof(MessageCenter.Publish), typeof(System.Type), typeof(MessageCenterMessage))]
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var originalMethod = AccessTools.Method(typeof(PauseStateChangedMessage), "get_Paused");

                var codes = new List<CodeInstruction>(instructions);

                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand == originalMethod)
                    {
                        codes[i] = new CodeInstruction(OpCodes.Nop);
                        codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldc_I4_1));
                        codes.Insert(i + 2, new CodeInstruction(OpCodes.Ret));
                    }
                }

                Logger.LogInfo("Transpiler Loaded for MessageCenter.Publish");

                return codes.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(UIManager))]
        internal static class SetPauseVisiblePatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch("SetPauseVisible")]
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                Logger.LogInfo("Transpiler Loaded for SetPauseVisible");
                return TranspilerLogic(instructions);
            }
        }

        //Solar Panel Patches
        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class SolarPanelsIneffectiveMessagePatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch("SolarPanelsIneffectiveMessage")]
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                Logger.LogInfo("Transpiler Loaded for SolarPanelsIneffectiveMessage in NotificationEvents");
                return TranspilerLogic(instructions);
            }
        }

        internal static void ApplyPatches(NotificationToggle notificationToggle)
        {
            Harmony harmony = new Harmony("com.github.cvusmo.Toggle-Notifications");
            harmony.PatchAll(typeof(AssistantToTheAssistantPatchManager).Assembly);
        }
    }
}
