param($installPath, $toolsPath, $package, $project)

function InstallDrivers
{
    Write-Host "Running VISA driver downloader"
	$exePath = $toolsPath + "\NIVISA1401full_downloader.exe"
    Start-Process -FilePath $exePath
}

function InstallDriversFromMsi
{
    Write-Host "Installing VISA drivers"
    $installSwitch = "/i"
    $msiPath = $toolsPath + "\NIVISA1401full_downloader.exe"
    $features = "ADDLOCAL='.NET Framework 4.5 Language Support"
    Start-Process -FilePath $msiPath -ArgumentList /i, $msiPath
}

$regKey = 'HKLM:Software\National Instruments\NI-VISA\CurrentVersion'

If (!(Test-Path($regKey)))
{
    Write-Host "No Registry key for NI-VISA drivers"
    InstallDrivers
}
Else
{
    $val = (Get-ItemProperty -Path 'HKLM:Software\National Instruments\NI-VISA\CurrentVersion' -Name "Version").Version
    If ($val -ne "14.0.1")
    {
        Write-Host "Wrong version for NI-VISA drivers"
        InstallDrivers
    }
}

