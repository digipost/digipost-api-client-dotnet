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

function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

$CurrentScriptDir = Get-ScriptDirectory

$Xsd1 = "\Xsd\api_v7.xsd"
$XsdPath = "$CurrentScriptDir$Xsd1"

$XsdToCodeDir = (Get-Item $CurrentScriptDir)
$CodeDir =  "$XsdToCodeDir\Code"

GenerateCode $XsdPath "$CodeDir"