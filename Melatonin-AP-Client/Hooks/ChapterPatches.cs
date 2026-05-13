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
        
        [HarmonyPatch(nameof(Chapter.Start))]
        [HarmonyPrefix]
        public static bool OnStart(Chapter __instance)
        {
            switch (SceneMonitor.mgr.GetActiveSceneName())
            {
                case "Chapter_1":
                    Chapter.activeChapterNum = 1;
                    break;
                case "Chapter_2":
                    Chapter.activeChapterNum = 2;
                    break;
                case "Chapter_3":
                    Chapter.activeChapterNum = 3;
                    break;
                case "Chapter_4":
                    Chapter.activeChapterNum = 4;
                    break;
                case "Chapter_5":
                    Chapter.activeChapterNum = 5;
                    break;
            }
            LivingRoom.env.Show();
            Technician.mgr.ToggleVsync(true);
            Interface.env.Letterbox.Show();
            MusicBox.ResetCustomSongClip();
            if (SaveManager.mgr.GetChapterNum() < Chapter.activeChapterNum)
            {
                __instance.ResetCache();
                __instance.EnterWithIntro();
            }
            else if (__instance.CheckIsOnSavedChapter() && __instance.CheckIsChapterComplete() && !SaveManager.mgr.CheckIsGameComplete() && Builder.mgr.CheckIsFullGame() || Chapter.isEnteringWithOutro)
            {
                __instance.ResetCache();
                __instance.ExitToNextChapter();
            }
            else if (Chapter.isEnteringWithIntro)
            {
                __instance.ResetCache();
                __instance.EnterWithIntro();
            }
            else if (Chapter.isEnteringFromDream)
            {
                Chapter.isEnteringFromDream = false;
                __instance.EnterFromDream();
            }
            else
            {
                __instance.ResetCache();
                __instance.EnterWithContinue();
            }

            return false;
        }
    }
}