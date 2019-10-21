using System.Collections.Generic;

namespace runner
{
    public static partial class Config
    {
        
        public static List<Sellable> getSellableList()
        {
            List<Sellable> s = new List<Sellable>();


            foreach (var s1 in ignoreList)
            {
                s.Add(new Sellable(s1));
            }

            foreach (var s1 in simple)
            {
                s.Add(new Sellable(s1));
            }

            foreach (var s1 in costMatch)
            {
                s.Add(new Sellable(s1[0], s1[1]));
            }

            return s;
        }
    }
    
    public class Sellable
    {
        public Sellable(string name)
        {
            this.name = name;
        }

        public Sellable(string name, string cost)
        {
            this.name = name;
            this.cost = cost;
        }

        public string name;
        private string cost;

        public bool costMatch(string cost)
        {
            if (this.cost == null)
            {
                return true;
            }

            return cost.Equals(this.cost);
        }
    }
}