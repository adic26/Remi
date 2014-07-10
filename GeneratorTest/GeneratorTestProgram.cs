using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorTest
{
    class GeneratorTestProgram
    {
        static void Main(string[] args)
        {
            string projectDir = @"C:\Users\jmckee\Source\Repos\TsdLib\TsdLib.Instrument";
            TsdLib.InstrumentGenerator.InstrumentGenerator.GenerateCodeFile(projectDir, projectDir);
        }
    }
}
