using KissLog.Web;
using System;
using System.IO;
using System.Web;

namespace KissLog.AspNet.Web
{
    internal static class WebRequestPropertiesFactory
    {
        public static WebRequestProperties Create(HttpRequest request)
        {
            WebRequestProperties result = new WebRequestProperties();

            if (request == null)
                return result;

            result.StartDateTime = DateTime.UtcNow;
            result.UserAgent = request.UserAgent;
            result.Url = request.Url;
            result.HttpMethod = request.HttpMethod;
            result.HttpReferer = request.UrlReferrer?.AbsolutePath;
            result.RemoteAddress = request.UserHostAddress;
            result.MachineName = GetMachineName(request);

            RequestProperties requestProperties = new RequestProperties();
            result.Request = requestProperties;

            requestProperties.Headers = DataParser.ToDictionary(request.Unvalidated.Headers);
            requestProperties.QueryString = DataParser.ToDictionary(request.Unvalidated.QueryString);
            requestProperties.FormData = DataParser.ToDictionary(request.Unvalidated.Form);
            requestProperties.ServerVariables = DataParser.ToDictionary(request.ServerVariables);
            requestProperties.Cookies = DataParser.ToDictionary(request.Unvalidated.Cookies);

            if (KissLogConfiguration.ShouldReadInputStream(result))
            {
                string inputStream = ReadInputStream(request);
                if (string.IsNullOrEmpty(inputStream) == false)
                {
                    requestProperties.InputStream = inputStream;
                }
            }

            return result;
        }

        private static string GetMachineName(HttpRequest request)
        {
            string machineName = string.Empty;

            try
            {
                machineName = Environment.MachineName;
            }
            catch
            {
                // ignored
            }

            if (string.IsNullOrEmpty(machineName))
            {
                if (!string.IsNullOrEmpty(request.ServerVariables["SERVER_NAME"]))
                {
                    machineName = request.ServerVariables["SERVER_NAME"];
                }
            }

            return machineName;
        }

        private static string ReadInputStream(HttpRequest request)
        {
            string content = string.Empty;
            if (request.InputStream.CanRead == false)
                return content;

            using (MemoryStream ms = new MemoryStream())
            {
                request.InputStream.CopyTo(ms);

                request.InputStream.Position = 0;
                ms.Position = 0;

                using (StreamReader readStream = new StreamReader(ms, request.ContentEncoding))
                {
                    content = readStream.ReadToEndAsync().Result;
                }
            }

            return content;
        }
    }
}
