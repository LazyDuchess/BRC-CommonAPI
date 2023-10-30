using BepInEx;
using Reptile;
using UnityEngine;
using HarmonyLib;
using System;
using BepInEx.Logging;

namespace CommonAPI
{
    [BepInPlugin(GUID, Name, Version)]
    internal class CommonAPIPlugin : BaseUnityPlugin
    {
        public static CommonAPIPlugin Instance;
        public static ManualLogSource Log => Instance.Logger;
        private const string GUID = "com.LazyDuchess.BRC.CommonAPI";
        private const string Name = "CommonAPI";
        private const string Version = "1.0.0";
        private void Awake()
        {
            Instance = this;
            try
            {
                SaveAPI.Initialize();
                CustomSequenceHandler.Initialize();
                new CustomStorage();
                var harmony = new Harmony(GUID);
                harmony.PatchAll();
                Logger.LogInfo($"{Name} {Version} loaded!");
            }
            catch(Exception e)
            {
                Logger.LogError($"Problem loading {Name} {Version}: {e}");
            }
        }

        private void OnApplicationQuit()
        {
            CustomStorage.Instance.HandleQuit();
        }
    }
}
