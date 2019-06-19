using System.Collections.Generic;

namespace KissLog.Apis.v1.Requests
{
    internal class UploadFilesRequest
    {
        public string OrganizationId { get; set; }
        public string ApplicationId { get; set; }
        public string RequestLogId { get; set; }
        public string RequestLogClientId { get; set; }
        public int HttpStatusCode { get; set; }
        public IEnumerable<File> Files { get; set; }

        public UploadFilesRequest()
        {
            Files = new List<File>();
        }
    }
}
