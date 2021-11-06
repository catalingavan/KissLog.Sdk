using KissLog.AspNetCore.Tests.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.AspNetCore.Tests
{
    internal static class ExtensionMethods
    {
        // https://github.com/tiagodaraujo/HttpContextMoq/blob/master/src/HttpContextMoq/Extensions/ContextExtensions.cs
        public static void SetUrl(this Mock<HttpRequest> httpRequest, Uri url)
        {
            httpRequest.Setup(x => x.IsHttps).Returns(url.Scheme == "https");
            httpRequest.Setup(x => x.Scheme).Returns(url.Scheme);
            if ((url.Scheme == "https" && url.Port != 443) || (url.Scheme == "http" && url.Port != 80))
            {
                httpRequest.Setup(x => x.Host).Returns(new HostString(url.Host, url.Port));
            }
            else
            {
                httpRequest.Setup(x => x.Host).Returns(new HostString(url.Host));
            }

            httpRequest.Setup(x => x.PathBase).Returns(string.Empty);
            httpRequest.Setup(x => x.Path).Returns(url.AbsolutePath);

            var queryString = QueryString.FromUriComponent(url);
            httpRequest.Setup(x => x.QueryString).Returns(queryString);

            var queryDictionary = QueryHelpers.ParseQuery(queryString.ToString());
            httpRequest.Setup(p => p.Query).Returns(new CustomQueryCollection(queryDictionary));
        }

        public static Dictionary<string, StringValues> ToStringValuesDictionary(this List<KeyValuePair<string, string>> items)
        {
            return items.ToDictionary(p => p.Key, p => new StringValues(p.Value));
        }
    }
}
