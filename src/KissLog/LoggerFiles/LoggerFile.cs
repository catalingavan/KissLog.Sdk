using System;
using System.IO;

namespace KissLog
{
    public class LoggerFile
    {
        public string FilePath { get; }
        public string FileName { get; }
        public string Extension { get; }

        public string FullFileName => $"{FileName}{Extension}";

        public LoggerFile(string filePath, string fileName)
        {
            if(string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            FilePath = filePath;
            FileName = Path.GetFileNameWithoutExtension(fileName);

            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
            {
                extension = ".txt";
            }

            Extension = extension.ToLowerInvariant();
        }
    }
}
