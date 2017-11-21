# RUNNING THE FIRST TIME
# To be able to run the first time, enable running of scripts for current user. Open PowerShell as admin and run:
#	Set-Executionpolicy -Scope CurrentUser -ExecutionPolicy UnRestricted
# If you want to check that it was set:
#	Get-ExecutionPolicy -List | Format-Table -AutoSize
# Set policy to restricted afterwards:
#  Set-Executionpolicy -Scope CurrentUser -ExecutionPolicy Restricted
# If problems running script from network drive:
#  net use Z: `"\\vmware-host\Shared Folders`

$Functions = $PSScriptRoot + "\Functions.ps1"
. $Functions

$CodeDir = Get-CodeDirectory

$XsdRelativePath= "\Digipost.Api.Client.Resources\Xsd\Data\api_V7.xsd"
$XsdPath = Get-Xsd($XsdRelativePath)

Generate-Code-From-Xsd $XsdPath "$CodeDir"