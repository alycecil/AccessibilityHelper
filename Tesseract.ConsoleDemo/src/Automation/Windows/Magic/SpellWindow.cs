using System;
using System.Threading;
using System.Windows.Automation;
using AutoIt;
using IO.Swagger.Model;
using runner.Magic;
using Tesseract;
using Tesseract.ConsoleDemo;

namespace runner
{
    public class SpellWindow
    {
        string spellName;
        private SpellType type;

        public SpellWindow(string spellName, SpellType type)
        {
            this.spellName = spellName;
            this.type = type;
        }

        public bool handle(IntPtr baseHandle, Program program, Event param)
        {
            return __handle(program, baseHandle, spellName, type, param);
        }

        private static bool __handle(Program program, IntPtr baseHandle, string spellName, SpellType type, Event curEvent)
        {
            if (!TryGetWindow(program, baseHandle, out IntPtr spell)) return false;

            AutoItX.WinActivate(spell);
            var ae = AutomationElement.FromHandle(spell);

            Console.WriteLine("Spells Open");

            TreeWalker walker = TreeWalker.ControlViewWalker;
            AutomationElement favorites = walker.GetFirstChild(ae);


            if (CastSpell(baseHandle, walker, favorites, spellName, type, curEvent))
            {
                switch (type)
                {
                    case SpellType.Auto:
                        return true;
                    case SpellType.Teleport:    
                        return Teleport.teleport(baseHandle, curEvent);
                    
                    default:
                        throw new NotImplementedException();
                }
            }

            return false;

            //maybe change schools
//            AutomationElement currentSchool = walker.GetLastChild(ae);
        }


        private static bool CastSpell(IntPtr baseHandle, TreeWalker walker, AutomationElement list,
            string spellName, SpellType type, Event curEvent)
        {
            var spell = walker.GetFirstChild(list);
            if (spell == null)
            {
                return false;
            }

            try
            {
                if (!spell.TryGetClickablePoint(out var locBase)) return false;

                int count = 0;
                while (spell != null)
                {
                    WindowHandleInfo.ConvertRect(out var rect, spell.Current.BoundingRectangle);
                    if (!rect.IsEmpty
                        && spell.TryGetClickablePoint(out var loc2)
                        && whatWeAreLookngFor(spell, walker, spellName, type, curEvent)
                    )
                    {
                        //todo click

                        WindowHandleInfo.GetScale(baseHandle, out float sX, out float sY);


                        MouseManager.MouseClickAbsolute(baseHandle,MouseButton.RIGHT, (int) locBase.X, (int) (locBase.Y + count * rect.Height * sY));
                        return true;
                    }


                    spell = walker.GetNextSibling(spell);
                    count++;
                }
            }
            catch (Exception)
            {
                //IGNORE
            }

            return false;
        }


        private static bool whatWeAreLookngFor(AutomationElement spell, TreeWalker walker,
            string spellName,
            SpellType type,
            Event curEvent)
        {
            //Console.WriteLine(spell.Current.Name);
            var name = spell.Current.Name;
            string price;
            var cost = walker.GetLastChild(spell);
            price = cost?.Current.Name;
            if (price == null) return false;

            if (name.Equals(spellName))
            {
                return true;
            }


            return false;
        }

        private static bool TryGetWindow(Program program, IntPtr baseHandle, out IntPtr spell)
        {
            if (__TryGetWindow(baseHandle, out spell)) return true;


            if (!program.stateEngine.InState(StateEngine.OutOfCombat)) return false;
            
            Teleport.close(baseHandle);

            ToolTips.moveOver(baseHandle,ExpectedToolTip.Spells);
            Thread.Sleep(1);
            MouseManager.MouseClickAbsolute(baseHandle);
            if (__TryGetWindow(baseHandle, out spell)) return true;

            //anyother look ups here

            return false;
        }

        private static bool __TryGetWindow(IntPtr baseHandle, out IntPtr spell)
        {
            spell = Windows.getSpellList(baseHandle);
            return spell != IntPtr.Zero;
        }
    }
}