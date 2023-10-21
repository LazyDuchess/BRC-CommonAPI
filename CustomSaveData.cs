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
        /// Default constructor. Given a plugin's name and a filename with a "{0}" token to format, will save the data inside BepInEx/config/(PluginName)/saves/(Filename).
        /// This constructor will also register the savedata into the SaveAPI.
        /// </summary>
        public CustomSaveData(string pluginName, string filename)
        {
            Filename = Path.Combine(BepInEx.Paths.ConfigPath, pluginName, "saves", filename);
            SaveAPI.RegisterCustomSaveData(this);
        }

        /// <summary>
        /// Called when creating a new save slot, or if the save slot being loaded doesn't have this custom data.
        /// </summary>
        public virtual void MakeNew()
        {

        }

        public virtual void Read(BinaryReader reader)
        {

        }

        public virtual void Write(BinaryWriter writer)
        {

        }

        internal string GetFilenameForFileID(int fileID)
        {
            return string.Format(Filename, fileID);
        }
    }
}
