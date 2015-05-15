using System.Linq;

namespace TsdLib.Configuration
{
    public class LocalSequenceConfig : ISequenceConfig
    {
        public LocalSequenceConfig(string fullSequenceTypeName)
        {
            SourceCode = "Not yet implemented, but this will be the source code";
            FullTypeName = fullSequenceTypeName;

            Name = fullSequenceTypeName.Split('.').LastOrDefault() ?? fullSequenceTypeName;
            Namespace = fullSequenceTypeName.Replace(Name, "");
        }

        public string SourceCode { get; private set; }

        public string FullTypeName { get; private set; }

        public string Name { get; set; }

        public bool StoreInDatabase { get; set; }

        public bool IsDefault { get; set; }

        public bool IsValid { get { return true; } }

        public IConfigItem Clone()
        {
            return this;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Namespace { get; private set; }
    }
}
