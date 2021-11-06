using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KissLog.CloudListeners.RequestLogsListener
{
    internal class ObfuscateFlushLogArgsService
    {
        internal const string Placeholder = "***obfuscated***";

        private readonly IObfuscationService _obfuscationService;
        public ObfuscateFlushLogArgsService(IObfuscationService obfuscationService)
        {
            _obfuscationService = obfuscationService;
        }

        public void Obfuscate(FlushLogArgs flushLogArgs)
        {
            if (flushLogArgs == null)
                throw new ArgumentNullException(nameof(flushLogArgs));

            if (_obfuscationService == null)
                return;

            Http.RequestProperties requestProperties = CreateRequestProperties(flushLogArgs);
            Http.ResponseProperties responseProperties = CreateResponseProperties(flushLogArgs);

            flushLogArgs.HttpProperties.Request.SetProperties(requestProperties);
            flushLogArgs.HttpProperties.Response.SetProperties(responseProperties);
        }

        private Http.RequestProperties CreateRequestProperties(FlushLogArgs flushLogArgs)
        {
            var headers = Obfuscate(flushLogArgs.HttpProperties.Request.Properties.Headers, GetPropertyName(p => p.HttpProperties.Request.Properties.Headers));
            var cookies = Obfuscate(flushLogArgs.HttpProperties.Request.Properties.Cookies, GetPropertyName(p => p.HttpProperties.Request.Properties.Cookies));
            var formData = Obfuscate(flushLogArgs.HttpProperties.Request.Properties.FormData, GetPropertyName(p => p.HttpProperties.Request.Properties.FormData));
            var serverVariables = Obfuscate(flushLogArgs.HttpProperties.Request.Properties.ServerVariables, GetPropertyName(p => p.HttpProperties.Request.Properties.ServerVariables));
            var claims = Obfuscate(flushLogArgs.HttpProperties.Request.Properties.Claims, GetPropertyName(p => p.HttpProperties.Request.Properties.Claims));

            return new Http.RequestProperties(new Http.RequestProperties.CreateOptions
            {
                Headers = headers,
                Cookies = cookies,
                QueryString = flushLogArgs.HttpProperties.Request.Properties.QueryString,
                FormData = formData,
                ServerVariables = serverVariables,
                Claims = claims,
                InputStream = flushLogArgs.HttpProperties.Request.Properties.InputStream
            });
        }

        private Http.ResponseProperties CreateResponseProperties(FlushLogArgs flushLogArgs)
        {
            var headers = Obfuscate(flushLogArgs.HttpProperties.Response.Properties.Headers, GetPropertyName(p => p.HttpProperties.Response.Properties.Headers));

            return new Http.ResponseProperties(new Http.ResponseProperties.CreateOptions
            {
                ContentLength = flushLogArgs.HttpProperties.Response.Properties.ContentLength,
                Headers = headers
            });
        }

        private List<KeyValuePair<string, string>> Obfuscate(IEnumerable<KeyValuePair<string, string>> values, string propertyName)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach(var item in values)
            {
                bool shouldObfuscate = _obfuscationService.ShouldObfuscate(item.Key, item.Value, propertyName);
                if(shouldObfuscate)
                {
                    result.Add(new KeyValuePair<string, string>(item.Key, Placeholder));
                }
                else
                {
                    result.Add(item);
                }
            }

            return result;
        }

        internal string GetPropertyName(Expression<Func<FlushLogArgs, object>> expression)
        {
            var body = expression.ToString();
            body = body.Substring(body.IndexOf(".", StringComparison.Ordinal) + 1);
            body = body.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            return body;
        }
    }
}
