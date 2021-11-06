using KissLog.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace KissLog.CloudListeners.RequestLogsListener
{
    internal class Options
    {
        private static readonly string[] NameClaims = { ClaimTypes.Name, ClaimTypes.GivenName, ClaimTypes.Surname, ClaimTypes.Email, "name", "username", "email", "emailaddress" };
        private static readonly string[] EmailAddressClaims = { ClaimTypes.Email, "email", "emailaddress" };
        private static readonly string[] AvatarClaims = { "avatar", "picture", "image" };

        internal HandlersContainer Handlers { get; }

        public Options()
        {
            Handlers = new HandlersContainer();
        }

        internal class HandlersContainer
        {
            public Func<HttpRequest, KissLog.RestClient.Requests.CreateRequestLog.User> CreateUserPayload { get; set; }
            public Func<FlushLogArgs, IEnumerable<string>> GenerateSearchKeywords { get; set; }

            public HandlersContainer()
            {
                CreateUserPayload = (HttpRequest args) =>
                {
                    return new RestClient.Requests.CreateRequestLog.User
                    {
                        Name = args.Properties.Claims.FirstOrDefault(p => NameClaims.Contains(p.Key.ToLowerInvariant())).Value,
                        EmailAddress = args.Properties.Claims.FirstOrDefault(p => EmailAddressClaims.Contains(p.Key.ToLowerInvariant())).Value,
                        Avatar = args.Properties.Claims.FirstOrDefault(p => AvatarClaims.Contains(p.Key.ToLowerInvariant())).Value
                    };
                };

                GenerateSearchKeywords = (FlushLogArgs args) =>
                {
                    var service = new GenerateSearchKeywordsService();
                    return service.GenerateKeywords(args);
                };
            }
        }
    }
}
