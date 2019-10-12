namespace KissLog.Web
{
    public class WebProperties
    {
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }

        public WebProperties()
        {
            Request = new HttpRequest();
            Response = new HttpResponse();
        }
    }
}
