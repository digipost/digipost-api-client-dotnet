function Generate-Code-From-Xsd($XsdPath, $OutDir)
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

# export-modulemember -function Generate-Code-From-Xsd