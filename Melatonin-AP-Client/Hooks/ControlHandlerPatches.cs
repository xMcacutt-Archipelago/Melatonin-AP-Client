using HarmonyLib;

namespace Melatonin_AP_Client.Hooks
{
    [HarmonyPatch(typeof(ControlHandler))]
    public class ControlHandlerPatches
    {
        [HarmonyPatch(nameof(ControlHandler.CheckIsActionLeftPressed))]
        [HarmonyPrefix]
        public static bool CheckIsActionLeftPressed(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsActionLeftPressing))]
        [HarmonyPrefix]
        public static bool CheckIsActionLeftPressing(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsLeftPressing))]
        [HarmonyPrefix]
        public static bool CheckIsLeftPressing(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsLeftPressed))]
        [HarmonyPrefix]
        public static bool CheckIsLeftPressed(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsUpPressed))]
        [HarmonyPrefix]
        public static bool CheckIsUpPressed(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsUpPressing))]
        [HarmonyPrefix]
        public static bool CheckIsUpPressing(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsDownPressed))]
        [HarmonyPrefix]
        public static bool CheckIsDownPressed(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsDownPressing))]
        [HarmonyPrefix]
        public static bool CheckIsDownPressing(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsActionRightPressed))]
        [HarmonyPrefix]
        public static bool CheckIsActionRightPressed(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsActionRightPressing))]
        [HarmonyPrefix]
        public static bool CheckIsActionRightPressing(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsActionRightReleased))]
        [HarmonyPrefix]
        public static bool CheckIsActionRightReleased(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsActionPressed))]
        [HarmonyPrefix]
        public static bool CheckIsActionPressed(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
        
        [HarmonyPatch(nameof(ControlHandler.CheckIsActionReleased))]
        [HarmonyPrefix]
        public static bool CheckIsActionReleased(ref bool __result)
        {
            if (!OptionInputHandler.IsInputMode) 
                return true;
            __result = false;
            return false;
        }
    }
}