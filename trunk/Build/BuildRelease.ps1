try{
	.\MergeAssemblies.ps1 -targetProject ".\..\Code\Subasta\Subasta" -buildConfiguration "Release" -outputAssembly "Subasta.exe" -mvc3 -internalize

}
catch
{
	throw
	Exit -1
}