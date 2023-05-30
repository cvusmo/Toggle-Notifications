using BepInEx.Logging;
using HarmonyLib;
using KSP.Game;
using KSP.Messages;
using System.Reflection;
using System.Reflection.Emit;
using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications.PatchManager
{
    internal static class AssistantToTheAssistantPatchManager
    {
        internal static ManualLogSource Logger { get; set; }
        internal static NotificationToggle NotificationToggle { get; set; }

        // Patch states
        internal static bool isGamePaused = true;
        internal static bool isSolarPanelsEnabled = true;
        internal static bool isPauseVisible = true;
        internal static bool isElectricalEnabled = true;
        internal static bool isLostControlEnabled = true;
        internal static bool isCommRangeEnabled = true;
        internal static bool isOutOfFuelEnabled = true;
        internal static bool IsPauseVisible
        {
            get { return isPauseVisible; }
            set { isPauseVisible = value; }
        }
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
        internal static class GamePauseToggledMessagePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch("GamePauseToggledMessage")]
            internal static bool Prefix(GamePauseToggledMessage __instance)
            {
                if (isPauseVisible)
                {
                    isGamePaused = true;
                    isGamePaused = isPauseVisible;
                    return __instance.IsPaused;

                }
                else
                {
                    isGamePaused = false;
                    isGamePaused = isPauseVisible;
                    return true;
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
                        codes[i].operand = AccessTools.Field(typeof(AssistantToTheAssistantPatchManager), nameof(isSolarPanelsEnabled));
                        codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldc_I4_0));
                        codes.Insert(i + 2, new CodeInstruction(OpCodes.Ceq));
                    }
                }

                Logger.LogInfo("Transpiler Loaded for SolarPanelsIneffectiveMessage in NotificationEvents");
                return TranspilerLogic(codes.AsEnumerable());
            }
        }

        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class VesselLostControlMessagePatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch("VesselLostControl")]
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                int descriptionTermIndex = codes.FindIndex(code => code.opcode == OpCodes.Ldloc_2 || code.opcode == OpCodes.Ldloc_S && code.operand.ToString() == "2");

                if (!isLostControlEnabled)
                {
                    codes[descriptionTermIndex] = new CodeInstruction(OpCodes.Ldstr, "");
                }
                return codes;
            }
        }

        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class VesselOutOfElectricityMessagePatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch("VesselOutOfElectricity")]
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                if (!isElectricalEnabled)
                {
                    codes.Clear();
                }

                return codes;
            }
        }

        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class VesselLeftCommunicationRangeMessagePatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch("VesselLeftCommunicationRangeMessage")]
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                if (!AssistantToTheAssistantPatchManager.isCommRangeEnabled)
                {
                    int index = codes.FindIndex(instruction =>
                        instruction.opcode == OpCodes.Callvirt &&
                        instruction.operand is MethodInfo method &&
                        method.Name == "ProcessNotification" &&
                        method.DeclaringType == typeof(NotificationManager));

                    if (index != -1)
                    {
                        codes.RemoveRange(index - 2, 5);
                    }
                }

                return codes;
            }
        }


        //OutOfFuel Patches

        [HarmonyPatch(typeof(NotificationEvents))]
        internal static class OutOfFuelPatch
        {
            [HarmonyTranspiler]
            [HarmonyPatch(typeof(NotificationEvents), "CannotPlaceManeuverNodeWhileOutOfFuelMessage")]
            internal static IEnumerable<CodeInstruction> TranspileCannotPlaceManeuverNode(IEnumerable<CodeInstruction> instructions)
            {
                if (!AssistantToTheAssistantPatchManager.isOutOfFuelEnabled)
                {
                    yield break;
                }

                foreach (CodeInstruction instruction in instructions)
                {
                    yield return instruction;
                }
            }

            [HarmonyTranspiler]
            [HarmonyPatch(typeof(NotificationEvents), "CannotChangeNodeWhileOutOfFuelMessage")]
            internal static IEnumerable<CodeInstruction> TranspileCannotChangeNode(IEnumerable<CodeInstruction> instructions)
            {
                if (!AssistantToTheAssistantPatchManager.isOutOfFuelEnabled)
                {
                    yield break;
                }

                foreach (CodeInstruction instruction in instructions)
                {
                    yield return instruction;
                }
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
