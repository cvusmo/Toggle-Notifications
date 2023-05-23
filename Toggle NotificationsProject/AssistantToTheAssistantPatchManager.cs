using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using System.Reflection.Emit;
using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications
{
    internal static class AssistantToTheAssistantPatchManager
    {
        internal static ManualLogSource Logger { get; set; }
        internal static NotificationToggle NotificationToggle { get; set; }

        //patch states
        internal static bool isGamePaused = true;
        internal static bool isSolarPanelsEnabled = true;

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

            return codes.AsEnumerable();
        }

        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class GamePausePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("GamePauseToggledMessage")]
            internal static bool Prefix(GamePauseToggledMessage __instance)
            {
                if (!isGamePaused)
                {

                    return false;
                }
                else
                {
                    __instance.IsPaused = isGamePaused;
                    return true; 
                }
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
                var codes = new List<CodeInstruction>(instructions);

                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Ldc_I4_1)
                    {
                        codes[i].opcode = OpCodes.Ldsfld;
                        codes[i].operand = AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(AssistantToTheAssistantPatchManager.isSolarPanelsEnabled));
                        codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldc_I4_0));
                        codes.Insert(i + 2, new CodeInstruction(OpCodes.Ceq));
                    }
                }

                Logger.LogInfo("Transpiler Loaded for SolarPanelsIneffectiveMessage in NotificationEvents");
                return TranspilerLogic(codes.AsEnumerable());
            }
        }
        internal static void ApplyPatches(NotificationToggle notificationToggle)
        {
            Harmony harmony = new Harmony("com.github.cvusmo.Toggle-Notifications");
            Logger = BepInEx.Logging.Logger.CreateLogSource("AssistantToTheAssistantPatchManager");
            harmony.PatchAll(typeof(AssistantToTheAssistantPatchManager).Assembly);
        }
    }
}
