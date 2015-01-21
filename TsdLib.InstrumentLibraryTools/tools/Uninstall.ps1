param($installPath, $toolsPath, $package, $project)
Remove-Item -Path 'HKCU:\Software\Microsoft\VisualStudio\11.0_Config\Generators\{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}\InstrumentClassGenerator'


ForEach ($config in $project.ConfigurationManager)
{
$constants = $config.Properties.Item("DefineConstants").Value
$config.Properties.Item("DefineConstants").Value = $constants.Replace(";INSTRUMENT_LIBRARY", "")
}

$RegAsm = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory() + 'RegAsm.exe'

$Assembly = 'C:\ProgramData\TsdLib\InstrumentLibraryTools\' + $package.version + '\' + $package.Id + '.dll'
Start-Process $RegAsm -ArgumentList /u, $Assembly -Verb runAs