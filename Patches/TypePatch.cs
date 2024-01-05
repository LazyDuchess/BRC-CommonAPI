using CommonAPI.Phone;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Android;

namespace CommonAPI.Patches {
    // Nite's gonna hate this :3 but hey if it ain't broke.
    [HarmonyPatch(typeof(Type))]
    internal class TypePatch {

        private const string ReptilePhoneNamespace = "Reptile.Phone.";

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Type.GetType), typeof(string))]
        private static void GetType_Postfix(string typeName, ref Type __result) {
            if (!typeName.StartsWith(ReptilePhoneNamespace)) return;
            var nameOnly = typeName.Substring(ReptilePhoneNamespace.Length);
            if (PhoneAPI.PhoneAppByTypeName.TryGetValue(nameOnly, out var result)) {
                __result = result.AppType;
            }
        }
    }
}
