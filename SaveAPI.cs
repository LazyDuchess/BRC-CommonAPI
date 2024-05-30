using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;
using UnityEngine.UIElements;

namespace CommonAPI
{
    /// <summary>
    /// Provides events to listen for savegame transactions as they happen.
    /// This can be used to create custom savegame data, for example, by writing and reading from custom files.
    /// </summary>
    public static class SaveAPI
    {
        /// <summary>
        /// Delegate for savegame events.
        /// </summary>
        /// <param name="saveSlot">Slot this savegame is in.</param>
        /// <param name="saveSlotFilename">Filename for the savegame.</param>
        /// <param name="fileId">The number in the filename. Might not be the same as the slot. Use this to name your own save files.</param>
        public delegate void SaveGameDelegate(int saveSlot, string saveSlotFilename, int fileId);
        /// <summary>
        /// Called when a new slate gets created.
        /// </summary>
        public static SaveGameDelegate OnNewGame;
        /// <summary>
        /// Called when the current savegame is being saved.
        /// </summary>
        public static SaveGameDelegate OnSaveGame;
        /// <summary>
        /// Called when a savegame is deleted by the user.
        /// </summary>
        public static SaveGameDelegate OnDeleteGame;
        /// <summary>
        /// Called on stage initialized, when loading into a savegame from the main menu.
        /// </summary>
        public static SaveGameDelegate OnLoadStageInitialized;
        /// <summary>
        /// Called on stage post initialization, when loading into a savegame from the main menu.
        /// </summary>
        public static SaveGameDelegate OnLoadStagePostInitialization;

        internal static bool AlreadyRanOnLoadStageInitialized = false;
        internal static bool AlreadyRanOnLoadStagePostInitialization = false;

        private static List<CustomSaveData> _customSaveDatas = new List<CustomSaveData>();

        public static void RegisterCustomSaveData(CustomSaveData customSaveData)
        {
            if (!customSaveData.Filename.Contains("{0}"))
            {
                throw new Exception("Can't register save data for " + customSaveData.GetType() +", Filename is missing a \"{0}\" token to differentiate save slots. Filename is: " + customSaveData.Filename);
            }
            if (CommonAPISettings.Debug)
                CommonAPIPlugin.Log.LogDebug($"Registering custom save data {customSaveData.GetType()}");
            _customSaveDatas.Add(customSaveData);
        }

        internal static void Initialize()
        {
            StageManager.OnStageInitialized += StageManager_OnStageInitialized;
            StageManager.OnStagePostInitialization += StageManager_OnStagePostInitialization;
        }

        internal static void OnSetCurrentSaveSlot()
        {
            if (!Core.Instance.SaveManager.HasCurrentSaveSlot)
                return;
            var slotId = Core.Instance.SaveManager.CurrentSaveSlot.saveSlotId;
            var saveSlotFilename = Core.Instance.SaveManager.saveSlotHandler.GetSaveSlotFileName(slotId);
            if (CommonAPISettings.Debug)
                CommonAPIPlugin.Log.LogInfo($"Loading into save slot {slotId}, filename: {saveSlotFilename} (Set Current Save Slot)");
            var fileID = GetFilenameID(saveSlotFilename);
            LoadAllCustomData(fileID);
        }

        private static void StageManager_OnStageInitialized()
        {
            if (AlreadyRanOnLoadStageInitialized)
                return;
            var slotId = Core.Instance.SaveManager.CurrentSaveSlot.saveSlotId;
            var saveSlotFilename = Core.Instance.SaveManager.saveSlotHandler.GetSaveSlotFileName(slotId);
            if (CommonAPISettings.Debug)
                CommonAPIPlugin.Log.LogDebug($"Loading into save slot {slotId}, filename: {saveSlotFilename} (Initialized)");
            var fileID = GetFilenameID(saveSlotFilename);
            OnLoadStageInitialized?.Invoke(slotId, saveSlotFilename, fileID);
            AlreadyRanOnLoadStageInitialized = true;
        }

        private static void StageManager_OnStagePostInitialization()
        {
            if (AlreadyRanOnLoadStagePostInitialization)
                return;
            var slotId = Core.Instance.SaveManager.CurrentSaveSlot.saveSlotId;
            var saveSlotFilename = Core.Instance.SaveManager.saveSlotHandler.GetSaveSlotFileName(slotId);
            if (CommonAPISettings.Debug)
                CommonAPIPlugin.Log.LogDebug($"Loading into save slot {slotId}, filename: {saveSlotFilename} (PostInitialiation)");
            OnLoadStagePostInitialization?.Invoke(slotId, saveSlotFilename, GetFilenameID(saveSlotFilename));
            AlreadyRanOnLoadStagePostInitialization = true;
        }

