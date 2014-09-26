using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TsdLib.CodeGenerator;

namespace TsdLib.CodeGeneratorApp
{
    class CodeGeneratorAppProgram
    {
        static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            if (args.Length < 5)
            {
                Trace.WriteLine("Wrong number of arguments");
                return -1;
            }

            string testSystemName = args[0];
            string instrumentsFolder = args[1];
            string schemaFile = args[2];
            string outputDirectory = args[3];
            Language language = (Language)Enum.Parse(typeof(Language), args[4]);

            string[] instrumentsFiles = Directory.EnumerateFiles(instrumentsFolder)
                .Where(file => Path.GetExtension(file) == ".xml")
                .ToArray();

            Generator.GenerateInstrumentsClassFile(testSystemName, instrumentsFiles, schemaFile, outputDirectory, language, false);

            return 0;
        }
    }
}
