using System;
using System.Text;
using AutoIt;

namespace runner
{
    class ControlLogger
    {
        private static int lenBlock = 157;

        public ControlLogger(IntPtr basehandle, IntPtr chat)
        {
            this.basehandle = basehandle;
            this.chat = chat;
        }

        IntPtr basehandle, chat;
        string lastBlock = null;

        public string LogRoom()
        {
            var sb = new StringBuilder();
            var chatText = AutoItX.ControlGetText(basehandle, chat);
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

                //   Console.WriteLine(">[{0}]", sb.ToString());
            }

            sb.Clear();
            return lastBlock;
        }

        private static void HandleLogs(StringBuilder sb)
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

        public static void HandleWeightLog(string split)
        {
//You are carrying 220 stones in equipment.  Your carrying capacity is 315 stones.  You are 70% encumbered.
            var stonesYouAre = "stones.  You are ";
            int locStart = split.LastIndexOf(stonesYouAre) + stonesYouAre.Length,
                locEnd = split.LastIndexOf('%');
            if (locStart < locEnd)
            {
                var weight = split.Substring(locStart, locEnd - locStart);
                Console.WriteLine("{1} Weight : {0}", weight, DateTime.Now);
                Action.updateWeight(weight);
            }
        }
    }
}