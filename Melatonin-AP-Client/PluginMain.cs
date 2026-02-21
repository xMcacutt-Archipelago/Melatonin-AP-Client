using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Melatonin_AP_Client.Hooks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Melatonin_AP_Client
{
    [BepInPlugin(GUID, PluginName, Version)]
    public class PluginMain : BaseUnityPlugin
    {        
        public static ConfigEntry<bool>? EnableDebugLogging;
        public static ConfigEntry<bool>? FilterLog;
        public static ConfigEntry<float>? MessageInTime;
        public static ConfigEntry<float>? MessageHoldTime;
        public static ConfigEntry<float>? MessageOutTime;
        public const string GameName = "Melatonin";
        private const string PluginName = "MelatoninAPClient";
        private const string GUID = "melatonin_ap_client";
        private const string Version = "1.0.1";

        private readonly Harmony _harmony = new Harmony(GUID);
        public static ManualLogSource? logger;
        
        public static ArchipelagoHandler ArchipelagoHandler;
        public static OptionInputHandler OptionInputHandler;
        public static SlotData SlotData;
        
        void Awake()
        {
            logger = Logger;
            _harmony.PatchAll();
            DontDestroyOnLoad(gameObject);
            
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                var landmarks = FindObjectsOfType<Landmark>();
                foreach (var landmark in landmarks)
                {
                    if (!SlotData.LevelMapping.TryGetValue(landmark.dreamName, out var newDreamName))
                        continue;
                    landmark.dreamName = newDreamName;
                }
            };
            
            ArchipelagoHandler = gameObject.AddComponent<ArchipelagoHandler>();
            OptionInputHandler = gameObject.AddComponent<OptionInputHandler>();
            OptionInputHandler.enabled = false;
            EnableDebugLogging = Config.Bind(
                "Logging",
                "EnableDebugLogging",
                false,
                "Enables or disables debug logging in the Archipelago Console."
            );
            
            FilterLog = Config.Bind(
                "Logging",
                "FilterLog",
                false,
                "Filter the archipelago log to only show messages relevant to you."
            );
            
            MessageInTime = Config.Bind(
                "Logging",
                "MessageInTime",
                0.25f,
                "How long messages take to animate in."
            );
            
            MessageHoldTime = Config.Bind(
                "Logging",
                "MessageHoldTime",
                3f,
                "How long messages stay in the log before animating out."
            );
            
            MessageOutTime = Config.Bind(
                "Logging",
                "MessageOutTime",
                0.5f,
                "How long messages stay in the log before animating out."
            );
        }

        void Update()
        {
            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                APConsole.Instance.Log(ArchipelagoHandler.starCount.ToString());
            }
        }
    }
}