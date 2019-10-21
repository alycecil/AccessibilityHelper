using System.Collections.Generic;

namespace runner
{
    public static partial class Config
    {
        public static readonly string 
            KEY_API_ENDPOINT = "API_ENDPOINT",
            KEY_ME = "NAME",
            KEY_AUTOGUARD = "KEY_AUTO_GUARD";

        private static bool loaded = false;
        private static Dictionary<string, string> config = new Dictionary<string, string>();

        public static string get(string key)
        {
            load();

            config.TryGetValue(key, out var value);
            return value;
        }

        public static bool autoGuard()
        {
            string config = get(KEY_AUTOGUARD);
            if (string.IsNullOrEmpty(config)) return false;
            if ("0".Equals(config) || "false".Equals(config)) return false;

            return true;
        }


        public static string[] getIgnoreList()
        {
            return ignoreList;
        }
    }
}