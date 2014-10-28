using System.IO;
using TsdLib.Annotations;

namespace TsdLib.Configuration
{
    class FilePath
    {
        private readonly bool _stripExtension;

        [NotNull]
        public string FullPath { get; private set; }

        public FilePath(string fullPath, bool stripExtension)
        {
            FullPath = fullPath;
            _stripExtension = stripExtension;
        }

        public override string ToString()
        {
            return _stripExtension ? Path.GetFileNameWithoutExtension(FullPath) : Path.GetFileName(FullPath);
        }
    }
}
