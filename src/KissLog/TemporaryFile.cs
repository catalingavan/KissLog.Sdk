using System;
using System.IO;
using System.Linq;

namespace KissLog
{
    public class TemporaryFile : IDisposable
    {
        private static readonly string[] AllowedExtensions = new[] { "tmp", "png", "jpg", "jpeg", "jfif", "gif", "bm", "bmp", "txt", "log" };

        public string FileName { get; private set; }

        private static string GenerateSalt()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 12);
        }

        private static string GetTempFileName(string extension = null)
        {
            if (string.IsNullOrEmpty(extension))
                extension = "tmp";

            extension = extension.Replace(".", string.Empty).Trim().ToLowerInvariant();

            if (AllowedExtensions.Any(p => extension == p) == false)
                extension = "tmp";

            string path = Path.Combine(Path.GetTempPath(), "KissLog", $"{GenerateSalt()}.{extension}");

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

        public TemporaryFile()
        {
            FileName = GetTempFileName(null);
        }

        public TemporaryFile(string extension)
        {
            FileName = GetTempFileName(extension);
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