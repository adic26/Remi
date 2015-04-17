param($solutionDir, $configuration)

$path = $solutionDir + '\*\bin\' + $configuration

$folders = Get-ChildItem -Path $path

ForEach ($folder in $folders)
{
    ForEach ($package in Get-ChildItem -Path $folder.FullName -Recurse -Include *.nupkg -Exclude *.symbols.nupkg)
    {
        $proc = Start-Process nuget.exe -ArgumentList 'push', $package, '-source http://tsd001ykf:81' -WindowStyle Hidden -Wait -PassThru

		#If ($proc.ExitCode -eq 0)
		 Write-Host Pushed $package to http://tsd001ykf:81 
		#Else
		#{ Write-Host Failed to push $package. Error code = $proc.ExitCode }

    }

    ForEach ($symbols in Get-ChildItem -Path $folder.FullName -Recurse -Include *.symbols.nupkg)
    {
        $proc = Start-Process nuget.exe -ArgumentList 'push', $symbols, '-source http://tsd001ykf:82/nuget' -WindowStyle Hidden
		#If ($proc.ExitCode -eq 0)
		 Write-Host Pushed $symbols to http://tsd001ykf:82/nuget 
		#Else
		#{ Write-Host Failed to push $symbols. Error code = $proc.ExitCode }
    }
}