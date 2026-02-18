using HarmonyLib;

namespace Melatonin_AP_Client.Hooks
{
    [HarmonyPatch(typeof(Chapter))]
    public class ChapterPatches
    {
        [HarmonyPatch(nameof(Chapter.ExitToNextChapter))]
        [HarmonyPostfix]
        public static void OnExitToNextChapter(Chapter __instance)
        {
            if (Chapter.activeChapterNum == 5)
                PluginMain.ArchipelagoHandler.Release();
        }
    }
}