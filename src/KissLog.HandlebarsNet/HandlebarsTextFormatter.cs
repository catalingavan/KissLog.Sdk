using KissLog.Listeners;
using KissLog.Web;
using System;

namespace KissLog.HandlebarsNet
{
    public class HandlebarsTextFormatter : ITextFormatter
    {
        private readonly Func<object, string> _logMessageTemplate;
        private readonly Func<object, string> _webRequestPropertiesTemplate;

        public HandlebarsTextFormatter(
            string logMessageTemplate,
            string webRequestPropertiesTemplate)
        {
            _logMessageTemplate = HandlebarsDotNet.Handlebars.Compile(logMessageTemplate);
            _webRequestPropertiesTemplate = HandlebarsDotNet.Handlebars.Compile(webRequestPropertiesTemplate);
        }

        public string Format(WebRequestProperties webRequestProperties)
        {
            return _webRequestPropertiesTemplate(webRequestProperties);
        }

        public string Format(LogMessage logMessage)
        {
            return _logMessageTemplate(logMessage);
        }
    }
}
