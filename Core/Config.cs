using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Battlefield_2042_Performance_fix.Core
{
    public static class ConfigFile
    {
        public static string[] Content { get; private set; }
        public static List<string> ReplacedStrings { get; private set; }

        static List<string> replacedStrings = new List<string>();

        static readonly string[] linesForOptimisation =
        {
            "GstRender.ShadowQuality",      //0
            "GstRender.TransparentShadows", //1
            "GstRender.WeaponDOF",          //2
            "GstRender.ShaderQuality",      //3
            "GstRender.Enlighten"           //4
        };

        private static string[] GetConfigContent(BF2042 config)
        {
            var content = File.ReadAllLines(config.SavePath + "\\PROFSAVE_profile");
            Content = content;
            return content;
        }

        public static bool OptimiseSettings(BF2042 config)
        {
            string[] content = GetConfigContent(config);

            var writer = new StreamWriter(config.SavePath + "\\PROFSAVE_profile");
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];

                foreach (string str in content)
                {
                    foreach (string str2 in linesForOptimisation)
                    {
                        if (str.Contains(str2) && str.Contains(" 2") || str.Contains(str2) && str.Contains(" 1"))
                        {
                            line = line.Replace(str, str2 + " 0");
                        }
                    }
                }
                writer.WriteLine(line);
            }

            writer.Close();
            return true;
        }
    }
}
