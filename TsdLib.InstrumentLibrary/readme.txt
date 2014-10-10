To enable the instrument code generator, please perform the following:
1) Click the Windows button and in the search bar, type: cmd
2) When cmd.exe appears, right-click it and select Run As Administrator
3) Copy and paste the following command into the command prompt by copying the text and right-clicking on the command prompt window. Make sure to replace <your_solution_directory> with your solution directory

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe <your_solution_directory>\packages\tools\TsdLib.InstrumentLibrary.dll> /codebase"



To use the code generator:
1) In the Solution Explorer, right-click and instrument xml definition file and select Properties
2) In the Copy To Output Directory field, select: Copy if newer
3) In the Custom Tool field, enter: InstrumentClassGenerator
4) In the Cutom Tool Namespace field, enter your project name
5) Right-click the instrument xml definition file and select: Run Custom Tool