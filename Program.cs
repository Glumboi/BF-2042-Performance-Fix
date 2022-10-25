using Battlefield_2042_Performance_fix.Core;
using Memory;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battlefield_2042_Performance_fix
{
    internal class Program
    {
        static readonly string[] processNames =
        {
            "NVDisplay.Container",
            "nvcontainer"
        };

        static readonly Mem _m = new Mem();
        static ProcessPriorityClass newPriority = ProcessPriorityClass.High;

        static void Main(string[] args)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string subFolderPath = Path.Combine(path, "Battlefield 2042\\settings");
            string gameLoc = string.Empty;

            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\EA Games\Battlefield 2042");

            if (key != null)
            {
                gameLoc = key.GetValue("Install Dir").ToString();
            }

            BF2042 game = new BF2042(gameLoc, subFolderPath);
            UserConfig userConfig = new UserConfig();

            PutDebug("Start!");
            PutDebug("Optimising Config file...");

            while(!ConfigFile.OptimiseSettings(game))
            {

            }
            PutDebug("Creating User.cfg file...");

            userConfig.CreateUserCfg(game);

            PutDebug("Done optimising the Config, starting the game with high process priority...");

            int PID = _m.GetProcIdFromName("BF2042");
            if (PID > 0)
            {
                RunOptimisations();
                return;
            }

            Launch2042(game);

            Thread.Sleep(20000);

            RunOptimisations();
        }

        static void RunOptimisations()
        {
            List<Process> processes = Processes();
            PutDebug(processes.Count + " processes found");
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

        static void Launch2042(BF2042 game)
        {
            

            Console.WriteLine("Game path: " + game.GamePath);
            Process.Start(game.GamePath + @"\BF2042.exe");
        }

        static List<Process> Processes()
        {
            List<Process> rtrnList = new List<Process>();

            foreach (string _p in processNames)
            {
                var processes = Process.GetProcessesByName(_p);
                foreach (Process p in processes)
                {
                    rtrnList.Add(p);
                }
            }

            return rtrnList;
        }

        static bool debug = true;
        public static void PutDebug(string info = "")
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