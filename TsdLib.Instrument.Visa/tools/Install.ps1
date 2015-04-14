param($installPath, $toolsPath, $package, $project)

function InstallDrivers
{
    Write-Host "Running VISA driver downloader"
	
	$projectFullName = $project.FullName
	$fileInfo = new-object -typename System.IO.FileInfo -ArgumentList $projectFullName
	$projectDirectory = $fileInfo.DirectoryName
	$exePath = $projectDirectory + "\NIVISA1401full_downloader.exe"
	Write-Host exe path = $exePath
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

$project.ProjectItems.Item("NIVISA1401full_downloader.exe").Properties.Item("CopyToOutputDirectory").Value = 2
$regKey = 'HKLM:Software\National Instruments\NI-VISA\CurrentVersion'

If (!(Test-Path($regKey)))
{
    Write-Host "No Registry key for NI-VISA drivers"
    InstallDrivers
}
Else
{
    $val = (Get-ItemProperty -Path 'HKLM:Software\National Instruments\NI-VISA\CurrentVersion' -Name "Version").Version
	Write-Host Found VISA version $val installed
    If ($val -ne "14.0.1")
    {
        Write-Host "Wrong version for NI-VISA drivers"
        InstallDrivers
    }
}

