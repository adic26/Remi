param($solutionDir, $configuration)

$path = $solutionDir + '\*\bin\' + $configuration

$folders = Get-ChildItem -Path $path

ForEach ($folder in $folders)
{
    ForEach ($package in Get-ChildItem -Path $folder.FullName -Recurse -Include *.nupkg -Exclude *.symbols.nupkg)
    {
        #Start-Process nuget.exe -ArgumentList 'push', $package, '-source http://tsd001ykf:81' -WindowStyle Hidden
		Write-Host Pushed $package to http://tsd001ykf:81
    }

    ForEach ($symbols in Get-ChildItem -Path $folder.FullName -Recurse -Include *.symbols.nupkg)
    {
        #Start-Process nuget.exe -ArgumentList 'push', $symbols, '-source http://tsd001ykf:82/nuget' -WindowStyle Hidden
		Write-Host Pushed $symbols to http://tsd001ykf:82/nuget
    }
}