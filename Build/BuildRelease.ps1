function prompt
{
    "PS " + $(get-location) + "> "
}
function Remove-Dir($target){
	if ([System.IO.Directory]::Exists($publishDirectory) -eq $true)
		{
			Remove-Item $publishDirectory -Force -Recurse
		}
}

function Create-BuildDirectory($target){
if ([System.IO.Directory]::Exists($target) -eq $false)
	{
		[System.IO.Directory]::CreateDirectory($target)
	}
}


try{
	prompt
	
	"removing previous Publish"
	$scriptLocation= get-location
	$publishDirectory= "$scriptLocation\..\publish"
	$buildDirectory="$scriptLocation\..\Output"
	$buildDirectoryLib="$buildDirectory\Lib"
	$buildDirectoryExe="$buildDirectory\Exe"
	
	"publishDirectory=$publishDirectory"
	"buildDirectory=$buildDirectory"
	Remove-Dir($publishDirectory)
	Remove-Dir($buildDirectory)
	Create-BuildDirectory($buildDirectoryLib)
	Create-BuildDirectory($buildDirectoryExe)
	
	"Setting version"
	$version = Get-Date -format 0.yyyy.MMdd.HHmm
	.\SetVersions.ps1  $version  
	
	"Creating module"
	C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild .\..\Code\Subasta.Lib\Subasta.Lib.csproj /p:Configuration=Release /property:OutputPath="$buildDirectoryLib"
	.\MergeAssemblies.ps1 -targetProject "Code\Subasta.Lib" -buildDirectory $buildDirectoryLib -primaryAssembly "Subasta.Lib.dll" -buildConfiguration "Release" -outputAssembly "Subasta.Lib.dll" -targetMergeKind "library" #.{library, exe, winexe} # -internalize
	.\nugetPublish
	
	"Creating the loader"
	C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild .\..\Code\Subasta.ModuleLoader\Subasta.ModuleLoader.csproj /p:Configuration=Release /property:OutputPath="$buildDirectoryExe"
	.\MergeAssemblies.ps1 -targetProject "Code\Subasta.ModuleLoader" -buildDirectory $buildDirectoryExe -primaryAssembly "Subasta.exe" -buildConfiguration "Release" -outputAssembly "subasta.exe" -targetMergeKind winexe #-internalize #.{library, exe, winexe} # -internalize
}
catch
{
	throw
	Exit -1
}