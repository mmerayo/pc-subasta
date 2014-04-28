<#
.SYNOPSIS
IlMerges an assembly with its dependencies. Depends on nuget being installed in the PATH.
 
.PARAMETER targetProject
The name of the project to be ilmerged
 
.PARAMETER outputAssembly
The name of the ilmerged assembly when it is created
 
.PARAMETER buildConfiguration
The build configuration used to create the assembly. Used to locate the assembly under the project. The usual format is Project/bin/Debug
 
.PARAMETER targetPlatform
Defaults to .NET 4
 
.PARAMETER mvc3
If the project is an Mvc3 project, the MVC3 assemblies need to be added to the ilmerge list. The script assumes that the MVC3 assemblies are installed in the default location.
 
.PARAMETER internalize
Adds the /internalize flag to the merged assembly to prevent namespace conflicts.
 
.EXAMPLE
MergeAssemblies.ps1 -targetProject "Subasta" -buildConfiguration "Release" -outputAssembly "Subasta.exe" -mvc3 -internalize
 
#>
param(
    [parameter(Mandatory=$true)] $targetProject, 
    $outputAssembly = "$targetProject.dll", 
    $buildConfiguration = "",
    $targetPlatform = "v4,c:\windows\Microsoft.NET\Framework\v4.0.30319",
	$targetMergeKind="dll",
    [switch] $mvc3,
    [switch] $internalize
)
 
function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}
 
function Get-Mvc3Dependencies()
{
    $system_web_mvc = """c:\Program Files (x86)\Microsoft ASP.NET\ASP.NET MVC 3\Assemblies\System.Web.Mvc.dll"""
    $system_web_webpages = """c:\Program Files (x86)\Microsoft ASP.NET\ASP.NET Web Pages\v1.0\Assemblies\System.Web.WebPages.dll"""
    $system_web_razor = """c:\Program Files (x86)\Microsoft ASP.NET\ASP.NET Web Pages\v1.0\Assemblies\System.Web.Razor.dll"""
    $system_web_webpages_razor = """c:\Program Files (x86)\Microsoft ASP.NET\ASP.NET Web Pages\v1.0\Assemblies\System.Web.WebPages.Razor.dll"""
 
    $mvc3Assemblies = "$system_web_mvc $system_web_webpages $system_web_razor $system_web_webpages_razor"
 
    return $mvc3Assemblies
}
 
function Get-InputAssemblyNames($buildDirectory)
{
	$assemblyNames = dir -path $buildDirectory -Include @("*.dll","*.exe") -rec | ForEach-Object { """" + $_.FullName + """" }
    #$assemblyNames = Get-ChildItem -Path $buildDirectory -Filter *.dll *.exe | ForEach-Object { """" + $_.FullName + """" }
	write-host "Assemblies to merge: $assemblyNames"
 
    $inArgument = [System.String]::Join(" ", $assemblyNames)
    return $inArgument
}
 
function Get-BuildDirectory($solutionDirectoryFullName)
{
 
    $targetProjectDirectory = "$solutionDirectoryFullName\$targetProject"
    $result = "$targetProjectDirectory\bin"
	if ($buildConfiguration -ne "")
	{
		$result = Join-Path $result $buildConfiguration
    }
	return $result
}
 
try
{
 
	$scriptPath= Get-ScriptDirectory
	$scriptDirectory = new-object System.IO.DirectoryInfo $scriptPath
	$solutionDirectory = $scriptDirectory.Parent
	$solutionDirectoryFullName = $solutionDirectory.FullName
 
 
	$ilMergeAssembly = "$scriptDirectory\.ilmerge\IlMerge\IlMerge.exe"
	$publishDirectory = "$solutionDirectoryFullName\Publish"
	$outputAssemblyFullPath = "$publishDirectory\$outputAssembly"
 
	$buildDirectory = Get-BuildDirectory $solutionDirectoryFullName
 
	"Script Directory  : $scriptPath"
	"Solution Directory: $solutionDirectoryFullName"
	"Build Directory   : $buildDirectory"
	"Publish Directory : $publishDirectory"
 
 
	$outArgument = "/out:$publishDirectory/$outputAssembly"
	$inArgument = Get-InputAssemblyNames $buildDirectory
 
 
	#    MVC3 assemblies are not a part of the .NET Framework, but neither are they a nuget package.
	#    They have to be referenced directly.
 
	if ($mvc3 -eq $true)
	{
		$mvcAssemblies = Get-Mvc3Dependencies
		$inArgument = "$inArgument $mvcAssemblies"
	}
	$cmd = "$ilMergeAssembly /t:$targetMergeKind /targetPlatform:""$targetPlatform"" $outArgument $inArgument /attr:""$buildDirectory\Subasta.Lib.dll"""
 
	if ($internalize)
	{
		$cmd = $cmd + " /internalize"
	}
 
 
	"Installing ilmerge"
	.\nuget install IlMerge -outputDirectory .ilmerge -ExcludeVersion
 
	"Ensuring that publication directory exists"
	if ([System.IO.Directory]::Exists($publishDirectory) -eq $false)
	{
		[System.IO.Directory]::CreateDirectory($publishDirectory)
	}
 
	"Running Command: $cmd"
	$result = Invoke-Expression $cmd
	"result " + $result
	"Getting assembly info for $outputAssemblyFullPath"
 
	$outputAssemblyInfo = New-Object System.IO.FileInfo $outputAssemblyFullPath
	if ($outputAssemblyInfo.Length -eq 0)
	{
		$outputAssemblyInfo.Delete	
	}
 
	$outputAssemblyInfo
 
	if ($outputAssemblyInfo.Exists -eq $false)
	{
		throw "Output assembly not created by ilmerge script."
		Exit -1
	}
	elseif ($outputAssemblyInfo.Length -eq 0) 
	{
		$outputAssemblyInfo.Delete();
		Exit -1;
	}
	else
	{
		"Output assembly created successfully at $outputAssemblyFullPath."
		Exit 0;
	}
}
catch
{
	throw
	Exit -1
}