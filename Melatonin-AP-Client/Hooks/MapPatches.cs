using System.Collections;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Melatonin_AP_Client.Hooks
{
    [HarmonyPatch(typeof(Map))]
    public class MapPatches
    {
        [HarmonyPatch(nameof(Map.Activate))]
        [HarmonyPrefix]
        public static bool Activate(Map __instance)
        {
            __instance.StartCoroutine(Activating(__instance));
            foreach (var title in Map.env.Neighbourhood.DreamTitles)
                title.gameObject.SetActive(false);
            return false;
        }

        private static IEnumerator Activating(Map __instance)
        {
            __instance.GrowBubble.Activate();
            yield return new WaitForSeconds(1.875f);
            __instance.sprites[0].TriggerAnim("deactivate");
            __instance.Feathering.Show();
            foreach (var cloud in __instance.Clouds)
                cloud.Show();
            __instance.Neighbourhood.Show();
            __instance.Floor.Show();
            if (Chapter.GetActiveChapterNum() <= 4)
            {
                foreach (Fence fence in __instance.Fences)
                    fence.Show();
                __instance.Pool.Show();
            }
            __instance.Neighbourhood.McMap.Show();
            __instance.Pool.Enable();
            UnlockHandler.CheckUnlocks(PluginMain.ArchipelagoHandler.starCount);
            __instance.Neighbourhood.McMap.Introduce();
        }
    }
}