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
        [HarmonyPatch(nameof(SaveManager.SaveCurrentSaveSlotBackup))]
        private static void SaveCurrentSaveSlotBackup_Prefix(SaveManager __instance)
        {
            var currentSaveSlot = __instance.saveSlotSettings.currentSaveSlot;
            var filename = __instance.saveSlotHandler.GetSaveSlotFileName(currentSaveSlot);
            var fileId = SaveAPI.GetFilenameID(filename);
            SaveAPI.SaveBackupsOfCustomData(fileId);
        }
    }
}
