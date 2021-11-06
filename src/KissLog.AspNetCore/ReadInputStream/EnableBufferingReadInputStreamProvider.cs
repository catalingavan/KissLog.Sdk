using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;

namespace KissLog.AspNetCore.ReadInputStream
{
    internal class EnableBufferingReadInputStreamProvider : IReadInputStreamProvider
    {
        public string ReadInputStream(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Body.CanRead == false)
                return null;

            string content = null;

            // Allows using several time the stream in ASP.Net Core
            request.EnableBuffering();

            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                var task = reader.ReadToEndAsync();
                task.Wait();

                content = task.Result;
            }

            request.Body.Position = 0;

            return content;
        }
    }
}
