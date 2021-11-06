using System;
using System.IO;
using System.Linq;

namespace KissLog
{
    internal class TemporaryFile : IDisposable
    {
        internal static readonly string[] AllowedExtensions = new[] { "tmp", "png", "jpg", "jpeg", "jfif", "gif", "bm", "bmp", "txt", "log", "pdf" };
        internal const string DefaultExtension = "tmp";

        public string FileName { get; private set; }

        internal bool _disposed = false;

        private static string GenerateSalt()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 12);
        }

        private static string GetTempFileName(string extension = null)
        {
            if (string.IsNullOrWhiteSpace(extension))
                extension = DefaultExtension;

            extension = extension.Replace(".", string.Empty).Trim().ToLowerInvariant();

            if (AllowedExtensions.Any(p => extension == p) == false)
                extension = DefaultExtension;

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
                    path = Path.Combine(Path.GetTempPath(), $"KissLog_{GenerateSalt()}.{extension}");
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

        public TemporaryFile() : this(null)
        {
            
        }

        public TemporaryFile(string extension)
        {
            FileName = GetTempFileName(extension);
        }

        public long GetSize()
        {
            if (_disposed)
                return 0;

            FileInfo fileInfo = new FileInfo(FileName);
            if(fileInfo.Exists)
                return fileInfo.Length;

            return 0;
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

            _disposed = true;
        }
    }
}
