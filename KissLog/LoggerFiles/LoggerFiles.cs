using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KissLog
{
    public class LoggerFiles : IDisposable
    {
        private const long MaxFileSizeBytes = 5 * 1024 * 1024;

        private readonly ILogger _logger;
        private readonly List<TemporaryFile> _tempFiles;
        private readonly List<LoggerFile> _files;

        private int _filesCount = 0;

        public LoggerFiles(ILogger logger)
        {
            _logger = logger;

            _tempFiles = new List<TemporaryFile>();
            _files = new List<LoggerFile>();
            _filesCount = 0;
        }

        public void LogFile(string sourceFilePath)
        {
            LogFile(sourceFilePath, $"File {_filesCount++}");
        }
        public void LogFile(string sourceFilePath, string fileName)
        {
            if (!File.Exists(sourceFilePath))
            {
                _logger.Warn($"Could not upload file '{sourceFilePath}' because it does not exist");
                return;
            }

            FileInfo fi = new FileInfo(sourceFilePath);
            if (fi.Length > MaxFileSizeBytes)
            {
                _logger.Warn($"Could not upload file '{sourceFilePath}', because size exceeds {MaxFileSizeBytes} bytes");
                return;
            }

            TemporaryFile tempFile = null;

            try
            {
                tempFile = new TemporaryFile();
                File.Copy(sourceFilePath, tempFile.FileName, true);
                _tempFiles.Add(tempFile);
                LoggerFile file = new LoggerFile(tempFile.FileName, fileName);
                _files.Add(file);
            }
            catch(Exception ex)
            {
                tempFile?.Dispose();
                _logger.Error(ex);
            }
        }

        public void LogAsFile(byte[] content)
        {
            LogAsFile(content, $"File {_filesCount++}");
        }
        public void LogAsFile(byte[] content, string fileName)
        {
            if(content == null || !content.Any())
                return;

            if (content.Length > MaxFileSizeBytes)
            {
                _logger.Warn($"Could not upload file because size exceeds {MaxFileSizeBytes} bytes");
                return;
            }

            TemporaryFile tempFile = null;

            try
            {
                tempFile = new TemporaryFile();
                File.WriteAllBytes(tempFile.FileName, content);
                _tempFiles.Add(tempFile);
                LoggerFile file = new LoggerFile(tempFile.FileName, fileName);
                _files.Add(file);
            }
            catch(Exception ex)
            {
                tempFile?.Dispose();
                _logger.Error(ex);
            }
        }

        public void LogAsFile(string content)
        {
            LogAsFile(content, $"File {_filesCount++}");
        }
        public void LogAsFile(string content, string fileName)
        {
            if (string.IsNullOrEmpty(content))
                return;

            if (content.Length > MaxFileSizeBytes)
            {
                _logger.Warn($"Could not upload file because size exceeds {MaxFileSizeBytes} bytes");
                return;
            }

            TemporaryFile tempFile = null;

            try
            {
                tempFile = new TemporaryFile();
                File.WriteAllText(tempFile.FileName, content);
                _tempFiles.Add(tempFile);
                LoggerFile file = new LoggerFile(tempFile.FileName, fileName);
                _files.Add(file);
            }
            catch (Exception ex)
            {
                tempFile?.Dispose();
                _logger.Error(ex);
            }
        }

        public IEnumerable<LoggerFile> GetFiles()
        {
            return _files.AsReadOnly();
        }

        public void Dispose()
        {
            _filesCount = 0;

            foreach (var file in _tempFiles)
            {
                try
                {
                    file.Dispose();
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
