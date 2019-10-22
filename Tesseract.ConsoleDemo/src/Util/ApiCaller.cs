using System.Runtime.InteropServices;
using IO.Swagger.Api;
using IO.Swagger.Model;
using RestSharp;

namespace runner
{
    public class ApiCaller
    {

        public ApiCaller()
        {
        }

        private string basePath()
        {
            return Config.get(Config.KEY_API_ENDPOINT);
        }
        private RestClient get()
        {
            
            //todo read from file
            var client = new RestClient();
            return client;

        }
        
        
        
        public Player updateHp(string name, int current, int max)
        {
            //basePath()
            var x = new UpdatePlayerApi(basePath());

            var playerUpdate = new PlayerUpdate();
            playerUpdate.Name = name;
            playerUpdate.Hp = current;
            playerUpdate.HpMax = max;
            var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
            return resp;
        }
        
        public Player updateMana(string name, int current)
        {
            //basePath()
            var x = new UpdatePlayerApi(basePath());

            var playerUpdate = new PlayerUpdate();
            playerUpdate.Name = name;
            playerUpdate.Mana = current;
            var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
            return resp;
        }

        public Player updateWeight(string name, int weight)
        {
            //basePath()
            var x = new UpdatePlayerApi(basePath());

            var playerUpdate = new PlayerUpdate();
            playerUpdate.Name = name;
            playerUpdate.Weight = weight;
            var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
            return resp;
        }
        
        
        public Player inCombat(string name)
        {
            //basePath()
            var x = new UpdatePlayerApi(basePath());

            var playerUpdate = new PlayerUpdate();
            playerUpdate.Name = name;
            playerUpdate.CombatWindowOpen = true;
            var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
            return resp;
        }
        
        
        public Player outOfCombat(string name)
        {
            //basePath()
            var x = new UpdatePlayerApi(basePath());

            var playerUpdate = new PlayerUpdate();
            playerUpdate.Name = name;
            playerUpdate.ExitedCombat = true;
            var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
            return resp;
        }



        public Event nextEvent(string name)
        {
            //try {
                var api = new GetOrdersPlayerApi(basePath());
                return api.EventUsingGET(name);
            //}ca
        }

        public string completeEvent(string egoName, Event currentEvent)
        {
            var api = new CompleteOrdersPlayerApi(basePath());
            return api.EventUsingPOST(true, currentEvent.Id, egoName);
        }

        public Player login(string name)
        {
            var x = new UpdatePlayerApi(basePath());

            var playerUpdate = new PlayerUpdate();
            playerUpdate.Name = name;
            var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
            return resp;
        }
    }
}