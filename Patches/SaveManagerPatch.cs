using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;

namespace CommonAPI.Patches
{
    [HarmonyPatch(typeof(SaveManager))]
    internal static class SaveManagerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(SaveManager.SetCurrentSaveSlot))]
        private static void SetCurrentSaveSlot_Prefix(int newCurrentSaveSlotId)
        {
            SaveAPI.AlreadyRanOnLoadStageInitialized = false;
            SaveAPI.AlreadyRanOnLoadStagePostInitialization = false;
        }
    }
}
