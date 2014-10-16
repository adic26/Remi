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

            if (args.Length < 4)
            {
                Trace.WriteLine("Wrong number of arguments");
                return -1;
            }

            string testSystemName = args[0];
            string instrumentsFolder = args[1];
            string outputDirectory = args[2];
            Language language = (Language)Enum.Parse(typeof(Language), args[3]);

            string[] instrumentsFiles = Directory.EnumerateFiles(instrumentsFolder)
                .Where(file => Path.GetExtension(file) == ".xml")
                .ToArray();

            using (Generator generator = new Generator(testSystemName, instrumentsFiles, language))
                generator.GenerateInstrumentsClassFile(outputDirectory);

            return 0;
        }
    }
}
