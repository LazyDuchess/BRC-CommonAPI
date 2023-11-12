using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;

namespace CommonAPI.Patches
{
    [HarmonyPatch(typeof(StageManager))]
    internal static class StageManagerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StageManager.SetupWorldHandler))]
        private static void SetupWorldHandler_Prefix(Stage newStage, Stage prevStage)
        {
            StageAPI.OnStagePreInitialization?.Invoke(newStage, prevStage);
        }
    }
}
