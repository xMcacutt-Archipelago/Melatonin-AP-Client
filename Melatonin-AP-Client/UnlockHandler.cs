using BepInEx;
using Utility = Melatonin_AP_Client.Hooks.Utility;

namespace Melatonin_AP_Client
{
    public class UnlockHandler
    {
        public static void CheckUnlocks(int starCount)
        {
            if (Map.env?.Neighbourhood?.McMap == null)
                return;
            var chapter = Chapter.activeChapterNum;
            var levelIndex = 0;
            var levelsComplete = Utility.LevelsComplete(Map.env.Neighbourhood);
            foreach (var landmark in Map.env.Neighbourhood.Landmarks)
            {
                APConsole.Instance.DebugLog($"SPL: {PluginMain.SlotData.StarsPerLevel} | Star Count: {starCount} | Chapter Index: {chapter - 1} | Level Index: {levelIndex} | Levels Complete: {levelsComplete}");
                if (landmark.isRemix && !levelsComplete)
                    continue;
                if (starCount >= ((chapter - 1) * 4 + levelIndex) * PluginMain.SlotData.StarsPerLevel)
                    landmark.Enable(true);
                levelIndex++;
            }
        }
    }
}