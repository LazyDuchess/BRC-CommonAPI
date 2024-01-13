using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile;

namespace CommonAPI.Patches
{
    [HarmonyPatch(typeof(SaveSlotHandler))]
    internal static class SaveSlotHandlerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(SaveSlotHandler.SetCurrentSaveSlotDataBySlotId))]
        private static void SetCurrentSaveSlotDataBySlotID_Postfix(int saveSlotId)
        {
            SaveAPI.AlreadyRanOnLoadStageInitialized = false;
            SaveAPI.AlreadyRanOnLoadStagePostInitialization = false;
            SaveAPI.OnSetCurrentSaveSlot();
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(SaveSlotHandler.SaveSaveSlot))]
        private static void SaveSaveSlot_Prefix(SaveSlotHandler __instance, int saveSlotId)
        {
            if (saveSlotId <= -1)
                return;
            var saveSlotFilename = __instance.GetSaveSlotFileName(saveSlotId);
            if (string.IsNullOrEmpty(saveSlotFilename))
                return;
            var fileID = SaveAPI.GetFilenameID(saveSlotFilename);
            SaveAPI.SaveAllCustomData(fileID);
            if (CommonAPISettings.Debug)
                CommonAPIPlugin.Log.LogInfo($"Saving {saveSlotFilename} in slot {saveSlotId}");
            SaveAPI.OnSaveGame?.Invoke(saveSlotId, saveSlotFilename, fileID);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(SaveSlotHandler.DeleteSaveSlotData))]
        private static void DeleteSaveSlotData_Prefix(SaveSlotHandler __instance, int slotId)
        {
            if (slotId <= -1)
                return;
            var saveSlotFilename = __instance.GetSaveSlotFileName(slotId);
            if (string.IsNullOrEmpty(saveSlotFilename))
                return;
            var fileID = SaveAPI.GetFilenameID(saveSlotFilename);
            SaveAPI.DeleteAllCustomData(fileID);
            if (CommonAPISettings.Debug)
                CommonAPIPlugin.Log.LogInfo($"Deleting save {saveSlotFilename} in slot {slotId}");
            SaveAPI.OnDeleteGame?.Invoke(slotId, saveSlotFilename, fileID);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(SaveSlotHandler.AddNewSaveSlot))]
        private static void AddNewSaveSlot_Prefix(SaveSlotHandler __instance, int saveSlotId)
        {
            var saveSlotFilename = __instance.GenerateNewSaveSlotFileName(saveSlotId);
            var fileID = SaveAPI.GetFilenameID(saveSlotFilename);
            SaveAPI.MakeNewForAllCustomData(fileID);
            if (CommonAPISettings.Debug)
                CommonAPIPlugin.Log.LogInfo($"Creating save {saveSlotFilename} in slot {saveSlotId}");
            SaveAPI.OnNewGame?.Invoke(saveSlotId, saveSlotFilename, fileID);
        }
    }
}
