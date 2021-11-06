using KissLog.ReadStream;
using System;

namespace KissLog.LogResponseBody
{
    internal class LogResponseBody : ILogResponseBodyStrategy
    {
        private readonly Logger _logger;
        private readonly string _responseFileName;
        private readonly IReadStreamStrategy _readStreamStrategy;
        public LogResponseBody(
            Logger logger,
            string responseFileName,
            IReadStreamStrategy readStreamStrategy)
        {
            if (string.IsNullOrWhiteSpace(responseFileName))
                throw new ArgumentNullException(nameof(responseFileName));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _readStreamStrategy = readStreamStrategy ?? throw new ArgumentNullException(nameof(readStreamStrategy));
            _responseFileName = responseFileName;
        }

        public void Execute()
        {
            ReadStreamResult result = _readStreamStrategy.Read();

            if(result.TemporaryFile != null)
            {
                _logger.DataContainer.FilesContainer.LogFile(result.TemporaryFile.FileName, _responseFileName);
            }
            else if(!string.IsNullOrEmpty(result.Content))
            {
                _logger.DataContainer.FilesContainer.LogAsFile(result.Content, _responseFileName);
            }
        }
    }
}
