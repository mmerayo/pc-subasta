function prompt
{
    "PS " + $(get-location) + "> "
}

try{
	prompt
	
	"removing previous Publish"
	$scriptLocation= get-location
	$publishDirectory= "$scriptLocation\..\publish"
	"publishDirectory=$publishDirectory"
	if ([System.IO.Directory]::Exists($publishDirectory) -eq $true)
	{
		Remove-Item $publishDirectory -Force -Recurse
	}
	
	"Setting version"
	$version = Get-Date -format 0.yyyy.MMdd.HHmm
	.\SetVersions.ps1  $version  
	
	"Creating module"
	C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild .\..\Code\Subasta.Lib\Subasta.Lib.csproj /p:Configuration=Release
	.\MergeAssemblies.ps1 -targetProject "Code\Subasta.Lib" -primaryAssembly "Subasta.Lib.dll" -buildConfiguration "Release" -outputAssembly "Subasta.Lib.dll" -targetMergeKind "library" #.{library, exe, winexe} # -internalize
	.\nugetPublish
	
	"Creating the loader"
	C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild .\..\Code\Subasta.ModuleLoader\Subasta.ModuleLoader.csproj /p:Configuration=Release
	.\MergeAssemblies.ps1 -targetProject "Code\Subasta.ModuleLoader" -primaryAssembly "Subasta.exe" -buildConfiguration "Release" -outputAssembly "subasta.exe" -targetMergeKind winexe #-internalize #.{library, exe, winexe} # -internalize
}
catch
{
	throw
	Exit -1
}