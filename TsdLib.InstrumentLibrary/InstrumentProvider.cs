using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TsdLib.CodeGenerator;
using TsdLib.InstrumentLibraryTools;

namespace TsdLib.InstrumentLibrary
{
    class InstrumentProvider
    {
        public IEnumerable<CodeCompileUnit> GetCodeCompileUnits(Language language)
        {
            if (!Directory.Exists("Instruments"))
                throw new DirectoryNotFoundException("No Instruments directory exists");

            string[] instrumentXmlFiles = Directory.GetFiles("Instruments", "*.xml");
            InstrumentParser instrumentXmlParser = new InstrumentParser("TsdLib.InstrumentLibrary.Visualizer", language.ToString());
            List<CodeCompileUnit> codeCompileUnits = instrumentXmlFiles.Select(xmlFile => instrumentXmlParser.Parse(new StreamReader(xmlFile))).ToList();

            if (Directory.Exists(@"Instruments\Interfaces"))
            {
                string[] instrumentHelperCsFiles = Directory.GetFiles(@"Instruments\Interfaces", "*.cs.pp");
                string[] instrumentHelperVbFiles = Directory.GetFiles(@"Instruments\Interfaces", "*.vb.pp");
                BasicCodeParser instrumentHelperParser = new BasicCodeParser();
                codeCompileUnits.AddRange(instrumentHelperCsFiles.Concat(instrumentHelperVbFiles).Select(xmlFile => instrumentHelperParser.Parse(new StreamReader(xmlFile))));
            }

            return codeCompileUnits;
        }

        public Assembly GetInstrumentsAssembly(Language language)
        {
            if (!Directory.Exists("Instruments"))
                throw new DirectoryNotFoundException("No Instruments directory exists");

            string[] instrumentXmlFiles = Directory.GetFiles("Instruments", "*.xml");
            InstrumentParser instrumentXmlParser = new InstrumentParser("TsdLib.InstrumentLibrary.Visualizer", language.ToString());
            List<CodeCompileUnit> codeCompileUnits = instrumentXmlFiles.Select(xmlFile => instrumentXmlParser.Parse(new StreamReader(xmlFile))).ToList();

            if (Directory.Exists(@"Instruments\Interfaces"))
            {
                string[] instrumentHelperCsFiles = Directory.GetFiles(@"Instruments\Interfaces", "*.cs.pp");
                string[] instrumentHelperVbFiles = Directory.GetFiles(@"Instruments\Interfaces", "*.vb.pp");
                BasicCodeParser instrumentHelperParser = new BasicCodeParser();
                codeCompileUnits.AddRange(instrumentHelperCsFiles.Concat(instrumentHelperVbFiles).Select(xmlFile => instrumentHelperParser.Parse(new StreamReader(xmlFile))));
            }

            DynamicCompiler generator = new DynamicCompiler(language, AppDomain.CurrentDomain.BaseDirectory);
            var assembly = generator.Compile(codeCompileUnits);
            return Assembly.LoadFrom(assembly);
        }
    }
}
