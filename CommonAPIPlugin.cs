using BepInEx;
using Reptile;
using UnityEngine;
using HarmonyLib;
using System;
using BepInEx.Logging;
using CommonAPI.Phone;

namespace CommonAPI
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    internal class CommonAPIPlugin : BaseUnityPlugin
    {
        public static CommonAPIPlugin Instance;
        public static ManualLogSource Log => Instance.Logger;
        private void Awake()
        {
            Instance = this;
            try
            {
                PhoneAPI.Initialize();
                SaveAPI.Initialize();
                CustomSequenceHandler.Initialize();
                new CustomStorage();
                //PhoneAPI.RegisterApp<AppTest>("test app");
                var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
                harmony.PatchAll();
                Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} loaded!");
            }
            catch(Exception e)
            {
                Logger.LogError($"Problem loading {PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION}!{Environment.NewLine}{e}");
            }
        }

        private void OnApplicationQuit()
        {
            CustomStorage.Instance.HandleQuit();
        }
    }
}
