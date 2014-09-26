using System.Diagnostics;
using System.IO;

namespace TsdLib.Configuration
{
    class FilePath
    {
        private readonly bool _stripExtension;

        public string FullPath { get; private set; }

        public FilePath(string fullPath, bool stripExtension)
        {
            FullPath = fullPath;
            _stripExtension = stripExtension;
        }

        public override string ToString()
        {
            Debug.Assert(FullPath != null);
            return _stripExtension ? Path.GetFileNameWithoutExtension(FullPath) : Path.GetFileName(FullPath);
        }
    }
}
