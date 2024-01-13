using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPI {
    public enum SaveLocations {
        /// <summary>
        /// Save data will be stored in the BepInEx config folder. So this savedata won't carry over to other R2 profiles or BepInEx installations.
        /// </summary>
        BepInEx,
        /// <summary>
        /// Save data is stored in the user's Documents folder, under Documents/Bomb Rush Cyberfunk Modding/saves.
        /// </summary>
        Documents,
        /// <summary>
        /// Save data is stored in an absolute location.
        /// </summary>
        Absolute
    }
}
