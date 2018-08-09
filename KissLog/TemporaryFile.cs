using System;
using System.IO;

namespace KissLog
{
    public class TemporaryFile : IDisposable
    {
        public string FileName { get; private set; }

        public TemporaryFile() : this(Path.GetTempFileName())
        {
        }

        public TemporaryFile(string fileName)
        {
            FileName = fileName;
        }

        public void Dispose()
        {
            try
            {
                if (FileName != null && File.Exists(FileName))
                    File.Delete(FileName);

                FileName = null;
            }
            catch
            {
                // ignored
            }
        }
    }
}
