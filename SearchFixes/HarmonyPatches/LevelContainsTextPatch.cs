using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFixes.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapLevelFilterModel))]
    [HarmonyPatch("LevelContainsText", MethodType.Normal)]
    internal class LevelContainsTextPatch
    {
		private static bool Prefix(IPreviewBeatmapLevel beatmapLevel, string[] searchTexts, ref bool __result)
		{
			int words = 0;
			int matches = 0;

			string songName = $" {beatmapLevel.songName} ";
			string songSubName = $" {beatmapLevel.songSubName} ";
			string songAuthorName = $" {beatmapLevel.songAuthorName} ";
			string levelAuthorName = $" {beatmapLevel.levelAuthorName} ";

			for (int i = 0; i < searchTexts.Length; i++)
			{
				if (!string.IsNullOrWhiteSpace(searchTexts[i]))
                {
					words++;

					string searchTerm = $" {searchTexts[i]} ";
					if (i == searchTexts.Length - 1)
					{
						searchTerm = searchTerm.Substring(0, searchTerm.Length - 1);
					}

					if (songName.IndexOf(searchTerm, 0, StringComparison.CurrentCultureIgnoreCase) != -1 ||
						songSubName.IndexOf(searchTerm, 0, StringComparison.CurrentCultureIgnoreCase) != -1 ||
						songAuthorName.IndexOf(searchTerm, 0, StringComparison.CurrentCultureIgnoreCase) != -1 ||
						levelAuthorName.IndexOf(searchTerm, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
					{
						matches++;
					}
				}
			}

			if (matches == words)
            {
				__result = true;
            }
			else
            {
				__result = false;
            }

			return false;
		}
	}
}
