using System;

namespace KissLog.AspNet.Web.Exceptions
{
    internal class NullHttpResponseException : Exception
    {
        public NullHttpResponseException(string httpModuleEvent) : base(ErrorMessage(httpModuleEvent)) { }

        private static string ErrorMessage(string httpModuleEvent)
        {
            return $"{httpModuleEvent}: HttpResponse is null";
        }
    }
}
