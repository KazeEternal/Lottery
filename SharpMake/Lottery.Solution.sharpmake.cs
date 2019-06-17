using Sharpmake;

[module: Sharpmake.Include("Lottery.Project.Core.sharpmake.cs")]
[module: Sharpmake.Include("Lottery.Project.CLI.sharpmake.cs")]
[module: Sharpmake.Include("Lottery.Project.GUI.sharpmake.cs")]
[module: Sharpmake.Include("Lottery.Common.sharpmake.cs")]

namespace Lottery
{
	[Generate]
    public class LotteryGenerator
    {
        [Main]
		public static void SharpmakeMain(Arguments sharpmakeArgs)
		{
            // Tells Sharpmake to generate the solution described by
            // BasicsSolution.
            sharpmakeArgs.Generate<LotterySolution>();
		}
    }

	[Generate]
	public class LotterySolution : Solution
	{
		public LotterySolution()//: base(Target.
		{
			Name = "Raffle Bot";
            AddTargets(Lottery.Common.Targets);
        }
		
		[Configure]
        public void ConfigureAll(Configuration conf, Sharpmake.Target target)
		{
            
			conf.SolutionFileName = "[solution.Name]_[target.DevEnv]";
			conf.SolutionPath = @"[solution.SharpmakeCsPath]\..\";
			conf.AddProject<CoreProject>(target);
			conf.AddProject<WPFProject>(target);
			conf.AddProject<CLIProject>(target);
			conf.Options.Add(Options.Vc.Compiler.Exceptions.Enable);
		}
	}
}