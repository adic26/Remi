To use the build tools,

Reference TsdLib.Build.dll in your target project
Copy the UsingTask and Target elements from the bottom of BuildToolsTest2.csproj to the bottom of your *.csproj file (note the AssemblyFile paths will need to be updated to point to the TsdLib.Build.dll file



To develop the build tools,
In Solution Configuration Manager, re-enable the build option for TsdLib.Build project
	It is currently disabled due to issues with MSBuild.exe locking the TsdLib.Build.dll
You may need to manually end any MSBuild.exe process in the Windows Task Manager between builds