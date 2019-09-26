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
            string content = string.Empty;

            try
            {
                if (request.Body.CanRead == false)
                    return content;

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
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error reading Request.InputStream");
                sb.AppendLine(ex.ToString());

                KissLog.Internal.InternalHelpers.Log(sb.ToString(), LogLevel.Error);
            }

            return content;
        }
    }
}
