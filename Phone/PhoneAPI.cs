using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommonAPI.Phone {
    public static class PhoneAPI {
        internal static List<RegisteredPhoneApp> Apps = null;
        internal static Dictionary<string, RegisteredPhoneApp> PhoneAppByTypeName = null;
        private static bool Initialized = false;

        internal static void Initialize() {
            if (Initialized) return;
            Initialized = true;
            Apps = [];
            PhoneAppByTypeName = [];
        }

        internal static void RegisterApp(Type appType, string title, Sprite icon = null) {
            if (!Initialized) Initialize();
            var phoneApp = new RegisteredPhoneApp() { AppType = appType, Icon = icon, Title = title };
            Apps.Add(phoneApp);
            PhoneAppByTypeName[appType.Name] = phoneApp;
        }

        public static void RegisterApp<T>(string title, Sprite icon = null) where T : CustomApp {
            RegisterApp(typeof(T), title, icon);
        }
    }
}
