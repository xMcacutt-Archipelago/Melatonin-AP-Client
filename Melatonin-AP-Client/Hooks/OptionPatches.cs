using System;
using System.Collections;
using BepInEx;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Melatonin_AP_Client.Hooks
{
    public class OptionInputHandler : MonoBehaviour
    {
        public static bool IsInputMode = false;
        private static bool inputModeRequested = false;
        private static int inputIndex = 0;

        private static string GetCurrentText()
        {
            return inputIndex switch
            {
                0 => OptionPatches.serverTMP.text,
                1 => OptionPatches.slotTMP.text,
                2 => OptionPatches.passwordTMP.text,
                _ => ""
            };
        }

        private static void SetCurrentText(string text)
        {
            Interface.env.Submenu.PlaySfx(0);
            switch (inputIndex)
            {
                case 0:
                    OptionPatches.serverTMP.text = text;
                    break;
                case 1:
                    OptionPatches.slotTMP.text = text;
                    break;
                case 2:
                    OptionPatches.passwordTMP.text = text;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputIndex));
            }
        }

        public static void InitializeInputMode(int index)
        {
            inputModeRequested = true;
            inputIndex = index;
            if (GetCurrentText() == "server" || GetCurrentText() == "slot" || GetCurrentText() == "password")
                SetCurrentText("");
        }

        private static void CompleteInput()
        {
            Interface.env.Submenu.PlaySfx(1);
            IsInputMode = false;
        }

        private static void ProcessInput(char c)
        {
            if (!IsInputMode)
                return;
            var text = GetCurrentText();
            if (c == '\b' && text.Length > 0)
            {
                text = text.Substring(0, text.Length - 1);
                SetCurrentText(text);
            }
            if (char.IsControl(c))
                return;
            text += c;
            SetCurrentText(text);
        }

        private void OnEnable()
        {
            if (Keyboard.current != null)
                Keyboard.current.onTextInput += ProcessInput;
        }
        
        private void OnDisable()
        {
            if (Keyboard.current != null)
                Keyboard.current.onTextInput -= ProcessInput;
        }
        
        void Update()
        {
            if (IsInputMode)
            {
                if (Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    CompleteInput();
                }
            }

            if (!IsInputMode && inputModeRequested)
            {
                Interface.env.Submenu.PlaySfx(1);
                IsInputMode = true;
                inputModeRequested = false;
            }
        }
    }
    
    [HarmonyPatch(typeof(Option))]
    public class OptionPatches
    {
        public static TextMeshPro? serverTMP;
        public static TextMeshPro? slotTMP; 
        public static TextMeshPro? passwordTMP;

        [HarmonyPatch(nameof(Option.Deactivate))]
        [HarmonyPrefix]
        public static void Deactivate(Option __instance)
        {
            Transform transform;
            switch (__instance.functionNum)
            {
                case 8751:
                case 8752:
                case 8753:
                   transform = __instance.tooltip.transform;
                    transform.position = new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z);
                    transform = __instance.tip.transform;
                    transform.position = new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z);
                    break;
            }
        }
        
        static void SetMessage(string message)
        {
            var menu = Interface.env?.Submenu?.Menus[1];
            if (menu != null)
                menu.MenuTitle.title.textMeshPro.text = message;
        }
        
        [HarmonyPatch(nameof(Option.Activate))]
        [HarmonyPostfix]
        public static void OnActivate(Option __instance)
        {
            switch (__instance.functionNum)
            {
                case 8750:
                    __instance.label.textMeshPro.text = "connect";
                    break;
                case 8751:
                    serverTMP = __instance.label.textMeshPro;
                    __instance.label.textMeshPro.text = ConnectionInfoHandler.savedServer.IsNullOrWhiteSpace() ? "server" : ConnectionInfoHandler.savedServer;
                    __instance.tip.gameObject.SetActive(true);
                    __instance.tip.FadeInText(0.6f, 0.33f);
                    __instance.tip.textMeshPro.text = "server address to connect to";
                    __instance.tooltip.ToggleSpriteRenderer(true);
                    __instance.tooltip.gameObject.SetActive(true);
                    __instance.tooltip.FadeInSprite(1f, 0.33f);
                    var transform = __instance.tooltip.transform;
                    transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
                    transform = __instance.tip.transform;
                    transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
                    break;
                case 8752:
                    slotTMP = __instance.label.textMeshPro;
                    __instance.label.textMeshPro.text = ConnectionInfoHandler.savedSlot.IsNullOrWhiteSpace() ? "slot" : ConnectionInfoHandler.savedSlot;
                    __instance.tip.gameObject.SetActive(true);
                    __instance.tip.FadeInText(0.6f, 0.33f);
                    __instance.tip.textMeshPro.text = "slot name within the world";
                    __instance.tooltip.gameObject.SetActive(true);
                    __instance.tooltip.FadeInSprite(1f, 0.33f);
                    transform = __instance.tooltip.transform;
                    transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
                    transform = __instance.tip.transform;
                    transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
                    break;
                case 8753:
                    passwordTMP = __instance.label.textMeshPro;
                    __instance.label.textMeshPro.text = ConnectionInfoHandler.savedPassword.IsNullOrWhiteSpace() ? "password" : ConnectionInfoHandler.savedPassword;
                    __instance.tip.gameObject.SetActive(true);
                    __instance.tip.FadeInText(0.6f, 0.33f);
                    __instance.tip.textMeshPro.text = "password for the world";
                    __instance.tooltip.gameObject.SetActive(true);
                    __instance.tooltip.FadeInSprite(1f, 0.33f);
                    transform = __instance.tooltip.transform;
                    transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
                    transform = __instance.tip.transform;
                    transform.position = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
                    break;
                case 8754:
                    __instance.label.textMeshPro.text = "connect";
                    break;
            }
        }
        
        [HarmonyPatch(nameof(Option.Select))]
        [HarmonyPostfix]
        public static void OnSelect(Option __instance)
        {
            switch (__instance.functionNum)
            {
                case 8750:
                    if (PluginMain.ArchipelagoHandler.IsConnected)
                    {
                        SaveManagerPatches.SaveSettings(Utility.SplitData(SaveManager.playerData).Item1);
                        SaveManager.mgr.LoadPlayerData();
                        if (SaveManager.mgr.GetChapterNum() == -1 || SaveManager.mgr.GetChapterNum() == 0 || !Builder.mgr.CheckIsFullGame())
                            Interface.env.ExitTo("Chapter_1");
                        else if (SaveManager.mgr.GetChapterNum() >= 1)
                            Interface.env.ExitTo("Chapter_" + SaveManager.mgr.GetChapterNum().ToString());
                        Interface.env.Submenu.PlaySfx(1);
                        break;
                    }
                    SubmenuPatches.SetConnect(Interface.env.Submenu);
                    break;
                case 8751:
                    OptionInputHandler.InitializeInputMode(0);
                    break;
                case 8752:
                    OptionInputHandler.InitializeInputMode(1);
                    break;
                case 8753:
                    OptionInputHandler.InitializeInputMode(2);
                    break;
                case 8754:
                    var server = serverTMP?.text ?? "";
                    var slot = slotTMP?.text ?? "";
                    if (slot.IsNullOrWhiteSpace())
                    {
                        SetMessage("Slot cannot be empty!");
                        return;
                    }
                    var password = passwordTMP?.text ?? "";
                    SetMessage("Creating Session");
                    PluginMain.ArchipelagoHandler.CreateSession(server, slot, password);
                    SetMessage("Connecting");
                    PluginMain.ArchipelagoHandler.OnConnected += () =>
                    {
                        ConnectionInfoHandler.Save(server, slot, password);
                        SaveManagerPatches.SaveSettings(Utility.SplitData(SaveManager.playerData).Item1);
                        SaveManager.mgr.LoadPlayerData();
                        if (SaveManager.mgr.GetChapterNum() == -1 || SaveManager.mgr.GetChapterNum() == 0 || !Builder.mgr.CheckIsFullGame())
                            Interface.env.ExitTo("Chapter_1");
                        else if (SaveManager.mgr.GetChapterNum() >= 1)
                            Interface.env.ExitTo("Chapter_" + SaveManager.mgr.GetChapterNum().ToString());
                        Interface.env.Submenu.PlaySfx(1);
                    };
                    PluginMain.ArchipelagoHandler.Connect();
                    break;
            }
        }
    }
}