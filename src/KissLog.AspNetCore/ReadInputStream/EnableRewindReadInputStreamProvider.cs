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
            string content = string.Empty;

            try
            {
                if (request.Body.CanRead == false)
                    return content;

                // Allows using several time the stream in ASP.Net Core
                request.EnableRewind();

                // Arguments: Stream, Encoding, detect encoding, buffer size 
                // AND, the most important: keep stream opened
                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    content = reader.ReadToEnd();
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
