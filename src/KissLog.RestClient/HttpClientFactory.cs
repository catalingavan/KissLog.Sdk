namespace KissLog.RestClient
{
    internal static class HttpClientFactory
    {
        private static System.Net.Http.HttpClient HttpClient = new System.Net.Http.HttpClient();
        private static System.Net.Http.HttpClient IgnoreSslHttpClient = new System.Net.Http.HttpClient(new System.Net.Http.HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            }
        });

        public static System.Net.Http.HttpClient Create(bool ignoreSslCertificate)
        {
            return ignoreSslCertificate ? IgnoreSslHttpClient : HttpClient;
        }
    }
}
