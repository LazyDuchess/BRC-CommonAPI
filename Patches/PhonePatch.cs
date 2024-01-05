using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Reptile.Phone;
using Reptile;
using UnityEngine;
using CommonAPI.Phone;

namespace CommonAPI.Patches {
    [HarmonyPatch(typeof(Reptile.Phone.Phone))]
    internal class PhonePatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Reptile.Phone.Phone.PhoneInit))]
        private static void PhoneInit_Prefix(Reptile.Phone.Phone __instance, Player setPlayer) {
            var appRoot = __instance.transform.Find("OpenCanvas/PhoneContainerOpen/MainScreen/Apps") as RectTransform;
            var apps = PhoneAPI.Apps;
            foreach(var app in apps) {
                AppUtility.CreateApp(app.AppType, appRoot);
            }
        }
    }
}
