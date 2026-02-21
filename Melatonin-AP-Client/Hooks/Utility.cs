namespace Melatonin_AP_Client.Hooks
{
    public static class Utility
    {
        public static playerData CombineData(PlayerDataGame gameData, PlayerDataSettings settingsData)
        {
            var pd = new playerData();
            
            pd.ver = settingsData.ver;
            pd.isVisualAssisting = settingsData.isVisualAssisting;
            pd.isAudioAssisting = settingsData.isAudioAssisting;
            pd.isBiggerHitWindows = settingsData.isBiggerHitWindows;
            pd.isEasyScoring = settingsData.isEasyScoring;
            pd.isVibrationDisabled = settingsData.isVibrationDisabled;
            pd.igc = settingsData.igc;
            pd.isDirectionKeysAlt = settingsData.isDirectionKeysAlt;
            pd.isTp = settingsData.isTp;
            pd.isCreator = settingsData.isCreator;
            pd.isWarmth = settingsData.isWarmth;
            pd.isPerfectsOnly = settingsData.isPerfectsOnly;
            pd.isVsynced = settingsData.isVsynced;
            pd.actionKey = settingsData.actionKey;
            pd.lang = settingsData.lang;
            pd.screenshake = settingsData.screenshake;
            pd.calibrationOffsetMs = settingsData.calibrationOffsetMs;
            pd.audioOffsetMs = settingsData.audioOffsetMs;
            pd.master = settingsData.master;
            pd.music = settingsData.music;
            pd.sfx = settingsData.sfx;
            pd.metronome = settingsData.metronome;
            pd.contrast = settingsData.contrast;
            pd.cn = gameData.cn;
            pd.fd = gameData.fd;
            pd.fdAlt = gameData.fdAlt;
            pd.sp = gameData.sp;
            pd.spAlt = gameData.spAlt;
            pd.tc = gameData.tc;
            pd.tcAlt = gameData.tcAlt;
            pd.fw = gameData.fw;
            pd.fwAlt = gameData.fwAlt;
            pd.id = gameData.id;
            pd.idAlt = gameData.idAlt;
            pd.ec = gameData.ec;
            pd.ecAlt = gameData.ecAlt;
            pd.cr = gameData.cr;
            pd.crAlt = gameData.crAlt;
            pd.sv = gameData.sv;
            pd.svAlt = gameData.svAlt;
            pd.dt = gameData.dt;
            pd.dtAlt = gameData.dtAlt;
            pd.pr = gameData.pr;
            pd.prAlt = gameData.prAlt;
            pd.ft = gameData.ft;
            pd.ftAlt = gameData.ftAlt;
            pd.mn = gameData.mn;
            pd.mnAlt = gameData.mnAlt;
            pd.wr = gameData.wr;
            pd.wrAlt = gameData.wrAlt;
            pd.nr = gameData.nr;
            pd.nrAlt = gameData.nrAlt;
            pd.md = gameData.md;
            pd.mdAlt = gameData.mdAlt;
            pd.ps = gameData.ps;
            pd.psAlt = gameData.psAlt;
            pd.fr = gameData.fr;
            pd.frAlt = gameData.frAlt;
            pd.me = gameData.me;
            pd.meAlt = gameData.meAlt;
            pd.fu = gameData.fu;
            pd.fuAlt = gameData.fuAlt;
            pd.ax = gameData.ax;
            pd.axAlt = gameData.axAlt;
            pd.fn = gameData.fn;
            pd.fnAlt = gameData.fnAlt;
            pd.min = gameData.min;

            return pd;
        }
        
        public static (PlayerDataSettings, PlayerDataGame) SplitData(playerData pd)
        {
            var settingsData = new PlayerDataSettings();
            var gameData = new PlayerDataGame();
            
            settingsData.ver = pd.ver;
            settingsData.isVisualAssisting = pd.isVisualAssisting;
            settingsData.isAudioAssisting = pd.isAudioAssisting;
            settingsData.isBiggerHitWindows = pd.isBiggerHitWindows;
            settingsData.isEasyScoring = pd.isEasyScoring;
            settingsData.isVibrationDisabled = pd.isVibrationDisabled;
            settingsData.igc = pd.igc;
            settingsData.isDirectionKeysAlt = pd.isDirectionKeysAlt;
            settingsData.isTp = pd.isTp;
            settingsData.isCreator = pd.isCreator;
            settingsData.isWarmth = pd.isWarmth;
            settingsData.isPerfectsOnly = pd.isPerfectsOnly;
            settingsData.isVsynced = pd.isVsynced;
            settingsData.actionKey = pd.actionKey;
            settingsData.lang = pd.lang;
            settingsData.screenshake = pd.screenshake;
            settingsData.calibrationOffsetMs = pd.calibrationOffsetMs;
            settingsData.audioOffsetMs = pd.audioOffsetMs;
            settingsData.master = pd.master;
            settingsData.music = pd.music;
            settingsData.sfx = pd.sfx;
            settingsData.metronome = pd.metronome;
            settingsData.contrast = pd.contrast;
            gameData.cn = pd.cn;
            gameData.fd = pd.fd;
            gameData.fdAlt = pd.fdAlt;
            gameData.sp = pd.sp;
            gameData.spAlt = pd.spAlt;
            gameData.tc = pd.tc;
            gameData.tcAlt = pd.tcAlt;
            gameData.fw = pd.fw;
            gameData.fwAlt = pd.fwAlt;
            gameData.id = pd.id;
            gameData.idAlt = pd.idAlt;
            gameData.ec = pd.ec;
            gameData.ecAlt = pd.ecAlt;
            gameData.cr = pd.cr;
            gameData.crAlt = pd.crAlt;
            gameData.sv = pd.sv;
            gameData.svAlt = pd.svAlt;
            gameData.dt = pd.dt;
            gameData.dtAlt = pd.dtAlt;
            gameData.pr = pd.pr;
            gameData.prAlt = pd.prAlt;
            gameData.ft = pd.ft;
            gameData.ftAlt = pd.ftAlt;
            gameData.mn = pd.mn;
            gameData.mnAlt = pd.mnAlt;
            gameData.wr = pd.wr;
            gameData.wrAlt = pd.wrAlt;
            gameData.nr = pd.nr;
            gameData.nrAlt = pd.nrAlt;
            gameData.md = pd.md;
            gameData.mdAlt = pd.mdAlt;
            gameData.ps = pd.ps;
            gameData.psAlt = pd.psAlt;
            gameData.fr = pd.fr;
            gameData.frAlt = pd.frAlt;
            gameData.me = pd.me;
            gameData.meAlt = pd.meAlt;
            gameData.fu = pd.fu;
            gameData.fuAlt = pd.fuAlt;
            gameData.ax = pd.ax;
            gameData.axAlt = pd.axAlt;
            gameData.fn = pd.fn;
            gameData.fnAlt = pd.fnAlt;
            gameData.min = pd.min;

            return (settingsData, gameData);
        }
        
        private static int GetScore(string levelName)
        {
            return levelName switch
            {
                "food" => SaveManager.playerData.fd + SaveManager.playerData.fdAlt,
                "tech" => SaveManager.playerData.tc + SaveManager.playerData.tcAlt,
                "shopping" => SaveManager.playerData.sp + SaveManager.playerData.spAlt,
                "followers" => SaveManager.playerData.fw + SaveManager.playerData.fwAlt,
                "exercise" => SaveManager.playerData.ec + SaveManager.playerData.ecAlt,
                "career" => SaveManager.playerData.cr + SaveManager.playerData.crAlt,
                "money" => SaveManager.playerData.sv + SaveManager.playerData.svAlt,
                "dating" => SaveManager.playerData.dt + SaveManager.playerData.dtAlt,
                "time" => SaveManager.playerData.ft + SaveManager.playerData.ftAlt,
                "mind" => SaveManager.playerData.mn + SaveManager.playerData.mnAlt,
                "space" => SaveManager.playerData.wr + SaveManager.playerData.wrAlt,
                "nature" => SaveManager.playerData.nr + SaveManager.playerData.nrAlt,
                "stress" => SaveManager.playerData.fr + SaveManager.playerData.frAlt,
                "past" => SaveManager.playerData.ps + SaveManager.playerData.psAlt,
                "future" => SaveManager.playerData.fu + SaveManager.playerData.fuAlt,
                "desires" => SaveManager.playerData.me + SaveManager.playerData.meAlt,
                _ => 0
            };
        }
        
        public static bool LevelsComplete(Neighbourhood neighbourhood)
        {
            if (neighbourhood.Landmarks.Length < 4)
                return true;
            var d1 = GetScore(neighbourhood.Landmarks[0].dreamName);
            var d2 = GetScore(neighbourhood.Landmarks[1].dreamName);
            var d3 = GetScore(neighbourhood.Landmarks[2].dreamName);
            var d4 = GetScore(neighbourhood.Landmarks[3].dreamName);
            return d1 > 0 && d2 > 0 && d3 > 0 && d4 > 0;
        }
        
        public static int? GetDreamIndex(this Dream dream)
        {
            return dream switch
            {
                Dream_food _ => 0,
                Dream_tech _ => 1,
                Dream_shopping _ => 2,
                Dream_followers _ => 3,
                Dream_indulgence _ => 4,
                Dream_exercise _ => 5,
                Dream_career _ => 6,
                Dream_money _ => 7,
                Dream_dating _ => 8,
                Dream_pressure _ => 9,
                Dream_time _ => 10,
                Dream_mind _ => 11,
                Dream_space _ => 12,
                Dream_nature _ => 13,
                Dream_meditation _ => 14,
                Dream_stress _ => 15,
                Dream_past _ => 16,
                Dream_future _ => 17,
                Dream_desires _ => 18,
                Dream_setbacks _ => 19,
                _ => null
            };
        }
    }
}
