using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Series_Tracker.Resources
{
    public class PubVars
    {
        public static string ApplicationName =>
            Assembly.GetExecutingAssembly()
            .GetName().Name ?? "Series Tracker";

        public static string Version {
            get
            { var info = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;
                if (string.IsNullOrWhiteSpace(info))
                    return "Unknown";

                var plusIndex = info.IndexOf('+');
                return plusIndex > 0 ? info[..plusIndex] : info;
            }
        }

        public static string BuildDate =>
        System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location)
        .ToString("yyyy-MM-dd");

        public static string OSVersion =>
        System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        public static bool IsNotSingleton { get; set; } = false;
    }
}
    
