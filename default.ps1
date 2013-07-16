if((Get-Module | Where-Object {$_.Name -eq "psake"}) -eq $null) 
    { 
        Write-Host "psake module not found, importing it" 
        $scriptPath = Split-Path $MyInvocation.InvocationName 
        Import-Module .\tools\psake.4.2.0.1\tools\psake.psm1
    } 
Import-Module .\tools\SetVersion.psm1

function Get-VersionNumber
{
  $completeVersionNumber = ""

  $buildNumber = $Env:BUILD_NUMBER
  
  if([string]::IsNullOrEmpty($buildNumber))
  {
    $completeVersionNumber = $majorMinorVersion + ".*"
  }
  else
  {
    #running in TeamCity
    $completeVersionNumber = $majorMinorVersion + "." + $buildNumber
  }

  return ,$completeVersionNumber
}

properties {
    $configuration = "Release"
    $rootLocation = get-location
    $srcRoot = "$rootLocation\src"
    $projectBaseName = "ServiceStack.Text.EnumMemberSerializer"
    $csprojFile = "$srcRoot\$projectBaseName\$projectBaseName.csproj"
    $unitTestNamePart = "UnitTests"
    $testDll = "$srcRoot\$projectBaseName.$unitTestNamePart\bin\$configuration\$projectBaseName.$unitTestNamePart.dll"
    $slnFile = "$srcRoot\$projectBaseName.sln"
    $nuspecFile ="$srcRoot\$projectBaseName\$projectBaseName.nuspec"
    $framework = "4.0"
    $xunitRunner = ".\tools\xunit.runners.1.9.1\tools\xunit.console.clr4.exe"
    $nugetOutputDir = ".\ReleasePackages"
    $nugetExe = "$rootLocation\tools\nuget\nuget.exe"
    $versionFile = ".\MajorMinorVersion.txt"
    $majorMinorVersion = Get-Content $versionFile
    $completeVersionNumber = Get-VersionNumber
    $versionSwitch = ""
    
    if(!$completeVersionNumber.EndsWith(".*"))
    {
      #running in TeamCity
      $versionSwitch = "-Version $completeVersionNumber"
      Write-Host "##teamcity[buildNumber '$completeVersionNumber']"
    }
}

task Default -depends Pack

task Clean -depends SetVersion {
  exec { msbuild "$slnFile" /t:Clean /p:Configuration=$configuration }
}

task Compile -depends Clean {
  exec { msbuild "$slnFile" /p:Configuration=$configuration }
}

task Test -depends Compile {
  exec { .$xunitRunner "$testDll" }
}

task SetReleaseNotes -depends Test {
  $releaseNotes = $Env:ReleaseNotes

  if(![string]::IsNullOrEmpty($releaseNotes))
  {
    Write-Host "Setting release notes to:"
    Write-Host "$releaseNotes"

    $nuspecContents = [Xml](Get-Content "$nuspecFile")
    $releaseNotes = $nuspecContents.package.metadata.SelectSingleNode("releaseNotes")
    if($releaseNotes -eq $null)
    {
      $releaseNotes = $nuspecContents.CreateElement('releaseNotes')
      $nuspecContents.package.metadata.AppendChild($releaseNotes)
    }

    $ignore = $releaseNotes.InnerText = $releaseNotes
    $nuspecContents.Save("$nuspecFile")
  }
  else
  {
    Write-Host "No release notes found."
  }
}

task Pack -depends SetReleaseNotes {
  mkdir -p "$nugetOutputDir" -force

  $completeVersionNumber = Get-VersionNumber
  exec { invoke-expression "& '$nugetExe' pack '$csprojFile' -Symbols -Properties Configuration=$configuration -OutputDirectory '$nugetOutputDir' $versionSwitch" }
}

task SetVersion {
  Write-Host "Setting version to $completeVersionNumber"
  Set-Version $completeVersionNumber
}

