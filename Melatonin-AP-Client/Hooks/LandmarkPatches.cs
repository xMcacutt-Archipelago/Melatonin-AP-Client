using HarmonyLib;

namespace Melatonin_AP_Client.Hooks
{
    [HarmonyPatch(typeof(Landmark))]
    public class LandmarkPatches
    {
        public static Landmark LastTriggeredLandmark; 
        
        [HarmonyPatch(nameof(Landmark.OnTriggerEnter2D))]
        [HarmonyPrefix]
        private static bool OnTriggerEnter2D(Landmark __instance)
        {
            LastTriggeredLandmark = __instance;
            
            if (__instance.isDisabled)
            {
                Map.env.PlayNopeSound();
                Map.env.RequiredBox.Activate();
            }
            else
            {
                __instance.isTriggered = true;
                __instance.gears[0].TriggerAnim("lower");
                Map.env.PlayToggleSound();
                if (__instance.reactSprites.Length != 0)
                {
                    foreach (Fragment reactSprite in __instance.reactSprites)
                        reactSprite.TriggerAnim("react");
                }
                if (__instance.starScore > 0 || __instance.ringScore > 0)
                    __instance.ScoreBubble.Deactivate();
                Map.env.Neighbourhood.McMap.ReadyUp();
                Map.env.Neighbourhood.McMap.ModeMenu.Activate(__instance.isRemix);
            }

            return false;
        }
        
        [HarmonyPatch(nameof(Landmark.OnTriggerExit2D))]
        [HarmonyPrefix]
        public static bool OnTriggerExit2D(Landmark __instance)
        {
            if (__instance.isDisabled)
            {
                Map.env.RequiredBox.Deactivate();
            }
            else
            {
                __instance.isTriggered = false;
                __instance.gears[0].TriggerAnim("rise");
                Map.env.PlayToggleSound();
                if (__instance.reactSprites.Length != 0)
                {
                    foreach (Fragment reactSprite in __instance.reactSprites)
                    {
                        if (reactSprite.CheckIsAnimExists("reset"))
                            reactSprite.TriggerAnim("reset");
                    }
                }
                if (__instance.starScore > 0 || __instance.ringScore > 0)
                    __instance.ScoreBubble.Activate();
                Map.env.Neighbourhood.McMap.Unready();
                Map.env.Neighbourhood.McMap.ModeMenu.Deactivate(0);
            }

            return false;
        }

        [HarmonyPatch(nameof(Landmark.Show))]
        [HarmonyPrefix]
        public static bool Show(Landmark __instance)
        {
            __instance.RenderChildren(true);
            __instance.starScore = SaveManager.mgr.GetScore("Dream_" + __instance.dreamName);
            __instance.ringScore = SaveManager.mgr.GetScore($"Dream_{__instance.dreamName}Alt");
            if (__instance.starScore > 0 || __instance.ringScore > 0)
            {
                __instance.ScoreBubble.SetNumStars(__instance.starScore);
                __instance.ScoreBubble.SetNumRings(__instance.ringScore);
                __instance.ScoreBubble.Show();
            }
            if (__instance == Map.env.Neighbourhood.GetRemixLandmark())
                __instance.isRemix = true;
            if (Chapter.activeChapterNum == 1 && __instance == Map.env.Neighbourhood.Landmarks[0])
                return false;
            __instance.isDisabled = true;
            __instance.gears[0].TriggerAnim("disabled");
            __instance.sprites[0].TriggerAnim("greyscaled");
            __instance.sprites[1].TriggerAnim("greyscaled");
            __instance.ScoreBubble.Hide();
            return false;
        }
    }
}