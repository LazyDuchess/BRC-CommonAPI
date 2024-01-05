using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPI.Phone {
    internal class RegisteredPhoneApp {
        public PhoneAppAttribute Metadata;
        public Type AppType;

        public RegisteredPhoneApp(PhoneAppAttribute metadata, Type appType) {
            Metadata = metadata;
            AppType = appType;
        }
    }
}
