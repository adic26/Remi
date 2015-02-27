using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TsdLib.InstrumentLibraryTools;

namespace TsdLib.InstrumentLibrary
{
    class InstrumentProvider
    {
        public IEnumerable<CodeCompileUnit> GetCodeCompileUnits()
        {
            if (!Directory.Exists("Instruments"))
                throw new DirectoryNotFoundException("No Instruments directory exists");

            string[] instrumentXmlFiles = Directory.GetFiles("Instruments", "*.xml");
            InstrumentParser instrumentXmlParser = new InstrumentParser("TsdLib.InstrumentLibrary.Visualizer", "CSharp");
            List<CodeCompileUnit> codeCompileUnits = instrumentXmlFiles.Select(xmlFile => instrumentXmlParser.Parse(new StreamReader(xmlFile))).ToList();

            if (Directory.Exists(@"Instruments\Interfaces"))
            {
                IEnumerable<CodeSnippetCompileUnit> instrumentHelperFiles = 
                    Directory.GetFiles(@"Instruments\Interfaces", "*.cs.pp")
                    .Concat(Directory.GetFiles(@"Instruments\Interfaces", "*.vb.pp"))
                    .Select(file => new StreamReader(file))
                    .Select(reader => new CodeSnippetCompileUnit(reader.ReadToEnd()));

                codeCompileUnits.AddRange(instrumentHelperFiles.ToArray());
            }

            return codeCompileUnits;
        }

        public Assembly GetInstrumentsAssembly(string language)
        {
            throw new NotImplementedException();
            //if (!Directory.Exists("Instruments"))
            //    throw new DirectoryNotFoundException("No Instruments directory exists");

            //string[] instrumentXmlFiles = Directory.GetFiles("Instruments", "*.xml");
            //InstrumentParser instrumentXmlParser = new InstrumentParser("TsdLib.InstrumentLibrary.Visualizer", language.ToString());
            //List<CodeCompileUnit> codeCompileUnits = instrumentXmlFiles.Select(xmlFile => instrumentXmlParser.Parse(new StreamReader(xmlFile))).ToList();

            //if (Directory.Exists(@"Instruments\Interfaces"))
            //{
            //    IEnumerable<CodeSnippetCompileUnit> instrumentHelperFiles =
            //        Directory.GetFiles(@"Instruments\Interfaces", "*.cs.pp")
            //        .Concat(Directory.GetFiles(@"Instruments\Interfaces", "*.vb.pp"))
            //        .Select(file => new StreamReader(file))
            //        .Select(reader => new CodeSnippetCompileUnit(reader.ReadToEnd()));

            //    codeCompileUnits.AddRange(instrumentHelperFiles.ToArray());
            //}

            //DynamicCompiler generator = new DynamicCompiler(language, AppDomain.CurrentDomain.BaseDirectory);
            //var assembly = generator.Compile(codeCompileUnits);
            //return Assembly.LoadFrom(assembly);
        }
    }
}
