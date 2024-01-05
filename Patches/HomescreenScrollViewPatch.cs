using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile.Phone;
using HarmonyLib;
using CommonAPI.Phone;

namespace CommonAPI.Patches {
    [HarmonyPatch(typeof(HomescreenScrollView))]
    internal class HomescreenScrollViewPatch {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(HomescreenScrollView.SetButtonContent))]
        private static bool SetButtonContent_Prefix(HomescreenScrollView __instance, PhoneScrollButton button, int contentIndex) {
            var app = __instance.m_HomeScreen.Apps[contentIndex];
            var appInstance = __instance.m_HomeScreen.AppInstance(app.AppName) as CustomApp;
            if (appInstance == null) return true;

            (button as HomescreenButton).SetContent(app, appInstance.Unread);
            return false;
        }
    }
}
