using System.Collections;
using System.Collections.Generic;

namespace runner
{
    public static class Config
    {
        public static readonly string KEY_API_ENDPOINT = "API_ENDPOINT", KEY_ME = "NAME";
        private static bool loaded = false;
        private static Dictionary<string, string> config = new Dictionary<string, string>();

        private static void load()
        {
            if (loaded) return;
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


        private static string[] ignoreList = new string[]
        {
            "Mug", "Plate", "Finger", "Pebble", "Candle", "Hammer", "Jet", "Leather Pouch", "Tobac Leaf",
            "Elder Oak Leaf", "Bowl", "Basalt Rock", "Zombie Finger", "Tooth", "Clump of Grass", "Jaw Bone", "Pigweed"
        };

        public static string[] getIgnoreList()
        {
            return ignoreList;
        }

        private static string[][] costMatch = new string[][]
        {
            new[]
            {
                "Backpack", "20"
            },
            new[]
            {
                "Bauble", "75",
            },
            new[]
            {
                "Bauble", "100",
            },
            new[]
            {
                "Clothing", "2500",
            },
        };

        private static string[] simple = new string[]
        {
            "Admantium Broad Sword",
            "Admantium Chain Vest",
            "Admantium Axe",
            "Admantium Long Sword",
            "Admantium Short Sword",
            "Admantium Dagger",
            "Admantium Throwing Dagger",
            "Admantium Club",
            "Admantium Cowl",
            "Amber Rod",
            "Aquamarine",
            "Basalt Rock",
            "Belt of Weakness",
            "Blue Rose",
            "Bottle of Amber Dye",
            "Bottle of Aqua Dye",
            "Bottle of Azure Dye",
            "Bottle of Black Dye",
            "Bottle of Blue Dye",
            "Bottle of Brown Dye",
            "Bottle of Gold Dye",
            "Bottle of Gray Dye",
            "Bottle of Jade Dye",
            "Bottle of Lime Dye",
            "Bottle of Pink Dye",
            "Bottle of Green Dye",
            "Bottle of Purple Dye",
            "Bottle of Royal Purple Dye",
            "Bottle of Violet Dye",
            "Bottle of Orange Dye",
            "Bottle of Yellow Dye",
            "Bottle of Teal Dye",
            "Bottle of White Dye",
            "Bowl",
            "Cowl",
            "Leather Tunic",
            "Candle",
            "Chain Cowl",
            "Chip of Marble",
            "Chrysanthemum",
            "Clump of Grass",
            "Crystal",
            "Daemon Guano",
            "Daemon Tooth",
            "Devilweed",
            "Drum",
            "Elder Oak Leaf",
            "Elixir",
            "Finger",
            "Flute",
            "Gold Nugget",
            "Goldenberries",
            "Hammer",
            "Imp Guano",
            "Jaw Bone",
            "Jet",
            "Leather Pouch",
            "Lord Ulric, the Valiant",
            "Lute",
            "Lyre",
            "Mirror",
            "Mug",
            "Mythril Helmet",
            "Oak Leaf",
            "Orb",
            "Pebble",
            "Plate",
            "Potion",
            "Plate Armor",
            "Rat Guano",
            "Red Rose",
            "Ring",
            "Ruby Chip",
            "Scroll",
            "Silverthorn Leaf",
            "Strynx",
            "Silvergrass",
            "Sulfur Rock",
            "Tobac Leaf",
            "Topaz",
            "Turquoise",
            "Statue",
            "Carving",
            "Wand",
            "White Daisy",
            "White Rose",
            "Zombie Finger"
        };

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