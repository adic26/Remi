using System;
using System.IO;

namespace TsdLib.Configuration
{
    [Obsolete]
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
            if (FullPath == null)
                throw new Exception();
            return _stripExtension ? Path.GetFileNameWithoutExtension(FullPath) : Path.GetFileName(FullPath);
        }
    }
}
