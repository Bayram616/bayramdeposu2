using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Helpers
{
    public static class ServiceHelper
    {
        public static IServiceProvider Services { get; set; }
        public static T GetService<T>() => Services.GetService<T>();
    }
}
