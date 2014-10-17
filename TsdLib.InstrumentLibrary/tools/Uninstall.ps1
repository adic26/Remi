param($installPath, $toolsPath, $package, $project)
Remove-Item -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\InstrumentClassGenerator'

$RegAsm = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory() + 'RegAsm.exe'

$Assembly = 'C:\ProgramData\TsdLib\CodeGenerator\' + $package.version + '\TsdLib.InstrumentLibrary.dll'
Start-Process $RegAsm -ArgumentList /u, $Assembly -Verb runAs