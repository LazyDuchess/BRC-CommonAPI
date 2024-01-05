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
            var availableApps = __instance.availableHomeScreenApps.ToList();
            foreach(var app in PhoneAPI.Apps) {
                AddApp(app, ref availableApps);
            }
            __instance.availableHomeScreenApps = availableApps.ToArray();
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(AppHomeScreen.OnAppEnable))]
        private static void OnAppEnable_Postfix(AppHomeScreen __instance) {
            var availableCustomApps = __instance.availableHomeScreenApps.Where(x => PhoneAPI.PhoneAppByTypeName.ContainsKey(x.AppName));
            foreach(var app in availableCustomApps) {
                var appInstance = __instance.AppInstance(app.AppName) as CustomApp;
                if (appInstance == null) continue;
                if (appInstance.Available)
                    __instance.AddApp(app);
                else
                    __instance.RemoveApp(app);
            }
        }

        private static void AddApp(RegisteredPhoneApp app, ref List<HomeScreenApp> apps) {
            var appInstance = ScriptableObject.CreateInstance<HomeScreenApp>();
            appInstance.m_AppName = app.AppType.Name;
            appInstance.m_DisplayName = app.Title;
            appInstance.m_AppIcon = app.Icon;
            appInstance.appType = HomeScreenApp.HomeScreenAppType.NONE;
            apps.Add(appInstance);
        }
    }
}
