function prompt
{
    "PS " + $(get-location) + "> "
}

try{
	prompt
	C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild .\..\Code\Subasta\Subasta.csproj /p:Configuration=Release
	.\MergeAssemblies.ps1 -targetProject "Code\Subasta.Lib" -buildConfiguration "Release" -outputAssembly "Subasta.Lib.dll" -targetMergeKind "library" -internalize #.{library, exe, winexe} # -internalize
}
catch
{
	throw
	Exit -1
}