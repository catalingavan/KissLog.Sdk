namespace KissLog.AspNet.Web
{
    public static class ExtensionMethods
    {
        public static bool WillHandleTheRequest(this ILogger logger)
        {
            return (logger as Logger)?.GetCustomProperty(KissLogHttpModule.IsHandledByHttpModule) != null;
        }
    }
}
