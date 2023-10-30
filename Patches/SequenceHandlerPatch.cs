using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using HarmonyLib;

namespace CommonAPI.Patches
{
    [HarmonyPatch(typeof(SequenceHandler))]
    internal class SequenceHandlerPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(SequenceHandler.UpdateSequenceHandler))]
        private static bool UpdateSequenceHandler_Prefix()
        {
            if (CustomSequenceHandler.instance == null)
                return true;
            if (CustomSequenceHandler.instance.isBusy)
                return false;
            return true;
        }
    }
}
