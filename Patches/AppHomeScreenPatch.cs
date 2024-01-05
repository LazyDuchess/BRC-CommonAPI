using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile.Phone;
using HarmonyLib;
using Reptile;
using UnityEngine;
using CommonAPI.Phone;

namespace CommonAPI.Patches {
    [HarmonyPatch(typeof(AppHomeScreen))]
    internal class AppHomeScreenPatch {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(AppHomeScreen.Awake))]
        private static void Awake_Postfix(AppHomeScreen __instance) {
            var apps = __instance.m_Apps.ToList();
            foreach(var app in InternalPhoneUtility.Apps) {
                AddApp(app, ref apps);
            }
            __instance.m_Apps = apps.ToArray();
        }

        private static void AddApp(RegisteredPhoneApp app, ref List<HomeScreenApp> apps) {
            var appInstance = ScriptableObject.CreateInstance<HomeScreenApp>();
            appInstance.m_AppName = app.AppType.FullName;
            appInstance.m_DisplayName = app.Metadata.Title;
            appInstance.m_AppIcon = app.Metadata.Icon;
            appInstance.appType = HomeScreenApp.HomeScreenAppType.NONE;
            apps.Add(appInstance);
        }
    }
}
