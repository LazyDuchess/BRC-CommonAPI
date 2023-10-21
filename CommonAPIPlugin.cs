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
                var harmony = new Harmony(GUID);
                harmony.PatchAll();
                Logger.LogInfo($"{Name} {Version} loaded!");
            }
            catch(Exception e)
            {
                Logger.LogError($"Problem loading {Name} {Version}: {e}");
            }
            StageManager.OnStagePostInitialization += StageManager_OnStagePostInitialization;
        }

        private void StageManager_OnStagePostInitialization()
        {
            Logger.LogInfo("Trying to load some shaders...");
            Logger.LogInfo($"AmbientCharacter: {AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientCharacter)}");
            Logger.LogInfo($"AmbientEnvironment: {AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientEnvironment)}");
            Logger.LogInfo($"AmbientEnvironmentCutout: {AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientEnvironmentCutout)}");
            Logger.LogInfo($"AmbientEnvironmentTransparent: {AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientEnvironmentTransparent)}");

            var ambmat = new Material(AssetAPI.GetShader(AssetAPI.ShaderNames.AmbientEnvironment));
            var playa = WorldHandler.instance.GetCurrentPlayer();
            var comps = playa.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(var comp in comps)
            {
                comp.sharedMaterial = ambmat;
            }
        }
    }
}
