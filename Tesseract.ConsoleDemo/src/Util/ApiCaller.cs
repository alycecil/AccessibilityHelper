using System;
using System.Collections.Generic;
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
            try
            {
                //basePath()
                var x = new UpdatePlayerApi(basePath());

                var playerUpdate = new PlayerUpdate {Name = name, Hp = current, HpMax = max};
                var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
                return resp;
            }
            catch (Exception e)
            {
                handleError(e);
                return null;
            }
        }

        private readonly HashSet<string> set = new HashSet<string>();
        private void handleError(Exception exception)
        {
            var eText = exception.ToString();
            if (set.Contains(eText)) return;
            set.Add(eText);
            Console.Error.WriteLine("Api Issue Encountered, {0}", eText);
        }

        public Player updateMana(string name, int current)
        {
            try
            {
                //basePath()
                var x = new UpdatePlayerApi(basePath());

                var playerUpdate = new PlayerUpdate {Name = name, Mana = current};
                var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
                return resp;
            }
            catch (Exception e)
            {
                handleError(e);
                return null;
            }
        }

        public Player updateWeight(string name, int weight)
        {
            try
            {
                //basePath()
                var x = new UpdatePlayerApi(basePath());

                var playerUpdate = new PlayerUpdate {Name = name, Weight = weight};
                var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
                return resp;
            }
            catch (Exception e)
            {
                handleError(e);
                return null;
            }
        }


        public Player inCombat(string name)
        {
            try
            {
                //basePath()
                var x = new UpdatePlayerApi(basePath());

                var playerUpdate = new PlayerUpdate
                {
                    Name = name, 
                    CombatWindowOpen = true
                };
                var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
                return resp;
            }
            catch (Exception e)
            {
                handleError(e);
                return null;
            }
        }


        public Player outOfCombat(string name)
        {
            try
            {
                //basePath()
                var x = new UpdatePlayerApi(basePath());

                var playerUpdate = new PlayerUpdate
                {
                    Name = name, 
                    ExitedCombat = true
                };
                var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
                return resp;
            }
            catch (Exception e)
            {
                handleError(e);
                return null;
            }
        }


        public Event nextEvent(string name)
        {
            try
            {
                var api = new GetOrdersPlayerApi(basePath());
                return api.EventUsingGET(name);
            }
            catch (Exception e)
            {
                handleError(e);
                return null;
            }
        }

        public string completeEvent(string egoName, Event currentEvent)
        {
            if (currentEvent == null) return null;
            try
            {
                var api = new CompleteOrdersPlayerApi(basePath());
                return api.EventUsingPOST(true, currentEvent.Id, egoName);
            }
            catch (Exception e)
            {
                handleError(e);
                return null;
            }
        }

        public Player login(string name)
        {
            try
            {
                var x = new UpdatePlayerApi(basePath());

                var playerUpdate = new PlayerUpdate {Name = name};
                var resp = x.PlayerUpdateUsingPOST(name, playerUpdate);
                return resp;
            }
            catch (Exception e)
            {
                handleError(e);
                return null;
            }
        }
    }
}