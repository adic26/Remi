param($installPath, $toolsPath, $package, $project) 
New-Item -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}' -name InstrumentClassGenerator -Force
New-ItemProperty -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\InstrumentClassGenerator' -name "(Default)" -value "For Visual Studio 2012. Transforms an instrument xml definition into a C# class file" -Force
New-ItemProperty -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\InstrumentClassGenerator' -name CLSID -value "{38E26680-7B84-4AE3-B49C-0D0B9E08BEAF}" -Force
New-ItemProperty -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\InstrumentClassGenerator' -name "GeneratesDesignTimeSource" -value 1 -Force

$dllPath = $installPath + "\lib\net45\" + $package.Id + ".dll"

ForEach ($config in $project.ConfigurationManager)
{
$constants = $config.Properties.Item("DefineConstants").Value + ";INSTRUMENT_LIBRARY"
$config.Properties.Item("DefineConstants").Value = $constants
}

$RegAsm = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory() + 'RegAsm.exe'

$DestinationFolder = "C:\ProgramData\TsdLib\CodeGenerator\" + $package.version

$Assembly = $DestinationFolder + '\TsdLib.InstrumentLibrary.dll'
$Codebase = '/codebase'

New-Item -Force $DestinationFolder -Type Directory
Copy-Item $dllPath $DestinationFolder -Force

Start-Process $RegAsm -ArgumentList $Assembly, $Codebase -Verb runAs

'InstrumentClassGenerator' | clip