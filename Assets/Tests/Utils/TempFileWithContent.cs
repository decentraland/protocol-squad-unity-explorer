using System;
using System.IO;

namespace TestUtils
{
    /// <summary>
    /// Create temp file with content passed to constructor
    /// </summary>
    public class TempFileWithContent : IDisposable
    {
        public readonly string Path;
        public string NormalizedPath => Path.Replace("\\", "/").Normalize();

        public TempFileWithContent(string fileContent)
        {
            Path = System.IO.Path.GetTempFileName();
            File.WriteAllText(Path, fileContent);
        }
        
        public void Dispose()
        {
            File.Delete(Path);
        }
    }
}