using HarmonyLib;

namespace Melatonin_AP_Client.Hooks
{
    [HarmonyPatch(typeof(TotalBox))]
    public class TotalBoxPatches
    {
        private static Fragment? starCount;
        public static void SetStars(int count)
        {
            if (starCount?.textMesh == null)
                return;
            string newText = count.ToString() ?? "";
            starCount?.SetText(newText);
        }
        
        [HarmonyPatch(nameof(TotalBox.Activate))]
        [HarmonyPrefix]
        public static bool Activate(TotalBox __instance)
        {
            __instance.RenderChildren(true);
            __instance.SetParent(Interface.env.Cam.GetOuterTransform());
            __instance.SetLocalPosition(12.817f, 7.211f);
            __instance.SetLocalZ(20f);
            __instance.fader.TriggerAnim("fadeIn");
            starCount = __instance.amounts[0];
            SetStars(PluginMain.ArchipelagoHandler.starCount);
            Fragment amount2 = __instance.amounts[1];
            var num = SaveManager.mgr.GetChapterEarnedRings(Chapter.GetActiveChapterNum());
            string newText2 = num.ToString() ?? "";
            amount2.SetText(newText2);
            Fragment amount3 = __instance.amounts[2];
            num = SaveManager.mgr.GetChapterEarnedPerfects(Chapter.GetActiveChapterNum());
            string newText3 = num.ToString() ?? "";
            amount3.SetText(newText3);
            return false;
        }
    }
}