function prompt
{
    "PS " + $(get-location) + "> "
}

try{
	prompt
	
	$version = Get-Date -format 0.yyyy.MMdd.HHmm
	
	.\SetVersions.ps1  $version  
	#C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild .\..\Code\Subasta.Lib\Subasta.Lib.csproj /p:Configuration=Release
	#.\MergeAssemblies.ps1 -targetProject "Code\Subasta.Lib" -buildConfiguration "Release" -outputAssembly "Subasta.Lib.dll" -targetMergeKind "library" -internalize #.{library, exe, winexe} # -internalize
}
catch
{
	throw
	Exit -1
}