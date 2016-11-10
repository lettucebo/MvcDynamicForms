Param (
  [switch]$Publish
)
 
$ErrorActionPreference = "Stop" 
$global:ExitCode = 1 
 
function Write-Log { 
 
  #region Parameters 
   
    [cmdletbinding()]
    Param(
      [Parameter(ValueFromPipeline=$true)]
      [array] $Messages,

      [Parameter()] [ValidateSet("Error", "Warn", "Info")]
      [string] $Level = "Info",

      [Parameter()]
      [Switch] $NoConsoleOut = $false,

      [Parameter()]
      [String] $ForegroundColor = 'White',

      [Parameter()] [ValidateRange(1,30)]
      [Int16] $Indent = 0,

      [Parameter()]
      [IO.FileInfo] $Path = ".\NuGet.log",

      [Parameter()]
      [Switch] $Clobber,

      [Parameter()]
      [String] $EventLogName,

      [Parameter()]
      [String] $EventSource,

      [Parameter()]
      [Int32] $EventID = 1

    )

  #endregion

Begin {} 
 
  Process { 
     
    $ErrorActionPreference = "Continue" 
 
    if ($Messages.Length -gt 0) { 
      try {       
        foreach($m in $Messages) {       
          if ($NoConsoleOut -eq $false) { 
            switch ($Level) { 
              'Error' {  
                Write-Error $m -ErrorAction SilentlyContinue
                Write-Host('{0}{1}' -f (" " * $Indent), $m) -ForegroundColor Red
              } 
              'Warn' {  
                Write-Warning $m  
              } 
              'Info' {  
                Write-Host('{0}{1}' -f (" " * $Indent), $m) -ForegroundColor $ForegroundColor 
              } 
            } 
          } 
 
          if ($m.Trim().Length -gt 0) { 
            $msg = '{0}{1} [{2}] : {3}' -f(" " * $Indent), (Get-Date -Format "yyyy-MM-dd HH:mm:ss"), $Level.ToUpper(), $m

feat: add nuget project

紙鈔魚
authored3/28/2016 @ 12:10 AM
commit:365bf3
parent:a14aaa
1 modified
10 added
Name
Full Path
Creatidea.Library.Web.DynamicForms.sln
MvcDynamicForms.Nuget
/MvcDynamicForms.Nuget.csproj
MvcDynamicForms.Nuget
/NuGet.config
MvcDynamicForms.Nuget
/NuGet.exe
MvcDynamicForms.Nuget
/NuGetPackage.ps1
MvcDynamicForms.Nuget
/NuGetSetup.ps1
MvcDynamicForms.Nuget
/Package.nuspec
MvcDynamicForms.Nuget/Properties
/AssemblyInfo.cs
MvcDynamicForms.Nuget/tools
/init.ps1
MvcDynamicForms.Nuget/tools
/install.ps1
MvcDynamicForms.Nuget/tools
/uninstall.ps1
File History: MvcDynamicForms.Nuget/NuGetPackage.ps1
Diff View
File View
feat: add nuget project
03/28/2016 by 紙鈔魚
365bf3
ADDED MvcDynamicForms.Nuget/NuGetPackage.ps1
End of History
@@ -0,0 +1,313 @@
Param ( 
  [switch]$Publish
)
 
$ErrorActionPreference = "Stop" 
$global:ExitCode = 1 
 
function Write-Log { 
 
  #region Parameters 
   
    [cmdletbinding()]
    Param(
      [Parameter(ValueFromPipeline=$true)]
      [array] $Messages,

      [Parameter()] [ValidateSet("Error", "Warn", "Info")]
      [string] $Level = "Info",

      [Parameter()]
      [Switch] $NoConsoleOut = $false,

      [Parameter()]
      [String] $ForegroundColor = 'White',

      [Parameter()] [ValidateRange(1,30)]
      [Int16] $Indent = 0,

      [Parameter()]
      [IO.FileInfo] $Path = ".\NuGet.log",

      [Parameter()]
      [Switch] $Clobber,

      [Parameter()]
      [String] $EventLogName,

      [Parameter()]
      [String] $EventSource,

      [Parameter()]
      [Int32] $EventID = 1

    )

  #endregion

Begin {} 
 
  Process { 
     
    $ErrorActionPreference = "Continue" 
 
    if ($Messages.Length -gt 0) { 
      try {       
        foreach($m in $Messages) {       
          if ($NoConsoleOut -eq $false) { 
            switch ($Level) { 
              'Error' {  
                Write-Error $m -ErrorAction SilentlyContinue
                Write-Host('{0}{1}' -f (" " * $Indent), $m) -ForegroundColor Red
              } 
              'Warn' {  
                Write-Warning $m  
              } 
              'Info' {  
                Write-Host('{0}{1}' -f (" " * $Indent), $m) -ForegroundColor $ForegroundColor 
              } 
            } 
          } 
 
          if ($m.Trim().Length -gt 0) { 
            $msg = '{0}{1} [{2}] : {3}' -f(" " * $Indent), (Get-Date -Format "yyyy-MM-dd HH:mm:ss"), $Level.ToUpper(), $m
