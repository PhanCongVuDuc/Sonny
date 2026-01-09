using Nuke.Common.Tools.DotNet ;
using static Nuke.Common.Tools.DotNet.DotNetTasks ;

namespace Build ;

sealed partial class Build
{
    /// <summary>
    ///     Compile all solution configurations.
    /// </summary>
    Target Compile =>
        _ => _
            .DependsOn(Clean)
            .Executes(() =>
            {
                foreach (var configuration in GlobBuildConfigurations()) {
                    // Exclude Test projects from build
                    var projectsToBuild = Solution.AllProjects
                        .Where(project => ! project.Name.Contains("Tests", StringComparison.OrdinalIgnoreCase))
                        .ToList() ;

                    foreach (var project in projectsToBuild) {
                        DotNetBuild(settings => settings
                            .SetProjectFile(project)
                            .SetConfiguration(configuration)
                            .SetVersion(ReleaseVersionNumber)
                            .SetVerbosity(DotNetVerbosity.minimal)) ;
                    }
                }
            }) ;
}
