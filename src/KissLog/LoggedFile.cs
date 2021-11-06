using System;

namespace KissLog
{
    public class LoggedFile
    {
        public string FileName { get; }
        public string FilePath { get; }
        public long FileSize { get; }

        internal LoggedFile(string fileName, string filePath, long fileSize)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (fileSize < 0)
                throw new ArgumentException(nameof(fileSize));

            FileName = fileName;
            FilePath = filePath;
            FileSize = fileSize;
        }

        internal LoggedFile Clone()
        {
            return new LoggedFile(FileName, FilePath, FileSize);
        }
    }
}
