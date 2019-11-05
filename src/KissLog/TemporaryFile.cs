using System;
using System.IO;

namespace KissLog
{
    public class TemporaryFile : IDisposable
    {
        public string FileName { get; private set; }

        private static string GenerateSalt()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 12);
        }

        private static string GetTempFileName()
        {
            string path = Path.Combine(Path.GetTempPath(), "KissLog", $"{GenerateSalt()}.tmp");

            try
            {
                FileInfo fi = new FileInfo(path);
                fi.Directory.Create();

                using (File.Create(path)) { };
            }
            catch
            {
                path = null;
            }


            if (path == null)
            {
                try
                {
                    path = Path.Combine(Path.GetTempPath(), $"KissLog_{GenerateSalt()}.tmp");
                    using (File.Create(path)) { };
                }
                catch
                {
                    path = null;
                }
            }

            if (path == null)
            {
                path = Path.GetTempFileName();
            }

            return path;
        }

        public TemporaryFile() : this(GetTempFileName())
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