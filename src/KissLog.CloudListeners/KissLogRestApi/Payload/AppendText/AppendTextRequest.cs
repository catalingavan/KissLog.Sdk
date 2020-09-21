using System.Collections.Generic;

namespace KissLog.CloudListeners.KissLogRestApi.Payload.AppendText
{
    internal class AppendTextRequest
    {
        public string SdkName { get; set; }
        public string SdkVersion { get; set; }
        public string OrganizationId { get; set; }
        public string ApplicationId { get; set; }
        public IEnumerable<string> Lines { get; set; }

        public AppendTextRequest()
        {
            Lines = new List<string>();
        }
    }
}
