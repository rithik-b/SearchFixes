using HarmonyLib;
using System.Collections.Generic;

namespace SearchFixes.HarmonyPatches
{
    [HarmonyPatch(typeof(LevelFilter))]
    [HarmonyPatch("FilterLevelByText", MethodType.Normal)]
    internal class LevelFilterPatch
    {
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
            var codes = new List<CodeInstruction>(instructions);
            var index = codes.FindIndex(x => x.opcode == System.Reflection.Emit.OpCodes.Newarr && x.operand.ToString() == "System.String");
            var offset = 24;
            if (index != -1)
            {
                var newCodes = new List<CodeInstruction>();
                newCodes.AddRange(codes.GetRange(0, index - 1));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Ldc_I4_7));
                newCodes.AddRange(codes.GetRange(index, offset));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Dup));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Ldc_I4_5));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Ldstr, " "));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Stelem_Ref));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Dup));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Ldc_I4_6));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Ldloc_2));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Callvirt, AccessTools.Property(typeof(IPreviewBeatmapLevel), nameof(IPreviewBeatmapLevel.levelAuthorName)).GetGetMethod()));
                newCodes.Add(new CodeInstruction(System.Reflection.Emit.OpCodes.Stelem_Ref));
                newCodes.AddRange(codes.GetRange(index + offset, codes.Count - index - offset));
                // Gonna leave this in for debugging purposes
                //for (int i = 0; i < index + offset + 9 + 10; i++)
                //{
                //    Plugin.Log.Notice($"{i} {newCodes[i].opcode} {newCodes[i].operand}");
                //}
                return newCodes;
            }

            return instructions;
		}
	}
}
