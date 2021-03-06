#tool "nuget:?package=JetBrains.dotCover.CommandLineTools"
#tool "nuget:?package=xunit.runner.console"

var target = Argument("target", "Default");
var buildConfiguration = Argument("configuration", "Release");
var majorMinorVersion = "3.0";
var baseDate = new DateTime(2018,04,06,0,0,0,0,DateTimeKind.Utc);
var elapsedSinceBaseDate = DateTime.UtcNow - baseDate;
var days = (int)elapsedSinceBaseDate.TotalDays;
var revision = (int)(elapsedSinceBaseDate.Subtract(TimeSpan.FromDays(days)).TotalSeconds / 1.5);
var version = majorMinorVersion + "." + days + "." + revision;

Task("Default")
  .IsDependentOn("Pack")
  .Does(() =>
{

});

Task("SetVersion")
  .Does(() =>
{
  TeamCity.SetBuildNumber(version);
  XmlPoke(
    "./src/ServiceStack.Text.EnumMemberSerializer/ServiceStack.Text.EnumMemberSerializer.csproj", 
    "/Project/PropertyGroup/Version",
    version
  );
});

Task("Restore")
  .IsDependentOn("SetVersion")
  .Does(() =>
{
  var restoreSettings = new DotNetCoreRestoreSettings
     {
         Sources = new[] {"https://api.nuget.org/v3/index.json"}
     };
  DotNetCoreRestore("./src/ServiceStack.Text.EnumMemberSerializer.sln", restoreSettings);
});

Task("Build")
  .IsDependentOn("Restore")
  .Does(() => 
{
  var buildSettings = new DotNetCoreBuildSettings
     {
         Configuration = buildConfiguration,
         NoIncremental = true
     };

  DotNetCoreBuild("./src/ServiceStack.Text.EnumMemberSerializer.sln", buildSettings);
});

Task("Test")
  .IsDependentOn("Build")
  .Does(() =>
{
  Action<ICakeContext, string> runTests = (ctx, framework) => { 
    ctx.DotNetCoreTest("./src/ServiceStack.Text.EnumMemberSerializer.UnitTests/ServiceStack.Text.EnumMemberSerializer.UnitTests.csproj", new DotNetCoreTestSettings 
        {
          Configuration = buildConfiguration,
          Framework = framework,
          NoBuild = true
        });
  };

  var coverSettings = new DotCoverCoverSettings()
    .WithFilter("-:*Tests")
    .WithFilter("-:ServiceStack.Text")
    .WithFilter("-:NuGet*")
    .WithFilter("-:xunit*")
    .WithFilter("-:MSBuild*");

  var coverageResult452 = new FilePath("./dotcover/dotcover452.data");
  DotCoverCover(
    ctx => runTests(ctx, "net452"), 
    coverageResult452, 
    coverSettings);

  var coverageResultCoreApp = new FilePath("./dotcover/dotcoverCoreApp.data");
  DotCoverCover(
    ctx => runTests(ctx, "netcoreapp1.1"), 
    coverageResultCoreApp, 
    coverSettings);

  var coverageResultCoreApp20 = new FilePath("./dotcover/dotcoverCoreApp20.data");
  DotCoverCover(
    ctx => runTests(ctx, "netcoreapp2.0"), 
    coverageResultCoreApp20, 
    coverSettings);

  var mergedData = new FilePath("./dotcover/dotcoverMerged.data");

  DotCoverMerge(
    new []{
      coverageResult452,
      coverageResultCoreApp,
      coverageResultCoreApp20
    }, 
    mergedData, 
    new DotCoverMergeSettings());

  if(TeamCity.IsRunningOnTeamCity) {
    TeamCity.ImportDotCoverCoverage(
      mergedData, 
      MakeAbsolute(Directory("./tools/JetBrains.dotCover.CommandLineTools/tools"))
    );
  }
  else {
    var htmlReportFile = new FilePath("./dotcover/dotcover.html");
    var reportSettings = new DotCoverReportSettings { ReportType = DotCoverReportType.HTML};
    DotCoverReport(mergedData, htmlReportFile, reportSettings);
    StartProcess("powershell", "start file:///" + MakeAbsolute(htmlReportFile));
  }
});

Task("Pack")
  .IsDependentOn("Test")
  .Does(() =>
{
  var packSettings = new DotNetCorePackSettings
     {
         Configuration = buildConfiguration,
         OutputDirectory = "./ReleasePackages/",
         ArgumentCustomization = args => args.Append("--include-symbols")
                                             .Append("--include-source")
     };
  DotNetCorePack("./src/ServiceStack.Text.EnumMemberSerializer/ServiceStack.Text.EnumMemberSerializer.csproj", packSettings);
});

RunTarget(target);
