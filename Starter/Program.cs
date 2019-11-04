using System;
using System.Diagnostics;

namespace Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            string name;

#if DEBUG
            name = "notepad.exe";
#else
            name = "Tesseract.ConsoleDemo.exe";
#endif
            if (args.Length > 0)
            {
                name = args[0];
            }

            string guid = Guid.NewGuid().ToString();
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = name,
                Arguments = guid,
                UseShellExecute = false
            };


            while (true)
            {
                var p = Process.Start(startInfo);
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    Console.WriteLine("Error encountered, restarting child shortly.");
                }
                else
                {
                    Console.WriteLine("Soft Shutdown.");
                    return;
                }
            }
        }
    }
}