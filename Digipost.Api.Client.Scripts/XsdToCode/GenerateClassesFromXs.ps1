# RUNNING THE FIRST TIME
# To be able to run the first time, enable running of scripts for current user. Open PowerShell as admin and run:
#	Set-Executionpolicy -Scope CurrentUser -ExecutionPolicy UnRestricted
# If you want to check that it was set:
#	Get-ExecutionPolicy -List | Format-Table -AutoSize
# Set policy to restricted afterwards:
#  Set-Executionpolicy -Scope CurrentUser -ExecutionPolicy Restricted
# If problems running script from network drive:
#  net use Z: `"\\vmware-host\Shared Folders`

function GenerateCode($XsdPath, $OutDir)
{
    Write-Host("XSD: $XsdPath")
    Write-Host("OUT: $OutDir")
    
    $CurrentDirectory = (Get-Item -Path ".\" -Verbose).FullName
	Set-Location "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\"
    Invoke-Expression ".\Xsd.exe $XsdPath /classes /out:$OutDir"
    Set-Location $CurrentDirectory
}

function Get-CodeDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  $ScriptDir = Split-Path $Invocation.MyCommand.Path

  $ScriptDir+"\Code"
}

function Get-Xsd($XsdPathRelativeToSourceDir)
{
  $FullScriptPath = (Get-Variable MyInvocation -Scope 1).Value
  $FullScriptDir =  Split-Path $FullScriptPath.MyCommand.Path
  $SourceDir = (Get-Item $FullScriptDir).Parent.Parent.FullName
  $XsdPath = "$SourceDir$XsdPathRelativeToSourceDir"
  
  $XsdPath
}


$CodeDir = Get-CodeDirectory

$XsdRelattivePath= "\Digipost.Api.Client.Resources\Xsd\Data\api_v7.xsd"
$XsdPath = Get-Xsd($XsdRelativePath)

GenerateCode $XsdPath "$CodeDir"