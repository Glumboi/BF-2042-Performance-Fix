using System;
using System.IO;

namespace Battlefield_2042_Performance_fix.Core
{
    public class UserConfig
    {
        private int coreCount = Environment.ProcessorCount; //Thread count of the current processor

        public void CreateUserCfg(BF2042 game, int mode) //0=aggressive, 1=default
        {
            if (mode > 1)
            {
                mode = 1;
            }

            if (!IsConfigExistingAlready(game.GamePath))
            {
                File.WriteAllText(game.GamePath + "\\User.cfg", CfgContent(mode));
                Program.PutDebug("Finished creating User.cfg...");
            }
            else
            {
                Program.PutDebug("A custom User.cfg already exists.");
            }
        }

        private string CfgContent(int mode)
        {
            string content = mode == 1 ?
                "Thread.ProcessorCount " + GetCores() + "\n" +
                "Thread.MaxProcessorCount " + GetThreads() + "\n" +
                "Thread.MinFreeProcessorCount 0\n" +
                "Thread.JobThreadPriority 0" //true
                :
                "Thread.ProcessorCount " + GetCores() + "\n" +
                "Thread.MaxProcessorCount " + GetThreads() + "\n" +
                "Thread.MinFreeProcessorCount 0\n" +
                "Thread.JobThreadPriority 0\n" +
                "GameTime.MaxVariableFps 0\n" +
                "WorldRender.TransparencyShadowmapsEnable 0\n" +
                "WorldRender.MotionBlurEnable 0\n" +
                "WorldRender.MotionBlurQuality 0\n" +
                "WorldRender.MotionBlurMaxSampleCount 0\n" +
                "WorldRender.MotionBlurRadialBlurMax 0\n" +
                "WorldRender.SpotLightShadowmapEnable 0\n" +
                "WorldRender.LightTileCsPathEnable 0\n" +
                "PostProcess.DynamicAOEnable 0\n" +
                "PostProcess.DynamicAOMethod 0\n" +
                "PostProcess.DofMethod 0\n" +
                "PostProcess.BlurMethod 0\n" +
                "PostProcess.HbaoBilateralBlurEnable 0\n" +
                "WorldRender.PlanarReflectionEnable 0 \n" +
                "RenderDevice.VSyncEnable 0\n" +
                "RenderDevice.TripleBufferingEnable 0\n" +
                "RenderDevice.DxDiagDriverDetectionEnable 0\n" +
                "RenderDevice.StereoconvergenceScale 0\n" +
                "RenderDevice.StereoSeparationScale 0\n" +
                "RenderDevice.StereoZoomSoldierConvergenceScale 0\n"; //false

            return content;
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