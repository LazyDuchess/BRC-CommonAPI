using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPI.Phone {
    internal static class InternalPhoneUtility {
        public static List<RegisteredPhoneApp> Apps = null;
        public static Dictionary<string, RegisteredPhoneApp> PhoneAppByTypeFullName = null;

        public static void RegisterAllApps() {
            Apps = [];
            PhoneAppByTypeFullName = [];
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies) {
                var types = assembly.GetTypes();
                foreach(var type in types) {
                    if (typeof(CustomApp).IsAssignableFrom(type)) {
                        var appAttribute = type.GetCustomAttribute<PhoneAppAttribute>();
                        if (appAttribute != null) {
                            RegisterApp(appAttribute, type);
                        }
                    }
                }
            }
        }

        private static void RegisterApp(PhoneAppAttribute metadata, Type appType) {
            var phoneApp = new RegisteredPhoneApp(metadata, appType);
            Apps.Add(phoneApp);
            PhoneAppByTypeFullName[appType.FullName] = phoneApp;
        }
    }
}
