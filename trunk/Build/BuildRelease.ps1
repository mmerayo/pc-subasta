try{
	"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild" ".\..\Code\Subasta\Subasta\Subasta.csproj" /p:Configuration=Debug
	.\MergeAssemblies.ps1 -targetProject ".\..\Code\Subasta\Subasta" -buildConfiguration "Release" -outputAssembly "Subasta.exe" -mvc3 -internalize
}
catch
{
	throw
	Exit -1
}