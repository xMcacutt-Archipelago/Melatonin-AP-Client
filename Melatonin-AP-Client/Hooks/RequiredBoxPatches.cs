using System;
using HarmonyLib;

namespace Melatonin_AP_Client.Hooks
{
    [HarmonyPatch(typeof(RequiredBox))]
    public class RequiredBoxPatches
    {
        [HarmonyPatch(nameof(RequiredBox.Activate))]
        [HarmonyPrefix]
        public static void Activate(RequiredBox __instance)
        {
            var chapterIndex = Chapter.activeChapterNum;
            var levelIndex = Array.IndexOf(Map.env.Neighbourhood.Landmarks, LandmarkPatches.LastTriggeredLandmark);
            __instance.message.textMeshPro.text = $"unlock with {((chapterIndex - 1) * 4 + levelIndex) * PluginMain.SlotData.StarsPerLevel} stars";
            if (PluginMain.ArchipelagoHandler.starCount >= ((chapterIndex - 1) * 4 + levelIndex) * PluginMain.SlotData.StarsPerLevel)
                __instance.message.textMeshPro.text = $"Complete all of tonight's dreams to unlock";
        }
    }
}