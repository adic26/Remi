Getting Started with the TsdLib Instrument Library 

Instrument XML definitions are placed in the Instruments project folder.

To include the instrument definitions in the test sequence:
1) Right-click the instrument xml file and select Properties.
2) In the Copy to Output Directory field, select Copy if newer
This will ensure that the latest instrument definition is copied to the build output directory, where it will be picked up by the test sequence assembly generator.

To generate a C# code file (useful for design-time verification):
1) Right-click the instrument xml file and select Properties.
2) In the Custom Tool field, enter InstrumentClassGenerator
3) In the Custom Tool Namespace field, enter the name of your project
Whenever the instrument xml file is modified and saved, the C# code file will be automatically regenerated.
You can also manually regenerate it by right-clicking the instrument xml file and selecting Run Custom Tool.