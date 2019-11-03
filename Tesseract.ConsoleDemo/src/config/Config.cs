using System;
using System.Collections.Generic;
using System.Threading;
using AutoIt;

namespace runner
{
    public static partial class Config
    {
        public static readonly string 
            KEY_API_ENDPOINT = "API_ENDPOINT",
            KEY_SCREENSCAN = "SCREENSCAN",
            KEY_AUTOGUARD = "KEY_AUTO_GUARD",
            KEY_TELEPORT = "KEY_TELEPORT..";

        private static bool loaded = false;
        private static Dictionary<string, string> config = new Dictionary<string, string>();

        public static string get(string key)
        {
            load();

            if(config.TryGetValue(key, out var value))
                return value;

            return null;
        }
        
        private static bool getBoolean(string key)
        {
            string config = get(key);
            if (string.IsNullOrEmpty(config)) return false;
            if ("0".Equals(config) || "false".Equals(config)) return false;

            return true;
        }
        
        private static bool getAsInt(string key, out int val)
        {
            val = 0;
            string config = get(key);
            if (string.IsNullOrEmpty(config)) return false;
            return int.TryParse(config, out val);
        }

        public static bool autoGuard()
        {
            string keyAutoguard = KEY_AUTOGUARD;
            return getBoolean(keyAutoguard);
        }

        public static bool screenScan()
        {
            string keyAutoguard = KEY_SCREENSCAN;
            return getBoolean(keyAutoguard);
        }


        public static string[] getIgnoreList()
        {
            return ignoreList;
        }

        public static bool click(IntPtr baseHandle, string keyX, string keyY)
        {
            var foundX = getAsInt(keyX, out int x);
            var foundY = getAsInt(keyY, out int y);
            if (foundX && foundY)
            {
                MouseManager.MouseClick(baseHandle,x,y);
                return true;
            }

            return false;

        }
    }
}