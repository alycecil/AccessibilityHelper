using System;

namespace runner.Magic
{
    public enum SpellType
    {
        Teleport,
        Auto,
        TargetGood,
        TargetBad
    }
    public class Spell
    {
        public String name, cost;
    }
}