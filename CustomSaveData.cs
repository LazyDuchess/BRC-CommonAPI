using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPI
{
    /// <summary>
    /// Custom data that is read and written alongside savegames.
    /// </summary>
    public abstract class CustomSaveData
    {
        /// <summary>
        /// Full file path. This string MUST contain a "{0}" token, in order to differentiate different save slots.
        /// </summary>
        public string Filename;

        /// <summary>
        /// Whether this save data will be auto-saved alongside the savegame slot. If not, you will manually have to queue a save by calling the Save() method, then the data will be saved next time the game saves your current slot.
        /// </summary>
        public bool AutoSave = true;

        internal bool QueuedSave = false;

        private const string ApplicationDirectoryName = "Bomb Rush Cyberfunk Modding";

        /// <summary>
        /// Default constructor. Given a plugin's name and a filename with a "{0}" token to format, will save the data inside BepInEx/config/(PluginName)/saves/(Filename).
        /// This constructor will also register the savedata into the SaveAPI.
        /// </summary>
        public CustomSaveData(string pluginName, string filename, SaveLocations saveLocation = SaveLocations.Documents)
        {
            Filename = Path.Combine(GetSaveLocation(saveLocation), pluginName, "saves", filename);
            SaveAPI.RegisterCustomSaveData(this);
        }

        private string GetSaveLocation(SaveLocations saveLocation) {
            switch (saveLocation) {
                case SaveLocations.BepInEx:
                    return BepInEx.Paths.BepInExConfigPath;
                case SaveLocations.Documents:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.DoNotVerify), ApplicationDirectoryName);
                case SaveLocations.LocalAppData:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), ApplicationDirectoryName);
            }
            return "";
        }

        /// <summary>
        /// Called when creating a new save slot, or if the save slot being loaded doesn't have this custom data.
        /// </summary>
        public virtual void Initialize()
        {

        }

        public virtual void Read(BinaryReader reader)
        {

        }

        public virtual void Write(BinaryWriter writer)
        {

        }

        /// <summary>
        /// Queues a save for the next time the current save slot saves. Doesn't do anything if this data has autosaves enabled.
        /// </summary>
        public void Save()
        {
            QueuedSave = true;
        }

        internal string GetFilenameForFileID(int fileID)
        {
            return string.Format(Filename, fileID);
        }
    }
}
