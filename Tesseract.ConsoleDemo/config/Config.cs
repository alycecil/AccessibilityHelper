using System.Collections.Generic;

namespace runner
{
    public static class Config
    {
        public static readonly string KEY_API_ENDPOINT = "API_ENDPOINT",
            KEY_ME = "NAME",
            KEY_AUTOGUARD = "KEY_AUTO_GUARD";

        private static bool loaded = false;
        private static Dictionary<string, string> config = new Dictionary<string, string>();

        private static void load()
        {
            if (loaded) return;
            //TODO READ FILE

            config[KEY_API_ENDPOINT] = "http://10.0.0.224:8080";
            config[KEY_ME] = "AliceDjinn";
            config[KEY_AUTOGUARD] = "true";
            loaded = true;
        }


        public static string get(string key)
        {
            load();

            string value = null;
            config.TryGetValue(key, out value);
            return value;
        }

        public static bool autoGuard()
        {
            string config = get(KEY_AUTOGUARD);
            if (string.IsNullOrEmpty(config)) return false;
            if ("0".Equals(config) || "false".Equals(config)) return false;

            return true;
        }


        private static string[] ignoreList = new string[]
        {
            "Mug", "Plate", "Finger", "Pebble", "Candle", "Hammer", "Jet", "Leather Pouch", "Tobac Leaf",
            "Elder Oak Leaf", "Bowl", "Basalt Rock", "Zombie Finger", "Tooth", "Clump of Grass", "Jaw Bone", "Pigweed",


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
            "Bottle of Red Dye",
            "Bottle of Olive Dye",
            "Bottle of Magenta Dye",
            "Bottle of Tan Dye",
            
            "Steel Broad Sword",
            "Steel Chain Vest",
            "Steel Plate Armor",
            "Steel Axe",
            "Steel Long Sword",
            "Steel Short Sword",
            "Steel Dagger",
            "Steel Throwing Dagger",
            "Steel Club",
            "Steel Cowl",
            "Steel Claw",
            "Steel Maul",
            "Steel Round Shield",
            "Steel Tower Shield",
            "Steel Two-Handed Sword",
            "Steel Wristbands",

            "Iron Broad Sword",
            "Iron Chain Vest",
            "Iron Plate Armor",
            "Iron Axe",
            "Iron Long Sword",
            "Iron Short Sword",
            "Iron Dagger",
            "Iron Throwing Dagger",
            "Iron Club",
            "Iron Cowl",
            "Iron Claw",
            "Iron Maul",
            "Iron Round Shield",
            "Iron Tower Shield",
            "Iron Two-Handed Sword",
            "Iron Wristbands",
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
                "Bauble", "125",
            },
            new[]
            {
                "Clothing", "2500",
            },
            new[]
            {
                "Clothing", "2502",
            },
            new[]
            {
                "Belt", "7",
            },
        };

        private static string[] simple = new string[]
        {
            "Admantium Broad Sword",
            "Admantium Chain Vest",
            "Admantium Plate Armor",
            "Admantium Axe",
            "Admantium Long Sword",
            "Admantium Short Sword",
            "Admantium Dagger",
            "Admantium Throwing Dagger",
            "Admantium Club",
            "Admantium Cowl",
            "Admantium Claw",
            "Admantium Maul",
            "Admantium Round Shield",
            "Admantium Tower Shield",
            "Admantium Two-Handed Sword",
            "Admantium Wristbands",
            
            "Mythril Broad Sword",
            "Mythril Chain Vest",
            "Mythril Plate Armor",
            "Mythril Axe",
            "Mythril Long Sword",
            "Mythril Short Sword",
            "Mythril Dagger",
            "Mythril Throwing Dagger",
            "Mythril Club",
            "Mythril Cowl",
            "Mythril Claw",
            "Mythril Maul",
            "Mythril Round Shield",
            "Mythril Tower Shield",
            "Mythril Two-Handed Sword",
            "Mythril Wristbands",
            
            "Magical Long Sword",
            
            "Long Leather Skirt",
            "Leather Armor",
            "Leather Pants",


            "Amber Rod",
            "Aquamarine",
            "Basalt Rock",
            "Belt of Weakness",
            "Blue Rose",

            "Bag of Obsidian Dust",
            "Bowl",
            "Cowl",
            "Leather Tunic",
            "Candle",
            "Chain Cowl",
            "Chip of Marble",
            "Chrysanthemum",
            "Corbalite Rock",
            "Clump of Grass",
            "Crystal",
            "Daemon Guano",
            "Daemon Tooth",
            "Devilweed",
            "Diamond",
            "Drum",
            "Elder Oak Leaf",
            "Elixir",
            "Emerald",
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
            "Ogre Eye",
            "Eye of Kilrog",
            "Pebble",
            "Plate",
            "Potion",
            "Plate Armor",
            "Platemail Greaves",
            "Rat Guano",
            "Red Rose",
            "Ruby",
            "Ruby Chip",
            "Scroll",
            "Silverthorn Leaf",
            "Strynx",
            "Silvergrass",
            "Sulfur Rock",
            "Sollerets",
            "Tobac Leaf",
            "Topaz",
            "Turquoise",
            "Statue",
            "Carving",
            "Wand",
            "White Daisy",
            "White Rose",
            "Wristband",
            "Zombie Finger",


            "Picaro's Alacrity: Coins of the Dead",
            "Magic Shield",
            "Journal 014",
            "*Trash"
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