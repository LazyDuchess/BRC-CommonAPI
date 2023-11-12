using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPI
{
    /// <summary>
    /// Provides APIs for interacting with the game's stage system.
    /// </summary>
    public static class StageAPI
    {
        public delegate void StageInitializationDelegate(Stage newStage, Stage previousStage);
        /// <summary>
        /// Called before scene objects like public toilets, spawners, npcs, etc. are registered. Ideal for adding new stuff to stages.
        /// </summary>
        public static StageInitializationDelegate OnStagePreInitialization;
    }
}
