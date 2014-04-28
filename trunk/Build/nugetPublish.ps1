function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

try
{
	$scriptDir=Get-ScriptDirectory
	$nuspecFile=$scriptDir+"\nuget\Subasta.Lib.nuspec"
	$outputFolder=$scriptDir + "\..\Publish"
	.\nuget.exe pack $nuspecFile -o $outputFolder -NoDefaultExcludes

}
catch
{
	throw
	Exit -1
}