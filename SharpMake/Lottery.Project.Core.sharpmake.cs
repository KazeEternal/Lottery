using Sharpmake;

[module: Sharpmake.Include("Lottery.Common.sharpmake.cs")]

namespace Lottery
{	
	[Generate]
	public class CoreProject : CSharpProject
	{
		public CoreProject()
		{
			Name = "Lottery Core";
            RootNamespace = "Lottery";
            AssemblyName = "LotteryCore";
			
			AddTargets(
				Lottery.Common.Targets
			);
			
			SourceRootPath = System.IO.Path.GetFullPath(@"[project.SharpmakeCsPath]\..\Source\Core");
		}
		
		[Configure()]
		public void ConfigureAll(Configuration conf, Target target)
		{
			//conf.SolutionFolder = "Source";
			
			conf.Output = Project.Configuration.OutputType.DotNetClassLibrary;

            conf.ReferencesByName.AddRange(
                new Strings(
                    "System",
                    "System.Core",
                    "System.Xml.Linq",
                    "System.Data.DataSetExtensions",
                    "System.Data",
                    "System.Xaml",
                    "System.Xml",
                    "Microsoft.VisualBasic",
                    "Microsoft.CSharp"
                )
            );

            conf.ProjectFileName = "[project.Name]_[target.DevEnv]";
			conf.ProjectPath = @"[project.SharpmakeCsPath]\Source\Core";
			
			conf.TargetPath = @"[project.SourceRootPath]\..\..\Build\Core";
            //conf.Defines.Add("WINDOWS");

            //Need a define for each target platform
            //conf.Defines.Add("WINDOWS");

            //if (target.Optimization == Optimization.Debug)
            //    conf.Options.Add(Options.Vc.Compiler.RuntimeLibrary.MultiThreadedDebugDLL);
            //else
            //    conf.Options.Add(Options.Vc.Compiler.RuntimeLibrary.MultiThreadedDLL);

            //conf.Options.Add(Sharpmake.Options.CSharp.TreatWarningsAsErrors.Enabled);

        }
	}
}