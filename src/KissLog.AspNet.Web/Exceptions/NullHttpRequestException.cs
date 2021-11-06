using System;

namespace KissLog.AspNet.Web.Exceptions
{
    internal class NullHttpRequestException : Exception
    {
        public NullHttpRequestException(string httpModuleEvent) : base(ErrorMessage(httpModuleEvent)) { }

        private static string ErrorMessage(string httpModuleEvent)
        {
            return $"{httpModuleEvent}: HttpRequest is null";
        }
    }
}
