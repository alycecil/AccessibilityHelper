using System;
using System.Text;
using AutoIt;
using Tesseract.ConsoleDemo;

namespace runner
{
    class SingleControlLogger : ControlLogger
    {
        private static int lenBlock = 157;

        public SingleControlLogger(Program program, IntPtr baseHandle, IntPtr chat) : base(program)
        {
            this.baseHandle = baseHandle;
            this.chat = chat;
        }

        IntPtr baseHandle, chat;
        string lastBlock = null;

        public override string LogRoom()
        {
            try
            {
                var sb = new StringBuilder();
                var chatText = AutoItX.WinGetText(baseHandle).Trim();
                var chatTextLength = chatText.Length;

                if (chatTextLength < lenBlock)
                {
                    //not enough to capture
                    return lastBlock;
                }

                var block = chatText.Substring(chatTextLength - lenBlock, lenBlock);

                if (lastBlock == null)
                {
                    sb.Append(chatText);
                    lastBlock = block;
                }
                else
                {
                    var newAfter = chatText.LastIndexOf(lastBlock);

                    if (newAfter < 0)
                    {
                        //all new
                        sb.Append(chatText);
                        lastBlock = block;
                    }
                    else if (newAfter + lenBlock < chatTextLength)
                    {
                        sb.Append(chatText.Substring(newAfter + lenBlock));
                        lastBlock = block;
                    }
                    else
                    {
                        //Console.WriteLine("<No new logs.");
                    }
                }


                //TODO Flush to somewheere
                if (sb.Length > 0)
                {
                    HandleLogs(sb);

                    // Console.WriteLine(">[{0}]", sb.ToString());
                }

                sb.Clear();
                return lastBlock;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Had an oopsie with a control logger: {0}", e);
                lastBlock = null;
                return null;
            }
        }

        private void HandleLogs(StringBuilder sb)
        {
            var raw = sb.ToString();
            var splits = raw.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


            foreach (var split in splits)
            {
                if (split.StartsWith("Info> You are carrying"))
                {
                    HandleWeightLog(split);
                }
            }
        }

        private void HandleWeightLog(string split)
        {
//You are carrying 220 stones in equipment.  Your carrying capacity is 315 stones.  You are 70% encumbered.
            var stonesYouAre = "stones.  You are ";
            int locStart = split.LastIndexOf(stonesYouAre) + stonesYouAre.Length,
                locEnd = split.LastIndexOf('%');
            if (locStart < locEnd)
            {
                var weight = split.Substring(locStart, locEnd - locStart);
                Console.WriteLine("{1} Weight : {0}", weight, DateTime.Now);
                program.action.UpdateWeight(weight);
            }
        }
    }
}