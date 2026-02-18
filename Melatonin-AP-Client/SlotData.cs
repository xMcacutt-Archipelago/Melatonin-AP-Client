using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Newtonsoft.Json.Linq;

namespace Melatonin_AP_Client
{
    public class SlotData
    {
        public readonly Dictionary<string, string> LevelMapping;
        public int StarsPerLevel; 
    
        public SlotData(Dictionary<string, object> slotDict)
        {
            foreach (var x in slotDict) APConsole.Instance.DebugLog($"{x.Key} {x.Value}");
            if (slotDict.TryGetValue("LevelMapping", out var rawLevelMapping) &&
                rawLevelMapping is JObject levelMapObj)
                LevelMapping = levelMapObj.ToObject<Dictionary<string, string>>() ?? new Dictionary<string, string>();
            else 
                LevelMapping = new Dictionary<string, string>();

            if (slotDict.TryGetValue("StarsPerLevel", out var rawStarsPerLevel) &&
                rawStarsPerLevel is long starsPerLevel)
                StarsPerLevel = (int)starsPerLevel;
        }
    }
}