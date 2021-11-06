using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;
using System.Text;

namespace KissLog.AspNetCore.ReadInputStream
{
    internal class EnableRewindReadInputStreamProvider : IReadInputStreamProvider
    {
        public string ReadInputStream(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Body.CanRead == false)
                return null;

            string content = null;

            // Allows using several time the stream in ASP.Net Core
            request.EnableRewind();

            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                content = reader.ReadToEnd();
            }

            request.Body.Position = 0;

            return content;
        }
    }
}
