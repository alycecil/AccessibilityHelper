using System;
using System.Windows.Automation;
using runner.Magic;

namespace runner
{
    public class SpellWindow
    {
        
        
        public void handle(IntPtr baseHandle)
        {
            string spellName = String.Empty;
            SpellType type = SpellType.Auto;
            handle(baseHandle, spellName, type);
        }
        
        private void handle(IntPtr baseHandle, string spellName, SpellType type)
        {
            var spell = Windows.getSpellList(baseHandle);
            if (spell == IntPtr.Zero) return;

            var ae = AutomationElement.FromHandle(spell);
            
            Console.WriteLine("Spells Open");
        }
        
        
    }
}