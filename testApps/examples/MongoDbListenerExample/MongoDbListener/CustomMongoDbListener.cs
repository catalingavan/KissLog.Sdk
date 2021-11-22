using KissLog;
using KissLog.Http;
using MongoDB.Driver;
using System;
using System.Linq;

namespace MongoDbListenerExample.MongoDbListener
{
    public class CustomMongoDbListener : ILogListener
    {
        private readonly Lazy<IMongoDatabase> _mongoDatabase;
        public CustomMongoDbListener(string connectionString, string databaseName)
        {
            _mongoDatabase = new Lazy<IMongoDatabase>(() =>
            {
                var mongoClient = new MongoClient(connectionString);
                var mongoDatabase = mongoClient.GetDatabase(databaseName);

                return mongoDatabase;
            });
        }

        public ILogListenerInterceptor Interceptor { get; set; }

        public void OnBeginRequest(HttpRequest httpRequest)
        {
            // do nothing
        }

        public void OnMessage(KissLog.LogMessage message)
        {
            // do nothing
        }

        public void OnFlush(FlushLogArgs args)
        {
            var logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();

            RequestLog requestLog = CreateModel(args.HttpProperties);
            requestLog.Messages = logMessages.Select(p => CreateModel(p)).ToList();

            _mongoDatabase.Value.GetCollection<RequestLog>("RequestLog").InsertOne(requestLog);
        }

        private LogMessage CreateModel(KissLog.LogMessage logMessage)
        {
            return new LogMessage
            {
                Category = logMessage.CategoryName,
                DateTime = logMessage.DateTime,
                LogLevel = logMessage.LogLevel.ToString(),
                Message = logMessage.Message
            };
        }

        private RequestLog CreateModel(HttpProperties httpProperties)
        {
            HttpRequest request = httpProperties.Request;
            HttpResponse response = httpProperties.Response;

            double durationInMs = (response.EndDateTime - request.StartDateTime).TotalMilliseconds;

            return new RequestLog
            {
                DateTime = request.StartDateTime,
                UserAgent = request.UserAgent,
                HttpMethod = request.HttpMethod,
                AbsoluteUri = request.Url.AbsoluteUri,
                RequestHeaders = request.Properties.Headers,
                DurationInMilliseconds = durationInMs,
                StatusCode = response.StatusCode,
                ResponseHeaders = response.Properties.Headers
            };
        }
    }
}
