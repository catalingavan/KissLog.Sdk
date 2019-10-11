using KissLog.Listeners.TextFileListener;
using KissLog.Web;
using System;

namespace KissLog.HandlebarsNet
{
    public class HandlebarsTextFormatter : ITextFormatter
    {
        private readonly Func<object, string> _logMessageTemplate;
        private readonly Func<object, string> _webRequestPropertiesTemplate;

        public string FormatBeginRequest(HttpRequest httpRequest)
        {
            throw new NotImplementedException();
        }

        public string FormatEndRequest(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            throw new NotImplementedException();
        }

        public string FormatFlush(WebProperties webProperties)
        {
            throw new NotImplementedException();
        }

        public string FormatLogMessage(LogMessage logMessage)
        {
            throw new NotImplementedException();
        }
    }
}
