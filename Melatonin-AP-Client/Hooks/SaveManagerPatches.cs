using System;
using System.Collections;
using System.IO;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace Melatonin_AP_Client.Hooks
{
    public class PlayerDataSettings
    {
        public int ver = 1;
        public bool isVisualAssisting;
        public bool isAudioAssisting;
        public bool isBiggerHitWindows;
        public bool isEasyScoring;
        public bool isVibrationDisabled;
        public bool igc;
        public bool isDirectionKeysAlt;
        public bool isTp;
        public bool isCreator;
        public bool isWarmth;
        public bool isPerfectsOnly;
        public bool isVsynced;
        public string actionKey = "SPACE";
        public int lang;
        public int screenshake = 2;
        public int calibrationOffsetMs;
        public int audioOffsetMs;
        public int master = 10;
        public int music = 10;
        public int sfx = 10;
        public int metronome = 10;
        public int contrast;
    }

    public class PlayerDataGame
    {
        public int cn = -1;
        public int fd;
        public int fdAlt;
        public int sp;
        public int spAlt;
        public int tc;
        public int tcAlt;
        public int fw;
        public int fwAlt;
        public int id;
        public int idAlt;
        public int ec;
        public int ecAlt;
        public int cr;
        public int crAlt;
        public int sv;
        public int svAlt;
        public int dt;
        public int dtAlt;
        public int pr;
        public int prAlt;
        public int ft;
        public int ftAlt;
        public int mn;
        public int mnAlt;
        public int wr;
        public int wrAlt;
        public int nr;
        public int nrAlt;
        public int md;
        public int mdAlt;
        public int ps;
        public int psAlt;
        public int fr;
        public int frAlt;
        public int me;
        public int meAlt;
        public int fu;
        public int fuAlt;
        public int ax;
        public int axAlt;
        public int fn;
        public int fnAlt;
        public float min;
    }
    
    [HarmonyPatch(typeof(SaveManager))]
    public class SaveManagerPatches
    {

        private static PlayerDataSettings LoadSettings()
        {
            var settingsString = "ArchipelagoSaves/settings.json";
            try
            {
                var settingsData = JsonUtility.FromJson<PlayerDataSettings>(File.ReadAllText(settingsString));
                return settingsData;
            }
            catch
            {
                var settingsData = new PlayerDataSettings();
                File.WriteAllText(settingsString, JsonUtility.ToJson(settingsData));
                return settingsData;
            }
        }

        private static PlayerDataGame LoadGame()
        {
            var saveString =
                $"ArchipelagoSaves/save{PluginMain.ArchipelagoHandler.Slot}{PluginMain.ArchipelagoHandler.seed}";
            var backupString =
                $"ArchipelagoSaves/backup{PluginMain.ArchipelagoHandler.Slot}{PluginMain.ArchipelagoHandler.seed}";
            try
            {
                var gameData = JsonUtility.FromJson<PlayerDataGame>(File.ReadAllText($"{saveString}.json"));
                return gameData;
            }
            catch
            {
                try
                {
                    var gameData = JsonUtility.FromJson<PlayerDataGame>(File.ReadAllText($"{backupString}.json"));
                    File.WriteAllText($"{saveString}.json", JsonUtility.ToJson(gameData));
                    return gameData;
                }
                catch
                {
                    var gameData = new PlayerDataGame();
                    File.WriteAllText($"{saveString}.json", JsonUtility.ToJson(gameData));
                    return gameData;
                }
            }
        }
        
        [HarmonyPatch(nameof(SaveManager.LoadPlayerData))]
        [HarmonyPrefix]
        public static bool LoadPlayerData(SaveManager __instance)
        {
            if (!Directory.Exists("ArchipelagoSaves"))
                Directory.CreateDirectory("ArchipelagoSaves");
            if (PluginMain.ArchipelagoHandler.seed.IsNullOrWhiteSpace() ||
                PluginMain.ArchipelagoHandler.Slot.IsNullOrWhiteSpace())
            {
                SaveManager.playerData = Utility.CombineData(new PlayerDataGame(), LoadSettings());
                return false;
            }
            SaveManager.playerData = Utility.CombineData(LoadGame(), LoadSettings());
            return false;
        }

        private static void SaveGame(PlayerDataGame gameData)
        {
            var saveString =
                $"ArchipelagoSaves/save{PluginMain.ArchipelagoHandler.Slot}{PluginMain.ArchipelagoHandler.seed}";
            var backupString =
                $"ArchipelagoSaves/backup{PluginMain.ArchipelagoHandler.Slot}{PluginMain.ArchipelagoHandler.seed}";
            File.WriteAllText("saveTemp.json", JsonUtility.ToJson(gameData));
            if (File.Exists($"{backupString}.json"))
                File.Delete($"{backupString}.json");
            if (File.Exists($"{saveString}.json"))
                File.Move($"{saveString}.json", $"{backupString}.json");
            File.Move("saveTemp.json", $"{saveString}.json");
        }

        public static void SaveSettings(PlayerDataSettings settingsData)
        {
            var settingsString = "ArchipelagoSaves/settings.json";
            File.WriteAllText("saveSettingsTemp.json", JsonUtility.ToJson(settingsData));
            if (File.Exists(settingsString))
                File.Delete(settingsString);
            File.Move("saveSettingsTemp.json", settingsString);
            File.Delete("saveSettingsTemp.json");
        }

        [HarmonyPatch(nameof(SaveManager.SavePlayerData))]
        [HarmonyPrefix]
        public static bool SavePlayerData(SaveManager __instance)
        {
            if (PluginMain.ArchipelagoHandler.seed.IsNullOrWhiteSpace() || PluginMain.ArchipelagoHandler.Slot.IsNullOrWhiteSpace())
                return false;
            if (!Directory.Exists("ArchipelagoSaves"))
                Directory.CreateDirectory("ArchipelagoSaves");
            __instance.isSavingPlayerDataStacked = false;
            __instance.isSavingPlayerDataStackedQueued = false;
            __instance.CancelCoroutine(__instance.savingPlayerDataStacked);
            __instance.CancelCoroutine(__instance.savingPlayerDataStackedQueued);
            var data = Utility.SplitData(SaveManager.playerData);
            SaveGame(data.Item2);
            SaveSettings(data.Item1);
            return false;
        }

        private static IEnumerator SavingPlayerDataStacked(SaveManager __instance)
        {
            if (PluginMain.ArchipelagoHandler.seed.IsNullOrWhiteSpace() ||
                PluginMain.ArchipelagoHandler.Slot.IsNullOrWhiteSpace())
                yield break;
            if (!Directory.Exists("ArchipelagoSaves"))
                Directory.CreateDirectory("ArchipelagoSaves");
            __instance.isSavingPlayerDataStacked = true;
            var data = Utility.SplitData(SaveManager.playerData);
            SaveGame(data.Item2);
            SaveSettings(data.Item1);
            yield return new WaitForSecondsRealtime(2f);
            __instance.isSavingPlayerDataStacked = false;
        }

        [HarmonyPatch(nameof(SaveManager.SavePlayerDataStacked))]
        [HarmonyPrefix]
        private static bool SavePlayerDataStacked(SaveManager __instance)
        {
            if (__instance.isSavingPlayerDataStacked && !__instance.isSavingPlayerDataStackedQueued)
            {
                __instance.savingPlayerDataStackedQueued =
                    __instance.StartCoroutine(SavingPlayerDataStackedQueued(__instance));
            }
            else
            {
                if (__instance.isSavingPlayerDataStacked)
                    return false;
                __instance.savingPlayerDataStacked = __instance.StartCoroutine(SavingPlayerDataStacked(__instance));
            }

            return false;
        }

        private static IEnumerator SavingPlayerDataStackedQueued(SaveManager __instance)
        {
            __instance.isSavingPlayerDataStackedQueued = true;
            yield return new WaitUntil(() => !__instance.isSavingPlayerDataStacked);
            __instance.isSavingPlayerDataStackedQueued = false;
            __instance.savingPlayerDataStacked = __instance.StartCoroutine(SavingPlayerDataStacked(__instance));
        }

        [HarmonyPatch(nameof(SaveManager.ClearPlayerData))]
        [HarmonyPrefix]
        public static bool ClearPlayerData(SaveManager __instance)
        {
            if (PluginMain.ArchipelagoHandler.seed.IsNullOrWhiteSpace() ||
                PluginMain.ArchipelagoHandler.Slot.IsNullOrWhiteSpace())
                return false;
            if (Directory.Exists("ArchipelagoSaves"))
                Directory.Delete("ArchipelagoSaves");
            return false;
        }
    }
}