using System.Collections;
using System.Collections.Generic;

namespace runner
{
    public static class Config
    {
        public static readonly string KEY_API_ENDPOINT = "API_ENDPOINT", KEY_ME = "NAME";
        private static bool loaded = false;
        private static Dictionary<string,string> config = new Dictionary<string,string>(); 
        
        private static void load()
        {
            if(loaded) return;
            //TODO READ FILE
            
            config[KEY_API_ENDPOINT] = "http://10.0.0.224:8080";
            config[KEY_ME] = "AliceDjinn";

            loaded = true;
        }



        public static string get(string key)
        {
            load();
            
            string value = null;
            config.TryGetValue(key, out value);
            return value;
        }
    }
}