using HarmonyLib;

namespace Melatonin_AP_Client.Hooks
{
    enum Result
    {
        None,
        OneStar,
        TwoStars,
        ThreeStars,
        PRank
    }

    [HarmonyPatch(typeof(ScoreMeter))]
    public class ScoreMeterPatches
    {
        [HarmonyPatch(nameof(ScoreMeter.TriggerScoreUpdate))]
        [HarmonyPrefix]
        public static void OnTriggerScoreUpdate(ScoreMeter __instance)
        {
            var gameMode = Dream.dir.GetGameMode();
            if (gameMode < 1 || gameMode > 4)
                return;
            var score = (Result)Dream.dir.GetScore();
            var dreamIndex = Dream.dir.GetDreamIndex();
            if (dreamIndex == null)
                return;
            var locBase = (int)dreamIndex * 6 + 0x100 + (gameMode == 2 || gameMode == 4 ? 3 : 0);
            if (score >= Result.OneStar && !PluginMain.ArchipelagoHandler.IsLocationChecked(locBase + 0))
                PluginMain.ArchipelagoHandler.CheckLocation(locBase + 0);
            if (score >= Result.TwoStars && !PluginMain.ArchipelagoHandler.IsLocationChecked(locBase + 1))
                PluginMain.ArchipelagoHandler.CheckLocation(locBase + 1);
            if (score >= Result.ThreeStars && !PluginMain.ArchipelagoHandler.IsLocationChecked(locBase + 2))
                PluginMain.ArchipelagoHandler.CheckLocation(locBase + 2);
        }
    }
}