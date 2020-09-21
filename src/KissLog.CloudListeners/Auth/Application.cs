namespace KissLog.CloudListeners.Auth
{
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
