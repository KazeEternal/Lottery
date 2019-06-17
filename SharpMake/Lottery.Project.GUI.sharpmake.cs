using Sharpmake;

[module: Sharpmake.Include("Lottery.Common.sharpmake.cs")]

namespace Lottery
{	
	[Generate]
	public class WPFProject : CSharpProject
	{
		public WPFProject()
		{
			Name = "WPF_GUI";
            RootNamespace = "WPF_GUI";
            AssemblyName = "LotteryGUI";
			
            ProjectTypeGuids = CSharpProjectType.Wpf;

            //SourceFilesIncludeRegex.Add(@".*\.png.*");
            //ResourceFiles.Add(@"[project.SharpmakeCsPath]\..\Resources");
            NoneExtensionsCopyIfNewer.Add(@".png");
            NoneExtensionsCopyIfNewer.Add(@".m4a");
            NoneExtensionsCopyIfNewer.Add(@".wav");

			AddTargets(
				Lottery.Common.Targets
			);
			
			SourceRootPath = @"[project.SharpmakeCsPath]\..\Source\GUI";
		}
		
		[Configure()]
		public void ConfigureAll(Configuration conf, Target target)
		{
			//conf.SolutionFolder = "Source";
			
			conf.Output = Configuration.OutputType.DotNetWindowsApp;

            conf.ReferencesByName.AddRange(
                new Strings(
                    "System",
                    "System.Core",
                    "System.Xml.Linq",
                    "System.Data.DataSetExtensions",
                    "System.Data",
                    "System.Xaml",
                    "System.Xml",
                    "PresentationCore",
                    "PresentationFramework",
                    "WindowsBase"
                )
            );

            conf.ProjectFileName = "[project.Name]_[target.DevEnv]";
			conf.ProjectPath = @"[project.SharpmakeCsPath]\Source\GUI";
			
			conf.TargetPath = @"[project.SourceRootPath]\..\..\Build\GUI";

            conf.AddPrivateDependency<CoreProject>(target);

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