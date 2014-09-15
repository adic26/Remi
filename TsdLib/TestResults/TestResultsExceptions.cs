using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsdLib.TestResults
{
    public class MeasurementSerializationException : TsdLibException
    {
        public MeasurementSerializationException()
            : base("Error serializing") { }
    }

    public class MeasurementParameterException : TsdLibException
    {
        public MeasurementParameterException(string name)
            : base("A MeasurementParameter named " + name + " does not exist.") { }
        
    }
}
