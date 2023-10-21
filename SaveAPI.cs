using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;

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
            _customSaveDatas.Add(customSaveData);
        }

        internal static void Initialize()
        {
            StageManager.OnStageInitialized += StageManager_OnStageInitialized;
            StageManager.OnStagePostInitialization += StageManager_OnStagePostInitialization;
        }

        private static void StageManager_OnStageInitialized()
        {
            if (AlreadyRanOnLoadStageInitialized)
                return;
            var slotId = Core.Instance.SaveManager.CurrentSaveSlot.saveSlotId;
            var saveSlotFilename = Core.Instance.SaveManager.saveSlotHandler.GetSaveSlotFileName(slotId);
            if (CommonAPISettings.Debug)
                CommonAPIPlugin.Log.LogInfo($"Loading into save slot {slotId}, filename: {saveSlotFilename} (Initialized)");
            var fileID = GetFilenameID(saveSlotFilename);
            LoadAllCustomData(fileID);
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
                CommonAPIPlugin.Log.LogInfo($"Loading into save slot {slotId}, filename: {saveSlotFilename} (PostInitialiation)");
            OnLoadStagePostInitialization?.Invoke(slotId, saveSlotFilename, GetFilenameID(saveSlotFilename));
            AlreadyRanOnLoadStagePostInitialization = true;
        }

        internal static void DeleteAllCustomData(int fileID)
        {
            foreach (var savedata in _customSaveDatas)
            {
                var filename = savedata.GetFilenameForFileID(fileID);
                if (File.Exists(filename))
                {
                    if (CommonAPISettings.Debug)
                        CommonAPIPlugin.Log.LogInfo($"Deleting custom data for {savedata.GetType()}, file: {filename}");
                    File.Delete(filename);
                }
            }
        }

        internal static void SaveAllCustomData(int fileID)
        {
            foreach(var savedata in _customSaveDatas)
            {
                var filename = savedata.GetFilenameForFileID(fileID);
                if (CommonAPISettings.Debug)
                    CommonAPIPlugin.Log.LogInfo($"Writing custom data for {savedata.GetType()}, file: {filename}");
                var ms = new MemoryStream();
                var writer = new BinaryWriter(ms);
                savedata.Write(writer);
                writer.Flush();
                CustomStorage.Instance.WriteFile(ms, filename);
                writer.Dispose();
                ms.Dispose();
            }
        }

        internal static void LoadAllCustomData(int fileID)
        {
            foreach(var savedata in _customSaveDatas)
            {
                var filename = savedata.GetFilenameForFileID(fileID);
                if (File.Exists(filename))
                {
                    if (CommonAPISettings.Debug)
                        CommonAPIPlugin.Log.LogInfo($"Loading custom data for {savedata.GetType()}, file: {filename}");
                    var fs = new FileStream(filename, FileMode.Open);
                    var reader = new BinaryReader(fs);
                    savedata.Read(reader);
                    reader.Dispose();
                    fs.Dispose();
                }
                else
                {
                    if (CommonAPISettings.Debug)
                        CommonAPIPlugin.Log.LogInfo($"Making new custom data for {savedata.GetType()}, file: {filename}");
                    savedata.MakeNew();
                }
            }
        }

        internal static void MakeNewForAllCustomData(int fileID)
        {
            foreach (var savedata in _customSaveDatas)
            {
                var filename = savedata.GetFilenameForFileID(fileID);
                if (CommonAPISettings.Debug)
                    CommonAPIPlugin.Log.LogInfo($"Making new custom data for {savedata.GetType()}, file: {filename}");
                savedata.MakeNew();
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
