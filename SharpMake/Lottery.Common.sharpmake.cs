using Sharpmake;

namespace Lottery
{
    public static class Common
    {
        public static Target[] Targets =
        {
            new Target(
                    Platform.anycpu,
                    DevEnv.vs2017,
                    Optimization.Debug | Optimization.Release,
                    OutputType.Dll,
                    Blob.NoBlob,
                    BuildSystem.MSBuild,
                    DotNetFramework.v4_7_2
                )
        };
    }
}