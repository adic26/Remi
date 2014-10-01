del *.nupkg
nuget pack %1 -Prop Configuration=%2
if %2 == Release xcopy /y *.nupkg \\fsg52ykf\ReliabilityTSD\TsdLib\Packages\