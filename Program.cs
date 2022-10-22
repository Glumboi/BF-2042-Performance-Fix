using Memory;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battlefield_2042_Performance_fix
{
    internal class Program
    {
        static readonly string[] processNames = { "NVDisplay.Container", "nvcontainer" };
        static readonly Mem _m = new Mem();
        static ProcessPriorityClass newPriority = ProcessPriorityClass.High;

        static void Main(string[] args)
        {
            PutDebug("Start!");

            int PID = _m.GetProcIdFromName("BF2042");
            if (PID > 0)
            {
                RunOptimisations();
            }

            Launch2042();

            Thread.Sleep(20000);

            RunOptimisations();
        }

        static void RunOptimisations()
        {
            List<Process> processes = Processes();
            PutDebug(processes.Count + " processed found");
            foreach (Process proc in processes)
            {
                PutDebug("New process found");
                Console.WriteLine("Changing Priority for id:" + proc.Id + " to " + newPriority.ToString());
                proc.PriorityClass = newPriority;
                PutDebug("Changed priority for " + proc.Id);
            }
            PutDebug("No more processes..");
            PutDebug("Closing...");
            Thread.Sleep(5000);
            /*Console.Write("Press a key, it's over !");
            Console.ReadLine();*/
        }

        static void Launch2042()
        {
            string gameLoc = string.Empty;

            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\EA Games\Battlefield 2042");

            if (key != null)
            {
                gameLoc = key.GetValue("Install Dir").ToString();
            }

            Console.WriteLine("Game path: " + gameLoc);
            Process.Start(gameLoc + @"\BF2042.exe");
        }

        static List<Process> Processes()
        {
            List<Process> rtrnList = new List<Process>();

            foreach (string _p in processNames)
            {
                var processes = Process.GetProcessesByName(_p);
                foreach(Process p in processes)
                {
                    rtrnList.Add(p);
                }
            }

            return rtrnList;
        }

        static bool debug = true;
        static void PutDebug(string info = "")
        {
            if (debug)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("[INFO]: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(info + "\n");
            }
        }
    }
}