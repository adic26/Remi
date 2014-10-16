param($installPath, $toolsPath, $package, $project) 
Remove-Item -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\InstrumentClassGenerator'
New-Item -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}' -name InstrumentClassGenerator
New-ItemProperty -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\InstrumentClassGenerator' -name "(Default)" -value "For Visual Studio 2012. Transforms an instrument xml definition into a C# class file"
New-ItemProperty -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\InstrumentClassGenerator' -name CLSID -value "{38E26680-7B84-4AE3-B49C-0D0B9E08BEAF}"
New-ItemProperty -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\InstrumentClassGenerator' -name "GeneratesDesignTimeSource" -value 1

$RegAsm = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory() + 'RegAsm.exe'
$SourceFolder = $toolsPath + '\asy\*'

Write-Host $SourceFolder

$DestinationFolder = "C:\ProgramData\TsdLib\CodeGenerator\" + $package.version

Write-Host $DestinationFolder

$Assembly = $DestinationFolder + '\TsdLib.InstrumentLibrary.dll'
$Codebase = '/codebase'

New-Item -Force $DestinationFolder -Type Directory
Copy-Item $SourceFolder $DestinationFolder -Force

Start-Process $RegAsm -ArgumentList $Assembly, $Codebase -Verb runAs