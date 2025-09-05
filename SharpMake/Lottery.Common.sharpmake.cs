using Sharpmake;

namespace Lottery
{
    public static class Common
    {
        public static readonly Target[] Targets = new[]
        {
            new Target(
                Platform.anycpu,
                DevEnv.vs2022,
                Optimization.Debug | Optimization.Release,
                OutputType.Dll,
                Blob.NoBlob,
                BuildSystem.MSBuild,
                DotNetFramework.v4_7_2
            )
        };
    }
}