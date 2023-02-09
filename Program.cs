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
        #region localVariables

        private static readonly string[] processNames =
        {
            "NVDisplay.Container",
            "nvcontainer"
        };

        private static readonly Mem _m = new Mem();
        private static ProcessPriorityClass newPriority = ProcessPriorityClass.High;

        #endregion localVariables

        private static void Main(string[] args)
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

            while (!ConfigFile.OptimiseSettings(game))
            {
            }

            #region user.cfg optimisations

            PutDebug("Creating User.cfg file...");

        createCfg:

            PutDebug("Please enter the mode of the User.cfg that you want to use (0=aggressive, 1=default)\n" +
                "Aggressive provides a bigger boost while making the game look slightly darker-ish." +
                "\nDefault will apply the default thread management tweaks.");

            int configMode;
            try
            {
                configMode = Int32.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                PutError("Please enter a number");
                goto createCfg;
            }

            userConfig.CreateUserCfg(game, configMode);
            PutDebug("Done optimising the Config, starting the game with high process priority...");

            #endregion user.cfg optimisations

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

        private static void RunOptimisations()
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
        }

        private static void Launch2042(BF2042 game)
        {
            Console.WriteLine("Game path: " + game.GamePath);
            Process.Start(game.GamePath + @"\BF2042.exe");
        }

        private static List<Process> Processes()
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

        private static bool debug = true;

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

        public static void PutError(string info = "")
        {
            if (debug)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[ERROR]: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(info + "\n");
            }
        }
    }
}