using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using System.Reflection;
using System.Reflection.Emit;
using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications
{
    internal static class AssistantToTheAssistantPatchManager
    {
        internal static ManualLogSource Logger { get; set; }
        internal static NotificationToggle NotificationToggle { get; set; }

        // Patch states
        internal static bool isGamePaused = true;
        internal static bool isSolarPanelsEnabled = true;
        internal static bool isPauseVisible = false;

        internal static NotificationToggle ToggleInstance { get; private set; }

        private static IEnumerable<CodeInstruction> TranspilerLogic(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Brfalse_S && codes[i + 1].opcode == OpCodes.Ldarg_0 && codes[i + 2].opcode == OpCodes.Ldfld && codes[i + 2].operand is FieldInfo field && field.Name == "_isGamePaused")
                {
                    codes[i] = new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(isGamePaused)));
                    codes[i + 1] = new CodeInstruction(OpCodes.Brtrue_S, codes[i + 1].operand);
                }
            }

            return codes;
        }

        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class NotificationEventsPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("GamePauseToggledMessage")]
            internal static bool Prefix(GamePauseToggledMessage __instance)
            {
                if (!isPauseVisible)
                {
                    // Disable the notification from showing by returning false
                    return false;
                }
                else
                {
                    // Update the IsPaused field with the game pause state and isPausePublish
                    __instance.IsPaused = isGamePaused;
                    return true; // Continue with the original method
                }
            }
        }

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

        internal static class SetPauseVisiblePatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch(typeof(UIManager), "SetPauseVisible")]
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                var originalMethod = AccessTools.PropertyGetter(typeof(PauseStateChangedMessage), "IsPaused");

                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Callvirt && codes[i].operand == originalMethod)
                    {
                        codes[i] = new CodeInstruction(OpCodes.Nop);
                        codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldc_I4_1));
                        codes.Insert(i + 2, new CodeInstruction(OpCodes.Ret));
                    }
                }

                AssistantToTheAssistantPatchManager.Logger.LogInfo("Transpiler Loaded for SetPauseVisible");

                return codes.AsEnumerable();
            }
        }
        internal static void ApplyPatches(NotificationToggle notificationToggle)
        {
            ToggleInstance = notificationToggle;
            Harmony harmony = new Harmony("com.github.cvusmo.Toggle-Notifications");
            Logger = BepInEx.Logging.Logger.CreateLogSource("AssistantToTheAssistantPatchManager");
            harmony.PatchAll(typeof(AssistantToTheAssistantPatchManager).Assembly);
        }
    }
}
