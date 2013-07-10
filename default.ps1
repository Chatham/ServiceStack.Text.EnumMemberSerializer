if((Get-Module | Where-Object {$_.Name -eq "psake"}) -eq $null) 
    { 
        Write-Host "psake module not found, importing it" 
        $scriptPath = Split-Path $MyInvocation.InvocationName 
        Import-Module .\tools\psake.4.2.0.1\tools\psake.psm1
    } 
Import-Module .\tools\SetVersion.psm1
properties {
    $configuration = "Release"
    $rootLocation = get-location
    $srcRoot = "$rootLocation\src"
    $projectBaseName = "ServiceStack.Text.EnumMemberSerializer"
    $csprojFile = "$srcRoot\$projectBaseName\$projectBaseName.csproj"
    $unitTestNamePart = "UnitTests"
    $testDll = "$srcRoot\$projectBaseName.$unitTestNamePart\bin\$configuration\$projectBaseName.$unitTestNamePart.dll"
    $slnFile = "$srcRoot\$projectBaseName.sln"
    $framework = "4.0"
    $xunitRunner = ".\tools\xunit.runners.1.9.1\tools\xunit.console.clr4.exe"
    $nugetOutputDir = ".\ReleasePackages"
    $nugetExe = "$rootLocation\tools\nuget\nuget.exe"
    $versionFile = ".\MajorMinorVersion.txt"
}

task Default -depends Pack

task Clean -depends SetVersion {
  msbuild "$slnFile" /t:Clean /p:Configuration=$configuration
}

task Compile -depends Clean {
  msbuild "$slnFile" /p:Configuration=$configuration 
}

task Test -depends Compile {
  .$xunitRunner "$testDll"
}

task Pack -depends Test {
  mkdir -p "$nugetOutputDir" -force
  invoke-expression "& '$nugetExe' pack '$csprojFile' -Symbols -Properties Configuration=$configuration -OutputDirectory '$nugetOutputDir'"
}

task SetVersion {
  $majorMinorVersion = Get-Content $versionFile

  $buildNumber = $Env:BUILD_NUMBER
  
  if([string]::IsNullOrEmpty($buildNumber))
  {
    $completeVersionNumber = $majorMinorVersion + ".*"
  }
  else
  {
    #running in TeamCity
    $completeVersionNumber = $majorMinorVersion + "." + $buildNumber
    Write-Host "##teamcity[buildNumber '$completeVersionNumber']"
  }
  
  write-host "Setting version to $completeVersionNumber"
  
  Set-Version $completeVersionNumber
}

