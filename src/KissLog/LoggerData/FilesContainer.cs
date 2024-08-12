using KissLog.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KissLog.LoggerData
{
    internal class FilesContainer : IDisposable
    {
        internal readonly List<TemporaryFile> _temporaryFiles;
        private readonly List<LoggedFile> _loggedFiles;
        private readonly Logger _logger;

        internal bool _disposed = false;

        public FilesContainer(Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _temporaryFiles = new List<TemporaryFile>();
            _loggedFiles = new List<LoggedFile>();
        }

        public LoggedFile LogAsFile(string contents, string fileName = null)
        {
            if (string.IsNullOrEmpty(contents))
                return null;

            fileName = NormalizeFileName(fileName);
            long fileSize = contents.Length;

            if(fileSize > Constants.MaximumAllowedFileSizeInBytes)
            {
                _logger.Debug(new FileSizeTooLargeException(fileSize, Constants.MaximumAllowedFileSizeInBytes).ToString());
                return null;
            }

            TemporaryFile temporaryFile = null;
            try
            {
                temporaryFile = new TemporaryFile();
                File.WriteAllText(temporaryFile.FileName, contents);

                LoggedFile loggedFile = new LoggedFile(fileName, temporaryFile.FileName, fileSize);

                _temporaryFiles.Add(temporaryFile);
                _loggedFiles.Add(loggedFile);

                return loggedFile;
            }
            catch(Exception ex)
            {
                if (temporaryFile != null)
                    temporaryFile.Dispose();

                _logger.Error(new LogStringAsFileException(contents, ex));

                return null;
            }
        }

        public LoggedFile LogAsFile(byte[] contents, string fileName = null)
        {
            if (contents == null || !contents.Any())
                return null;

            fileName = NormalizeFileName(fileName);
            long fileSize = contents.Length;

            if (fileSize > Constants.MaximumAllowedFileSizeInBytes)
            {
                _logger.Debug(new FileSizeTooLargeException(fileSize, Constants.MaximumAllowedFileSizeInBytes).ToString());
                return null;
            }

            TemporaryFile temporaryFile = null;
            try
            {
                temporaryFile = new TemporaryFile();
                File.WriteAllBytes(temporaryFile.FileName, contents);

                LoggedFile loggedFile = new LoggedFile(fileName, temporaryFile.FileName, fileSize);

                _temporaryFiles.Add(temporaryFile);
                _loggedFiles.Add(loggedFile);

                return loggedFile;
            }
            catch (Exception ex)
            {
                if (temporaryFile != null)
                    temporaryFile.Dispose();

                _logger.Debug(new LogByteArrayAsFileException(contents, ex).ToString());

                return null;
            }
        }

        public LoggedFile LogFile(string sourceFilePath, string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = Path.GetFileName(sourceFilePath);

            fileName = NormalizeFileName(fileName);

            FileInfo fi = new FileInfo(sourceFilePath);

            if(!fi.Exists)
            {
                _logger.Debug(new LogFileException(sourceFilePath, new FileNotFoundException(null, sourceFilePath)).ToString());
                return null;
            }

            if (fi.Length > Constants.MaximumAllowedFileSizeInBytes)
            {
                _logger.Debug(new FileSizeTooLargeException(fi.Length, Constants.MaximumAllowedFileSizeInBytes).ToString());
                return null;
            }

            TemporaryFile temporaryFile = null;
            try
            {
                temporaryFile = new TemporaryFile();
                File.Delete(temporaryFile.FileName);
                File.Copy(sourceFilePath, temporaryFile.FileName, true);
                long length = temporaryFile.GetSize();

                LoggedFile loggedFile = new LoggedFile(fileName, temporaryFile.FileName, length);

                _temporaryFiles.Add(temporaryFile);
                _loggedFiles.Add(loggedFile);

                return loggedFile;
            }
            catch (Exception ex)
            {
                if (temporaryFile != null)
                    temporaryFile.Dispose();

                _logger.Debug(new LogFileException(sourceFilePath, ex).ToString());

                return null;
            }
        }

        internal string NormalizeFileName(string fileName)
        {
            string result = (string.IsNullOrWhiteSpace(fileName) ? $"File {_loggedFiles.Count + 1}" : fileName).Trim();

            result = Constants.FileNameRegex.Replace(result, string.Empty).Trim();
            result = string.IsNullOrWhiteSpace(result) ? $"File {_loggedFiles.Count + 1}" : result;

            return result;
        }

        internal List<LoggedFile> GetLoggedFiles()
        {
            return _loggedFiles.ToList();
        }

        public void Dispose()
        {
            foreach(var file in _temporaryFiles)
            {
                file.Dispose();
            }

            _disposed = true;
        }
    }
}