        internal static void DeleteAllCustomData(int fileID)
        {
            foreach (var savedata in _customSaveDatas)
            {
                var filename = savedata.GetFilenameForFileID(fileID);
                var backupFilename = savedata.GetBackupFilenameForFileID(fileID);
                if (File.Exists(filename))
                {
                    if (CommonAPISettings.Debug)
                        CommonAPIPlugin.Log.LogDebug($"Deleting custom data for {savedata.GetType()}, file: {filename}");
                    File.Delete(filename);
                }
                if (File.Exists(backupFilename))
                    File.Delete(backupFilename);
            }
        }

        internal static void SaveAllCustomData(int fileID)
        {
            foreach(var savedata in _customSaveDatas)
            {
                if (!savedata.AutoSave && !savedata.QueuedSave)
                    continue;
                savedata.QueuedSave = false;
                WriteData(savedata, savedata.GetFilenameForFileID(fileID));
            }
        }

        internal static void SaveBackupsOfCustomData(int fileID)
        {
            foreach (var savedata in _customSaveDatas)
            {
                WriteData(savedata, savedata.GetBackupFilenameForFileID(fileID));
            }
        }

        private static void WriteData(CustomSaveData saveData, string filename)
        {
            if (CommonAPISettings.Debug)
                CommonAPIPlugin.Log.LogDebug($"Writing custom data for {saveData.GetType()}, file: {filename}");
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);
            saveData.Write(writer);
            writer.Flush();
            var data = ms.ToArray();
            CustomStorage.Instance.WriteFile(data, filename);
            writer.Dispose();
            ms.Dispose();
        }

        private static bool TryLoadCustomData(CustomSaveData saveData, string filename)
        {
            saveData.FailedToLoad = false;
            try
            {
                var fs = new FileStream(filename, FileMode.Open);
                var reader = new BinaryReader(fs);
                saveData.Read(reader);
                reader.Dispose();
                fs.Dispose();
                if (saveData.FailedToLoad)
                {
                    CommonAPIPlugin.Log.LogError($"Failed to load custom data for {saveData.GetType()} (Handled), file: {filename}");
                    return false;
                }
            }
            catch (Exception e)
            {
                CommonAPIPlugin.Log.LogError($"Failed to load custom data for {saveData.GetType()} (Unhandled), file: {filename}{Environment.NewLine}{e}");
                return false;
            }
            return true;
        }

        internal static void LoadAllCustomData(int fileID)
        {
            foreach(var savedata in _customSaveDatas)
            {
                savedata.QueuedSave = false;
                var filename = savedata.GetFilenameForFileID(fileID);
                var backupFilename = savedata.GetBackupFilenameForFileID(fileID);

                if (!File.Exists(filename))
                    filename = backupFilename;

                if (File.Exists(filename))
                {
                    if (CommonAPISettings.Debug)
                        CommonAPIPlugin.Log.LogDebug($"Loading custom data for {savedata.GetType()}, file: {filename}");
                    var result = TryLoadCustomData(savedata, filename);
                    if (!result)
                    {
                        savedata.Initialize();

                        if (File.Exists(backupFilename))
                        {
                            CommonAPIPlugin.Log.LogInfo($"Will load backup file for {savedata.GetType()}.");
                            result = TryLoadCustomData(savedata, backupFilename);
                        }

                        if (!result)
                        {
                            CommonAPIPlugin.Log.LogInfo($"Starting a clean save for {savedata.GetType()}.");
                            savedata.Initialize();
                        }
                    }
                }
                else
                {
                    if (CommonAPISettings.Debug)
                        CommonAPIPlugin.Log.LogDebug($"Making new custom data for {savedata.GetType()}.");
                    savedata.Initialize();
                }
            }
        }

        internal static void MakeNewForAllCustomData(int fileID)
        {
            foreach (var savedata in _customSaveDatas)
            {
                savedata.QueuedSave = false;
                var filename = savedata.GetFilenameForFileID(fileID);
                if (CommonAPISettings.Debug)
                    CommonAPIPlugin.Log.LogDebug($"Making new custom data for {savedata.GetType()}, file: {filename}");
                savedata.Initialize();
            }
        }
        
        internal static int GetFilenameID(string filename)
        {
            var num = "";
            for(var i=0;i<filename.Length;i++)
            {
                if (char.IsDigit(filename[i]))
                    num += filename[i];
            }
            if (int.TryParse(num, out int result))
                return result;
            return -1;
        }
    }
}
