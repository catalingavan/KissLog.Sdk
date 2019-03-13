using KissLog.Web;
using System.Net;
using System.Web;

namespace KissLog.AspNet.Web
{
    internal static class WebResponsePropertiesFactory
    {
        public static ResponseProperties Create(HttpResponse response)
        {
            ResponseProperties result = new ResponseProperties();
            if (response == null)
                return result;

            result.HttpStatusCode = (HttpStatusCode)response.StatusCode;
            result.Headers = DataParser.ToDictionary(response.Headers);

            return result;
        }
    }
}
