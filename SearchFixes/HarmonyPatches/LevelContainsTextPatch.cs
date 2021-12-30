using HarmonyLib;
using System;

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

			string songName = $" {beatmapLevel.songName.RemoveSpecialCharacters()} ";
			string songSubName = $" {beatmapLevel.songSubName.RemoveSpecialCharacters()} ";
			string songAuthorName = $" {beatmapLevel.songAuthorName.RemoveSpecialCharacters()} ";
			string levelAuthorName = $" {beatmapLevel.levelAuthorName.RemoveSpecialCharacters()} ";

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
