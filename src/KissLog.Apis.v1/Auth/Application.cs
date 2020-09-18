using System;

namespace KissLog.Apis.v1.Auth
{
    [Obsolete("Install KissLog.CloudListeners NuGet package and replace with Application(). https://docs.kisslog.net/SDK/change-log/index.html#kisslog-4-0-0", true)]
    public class Application
    {
        public string OrganizationId { get; }
        public string ApplicationId { get; }

        public Application(string organizationId, string applicationId)
        {
            OrganizationId = organizationId;
            ApplicationId = applicationId;
        }
    }
}
