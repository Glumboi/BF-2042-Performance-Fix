using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battlefield_2042_Performance_fix.Core
{
    public class UserConfig
    {
        int coreCount = Environment.ProcessorCount; //Thread count of the current processor

        public void CreateUserCfg(BF2042 game)
        {
            if (!IsConfigExistingAlready(game.GamePath))
            {
                File.WriteAllText(game.GamePath + "\\User.cfg", CfgContent());
                Program.PutDebug("Finished creating User.cfg...");
            }
            else
            {
                Program.PutDebug("A custom User.cfg already exists.");
            }
        }

        private string CfgContent()
        {
            return "Thread.ProcessorCount " + GetCores() + "\n" +
                "Thread.MaxProcessorCount " + GetThreads() + "\n" +
                "Thread.MinFreeProcessorCount 0\n" +
                "Thread.JobThreadPriority 0";
        }

        private int GetCores()
        {
            return coreCount / 2;
        }

        private int GetThreads()
        {
            if (coreCount > 15)
            {
                return 15;
            }
            return coreCount--;
        }

        private bool IsConfigExistingAlready(string path)
        {
            if (File.Exists(path + "\\User.cfg"))
            {
                return true;
            }
            return false;
        }
    }
}
