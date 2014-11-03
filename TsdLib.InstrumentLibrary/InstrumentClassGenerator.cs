using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using TsdLib.CodeGenerator;

namespace TsdLib.InstrumentLibrary
{
    [Guid("38E26680-7B84-4AE3-B49C-0D0B9E08BEAF")]
    [CustomTool("InstrumentClassGenerator", "For Visual Studio 2012. Transforms an instrument xml definition into a C# class file.")]
    public class InstrumentClassGenerator : IVsSingleFileGenerator
    {
        internal static Guid CSharpCategoryGuid = new Guid("FAE04EC1-301F-11D3-BF4B-00C04F79EFBC");
        
        public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            try
            {
                Generator generator = new Generator(wszDefaultNamespace, new[] { wszInputFilePath }, Language.CSharp);

                string sourceCode = generator.GenerateInstrumentsClassSourceCode();
                byte[] bytes = Encoding.UTF8.GetBytes(sourceCode);

                rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(bytes.Length);
                Marshal.Copy(bytes, 0, rgbOutputFileContents[0], bytes.Length);

                pcbOutput = (uint)bytes.Length;

                return 0;
            }
            catch (Exception ex)
            {
                byte[] bytes = Encoding.UTF8.GetBytes("The xml file is invalid." + Environment.NewLine + ex);
                rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(bytes.Length);
                Marshal.Copy(bytes, 0, rgbOutputFileContents[0], bytes.Length);

                pcbOutput = (uint)bytes.Length;
                return 0;
            }
        }

        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = ".cs";
            return pbstrDefaultExtension.Length;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class CustomToolAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public CustomToolAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}