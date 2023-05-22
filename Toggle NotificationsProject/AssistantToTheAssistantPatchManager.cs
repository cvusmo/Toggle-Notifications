using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using KSP.Logging;
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
        internal static bool isPauseVisible = false;
        internal static bool isPausePublish = false;
        internal static bool isOnPaused = true;

        private static IEnumerable<CodeInstruction> TranspilerLogic(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                // Existing code...

                if (codes[i].opcode == OpCodes.Ldc_I4_1)
                {
                    // Replace the true value with the inverted local variable
                    codes[i].opcode = OpCodes.Ldsfld;
                    codes[i].operand = AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(isGamePaused));
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldc_I4_0));
                    codes.Insert(i + 2, new CodeInstruction(OpCodes.Ceq));
                }
            }

            // Add the isPauseVisible check here
            codes.Insert(0, new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(isPauseVisible))));
            codes.Insert(1, new CodeInstruction(OpCodes.Brfalse_S, codes[codes.Count - 1].labels[0]));

            // Add the isPausePublish check here
            codes.Insert(0, new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(isPausePublish))));
            codes.Insert(1, new CodeInstruction(OpCodes.Brfalse_S, codes[codes.Count - 1].labels[0]));

            return codes.AsEnumerable();
        }

        internal static class NotificationEventsPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("GamePauseToggledMessage")]
            internal static bool Prefix(GamePauseToggledMessage __instance)
            {
                __instance.IsPaused = isGamePaused && isPausePublish && isPausePublish && isOnPaused; // Update the IsPaused field with the game pause state and isPausePublish
                return true; // Continue with the original method
            }
        }
        [HarmonyPatch(typeof(MessageCenter))]
        internal static class MessageCenterPublishPatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch(nameof(MessageCenter.Publish), typeof(System.Type), typeof(MessageCenterMessage))]
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
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

                // Find the return instruction
                var returnIndex = codes.FindLastIndex(code => code.opcode == OpCodes.Ret);
                if (returnIndex >= 0)
                {
                    // Insert the isPausePublish check before the return instruction
                    var label = generator.DefineLabel();
                    codes.Insert(returnIndex, new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(isPausePublish))));
                    codes.Insert(returnIndex + 1, new CodeInstruction(OpCodes.Brfalse_S, label));
                    codes.Insert(returnIndex + 2, new CodeInstruction(OpCodes.Ret));
                    codes[returnIndex + 3].labels.Add(label);
                }

                Logger.LogInfo("Transpiler Loaded for MessageCenter.Publish");

                // Insert a default return instruction in case the code flow reaches the end of the method
                codes.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
                codes.Add(new CodeInstruction(OpCodes.Ret));

                return codes.AsEnumerable();
            }
        }

        internal static class OnGamePauseToggledPatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch(typeof(NotificationUI), "OnGamePauseToggled")]
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                var originalMethod = AccessTools.Method(typeof(GamePauseToggledMessage), "get_IsPaused");

                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand == originalMethod)
                    {
                        codes[i] = new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(AssistantToTheAssistantPatchManager.isOnPaused)));
                    }
                }

                AssistantToTheAssistantPatchManager.Logger.LogInfo("Transpiler Loaded for OnGamePauseToggled");

                return codes.AsEnumerable();
            }
        }

        internal static class SetPauseVisiblePatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch(typeof(UIManager), "SetPauseVisible")]
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

                Logger.LogInfo("Transpiler Loaded for SetPauseVisible");

                return codes.AsEnumerable();
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

        ///internal static void SetPauseState(bool isPaused, bool isVisible, bool shouldPublish)
       /// {
           /// isGamePaused = isPaused;
           /// isPauseVisible = isVisible;
           /// isPausePublish = shouldPublish;
       /// }
        internal static void ApplyPatches(NotificationToggle notificationToggle)
        {
            Harmony harmony = new Harmony("com.github.cvusmo.Toggle-Notifications");
            harmony.PatchAll(typeof(AssistantToTheAssistantPatchManager).Assembly);
        }
    }
}
