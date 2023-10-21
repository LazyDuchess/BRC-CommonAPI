using System;
using System.Collections.Generic;
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
        public delegate void SaveGameDelegate(int saveSlot, string saveSlotFilename);
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
            OnLoadStageInitialized?.Invoke(slotId, saveSlotFilename);
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
            OnLoadStagePostInitialization?.Invoke(slotId, saveSlotFilename);
            AlreadyRanOnLoadStagePostInitialization = true;
        }
    }
}
