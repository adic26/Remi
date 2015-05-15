//using System.Collections.Generic;

namespace TsdLib.Configuration
{
    public interface ISequenceConfig : IConfigItem
    {
        //HashSet<string> AssemblyReferences { get; }
        string SourceCode { get; }

        string FullTypeName { get; }
        //string ClassName { get; }
        string Namespace { get; }
    }
}