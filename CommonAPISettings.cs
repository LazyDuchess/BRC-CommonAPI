using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPI
{
    internal class CommonAPISettings
    {
#if COMMONAPI_DEBUG
        public static bool Debug = true;
#else
        public static bool Debug = false;
#endif
    }
}
