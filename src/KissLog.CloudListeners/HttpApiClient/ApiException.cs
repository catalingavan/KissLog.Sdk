namespace KissLog.CloudListeners.HttpApiClient
{
    internal class ApiException
    {
        public int HttpStatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Description { get; set; }
    }
}
