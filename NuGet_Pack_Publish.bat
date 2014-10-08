del *.nupkg
if [%3]==[] nuget pack %1 -Prop Configuration=%2
if [%3]==[tool] nuget pack %1 -Prop Configuration=%2 -Tool
if %2 == Release xcopy /y *.nupkg \\fsg52ykf\ReliabilityTSD\TsdLib\Packages\
if %2 == Debug xcopy /y *.nupkg \\fsg52ykf\ReliabilityTSD\TsdLib\Packages_Debug\