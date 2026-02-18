using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace Melatonin_AP_Client.Hooks
{
    [HarmonyPatch(typeof(Submenu))]
    public class SubmenuPatches
    {
        private static bool hasLogged;
        [HarmonyPatch(nameof(Submenu.Activate))]
        [HarmonyPostfix]
        public static void OnActivate(Submenu __instance)
        {
            if (!hasLogged)
                APConsole.Instance.Log("Welcome to Melatonin Archipelago.");
            hasLogged = true;
        }

        [HarmonyPatch(nameof(Submenu.SetMainMenu))]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var foundString = false;
            var foundContinue = false;
            foreach (var t in codes)
            {
                if (!foundString) 
                {
                    if (t.opcode != OpCodes.Ldstr || !t.operand.ToString().Equals("melatonin"))
                        foundString = true;
                }
                else if (!foundContinue)
                {
                    if (t.opcode != OpCodes.Ldc_I4_2) continue;
                    t.opcode = OpCodes.Ldc_I4_0;
                    foundContinue = true;
                }
                else
                {
                    if (t.opcode != OpCodes.Ldc_I4_4) continue;
                    t.opcode = OpCodes.Ldc_I4;
                    t.operand = 8750;
                    break;
                }
            }
            return codes.AsEnumerable();
        }

        public static void SetConnect(Submenu submenu)
        {
            ConnectionInfoHandler.Load();
            PluginMain.OptionInputHandler.enabled = true;
            submenu.footer.FadeOutSprite(1f, 0.33f);
            submenu.Menus[0].Deactivate();
            if (submenu.Menus[2].CheckIsActivated())
            {
                submenu.Menus[2].Deactivate();
                submenu.Menus[2].ResetActiveOption();
                submenu.speakers[0].TriggerSound(1);
            }
            else
                submenu.speakers[0].TriggerSound(0);
            submenu.menuType = 2;
            var menu = submenu.Menus[1];
            menu.MenuTitle.title.textMeshPro.text = "ARCHIPELAGO";
            foreach (var option in menu.Options)
                option.SetFunction(0, 0);
            menu.Options[0].SetFunction(2, 8751);
            menu.Options[1].SetFunction(2, 8752);
            menu.Options[2].SetFunction(2, 8753);
            menu.Options[3].SetFunction(2, 8754);
            
            submenu.Menus[1].Activate(submenu.menuDirection);
        }
    }
}