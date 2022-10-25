using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battlefield_2042_Performance_fix.Core
{
    public struct BF2042
    {
        public string GamePath{ get; private set; }
        public string SavePath{ get; private set; }

        public BF2042(string gamePath, string savePath)
        {
            GamePath = gamePath;
            SavePath = savePath;
        }

    }
}
