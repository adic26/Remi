del /q *.nupkg

nuget pack %1 -Prop Configuration=%2 -IncludeReferencedProjects
nuget push *.nupkg -source \\fsg52ykf\ReliabilityTSD\TsdLib\Packages_Experimental
