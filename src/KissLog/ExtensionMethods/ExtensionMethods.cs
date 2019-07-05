using KissLog.Internal;
using System.Net;

namespace KissLog
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// <para>Sets the KissLog HttpStatusCode regardless of the Response returned by the server</para>
        /// </summary>
        public static void SetHttpStatusCode(this ILogger logger, HttpStatusCode httpStatusCode)
        {
            if (logger is Logger theLogger)
            {
                theLogger.DataContainer.ExplicitHttpStatusCode = httpStatusCode;
            }
        }

        /// <summary>
        /// <para>Explicitly instruct logger to capture the Response Body value</para>
        /// </summary>
        public static void LogResponseBody(this ILogger logger, bool value = true)
        {
            if (logger is Logger theLogger)
            {
                theLogger.DataContainer.AddProperty(Constants.LogResponseBodyProperty, value);
            }
        }

        /// <summary>
        /// <para>Returns true if the ILogger is created and handled automatically by the HttpRequest.</para>
        /// </summary>
        public static bool IsCreatedByHttpRequest(this ILogger logger)
        {
            if (logger is Logger theLogger)
            {
                return theLogger.DataContainer.GetProperty(Constants.IsCreatedByHttpRequestProperty) != null;
            }

            return false;
        }


        public static bool AutoFlush(this ILogger logger)
        {
            if (logger is Logger theLogger)
            {
                return theLogger.DataContainer.GetProperty(Constants.AutoFlushProperty) != null;
            }

            return false;
        }
    }
}
