#addin "Cake.FileHelpers"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target              = Argument("target", "Default");
var configuration       = Argument("configuration", "Release");
var solutionFolderPath  = "./src/";
var solution            = solutionFolderPath + "SheetToObjects.sln";
var artifactsFolderPath = "./artifacts/";
var buildNumber         = string.IsNullOrWhiteSpace(EnvironmentVariable("APPVEYOR_BUILD_NUMBER")) 
                            ? "0" 
                            : EnvironmentVariable("APPVEYOR_BUILD_NUMBER");
var version             = FileReadText("version.txt");
var commitSha           = string.IsNullOrWhiteSpace(EnvironmentVariable("APPVEYOR_REPO_COMMIT"))
                            ? ""
                            : EnvironmentVariable("APPVEYOR_REPO_COMMIT");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory(solutionFolderPath + "bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifactsFolderPath);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        ArgumentCustomization = args => args.Append("/p:Version=" + version + ";FileVersion=" + version + ";InformationalVersion=" + commitSha),
        Configuration = configuration
    };

    DotNetCoreBuild(solution, settings);
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projects = GetFiles(solutionFolderPath + "*.Specs/*.csproj");
    foreach(var project in projects)
    {
        DotNetCoreTest(
            project.FullPath,
            new DotNetCoreTestSettings()
            {
                Configuration = configuration,
                NoBuild = true
            });
    }
});

Task("Create-Nuget-Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    var packageVersion = version + "-ci" + buildNumber.PadLeft(5, '0');

    var settings = new DotNetCorePackSettings
    {
        ArgumentCustomization = args => args.Append("/p:Version=" + packageVersion),
        Configuration = "Release",
        OutputDirectory = artifactsFolderPath,
        NoBuild = true,
    };

    DotNetCorePack(solutionFolderPath + "/SheetToObjects.Lib/SheetToObjects.Lib.csproj", settings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Create-Nuget-Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
