using System;

namespace KissLog.AspNet.Web.Exceptions
{
    internal class NullResponseFilterException : Exception
    {
        public NullResponseFilterException(string httpModuleEvent) : base(ErrorMessage(httpModuleEvent)) { }

        private static string ErrorMessage(string httpModuleEvent)
        {
            return $"{httpModuleEvent}: Response.Filter is null";
        }
    }
}
