using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Http
{
    public class ResponseProperties
    {
        public IEnumerable<KeyValuePair<string, string>> Headers { get; }
        public long ContentLength { get; }

        internal ResponseProperties(CreateOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.ContentLength < 0)
                throw new ArgumentException(nameof(options.ContentLength));

            Headers = options.Headers?.Where(p => !string.IsNullOrWhiteSpace(p.Key)).ToList() ?? new List<KeyValuePair<string, string>>();
            ContentLength = options.ContentLength;
        }

        internal class CreateOptions
        {
            public IEnumerable<KeyValuePair<string, string>> Headers { get; set; }
            public long ContentLength { get; set; }
        }

        internal ResponseProperties Clone()
        {
            return new ResponseProperties(new CreateOptions
            {
                Headers = Headers,
                ContentLength = ContentLength
            });
        }
    }
}
