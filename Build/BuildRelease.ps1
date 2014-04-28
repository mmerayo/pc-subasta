function prompt
{
    "PS " + $(get-location) + "> "
}

try{
	prompt
	#C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild .\..\Code\Subasta\Subasta.csproj /p:Configuration=Release
	.\MergeAssemblies.ps1 -targetProject "Code\Subasta" -buildConfiguration "Release" -outputAssembly "Subasta.exe" -mvc3 -internalize
}
catch
{
	throw
	Exit -1
}