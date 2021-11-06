using KissLog.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    public class FlushLogArgs
    {
        public HttpProperties HttpProperties { get; }
        public IEnumerable<LogMessagesGroup> MessagesGroups { get; private set; }
        public IEnumerable<CapturedException> Exceptions { get; }
        public IEnumerable<LoggedFile> Files { get; private set; }
        public IEnumerable<KeyValuePair<string, object>> CustomProperties { get; }
        public bool IsCreatedByHttpRequest { get; }

        internal FlushLogArgs(CreateOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.HttpProperties == null)
                throw new ArgumentNullException(nameof(options.HttpProperties));

            if (options.HttpProperties.Response == null)
                throw new ArgumentNullException(nameof(options.HttpProperties.Response));

            HttpProperties = options.HttpProperties;
            MessagesGroups = options.MessagesGroups ?? new List<LogMessagesGroup>();
            Exceptions = options.Exceptions ?? new List<CapturedException>();
            Files = options.Files ?? new List<LoggedFile>();
            CustomProperties = options.CustomProperties ?? new List<KeyValuePair<string, object>>();
            IsCreatedByHttpRequest = options.IsCreatedByHttpRequest;
        }

        internal void SetFiles(IEnumerable<LoggedFile> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            Files = files.ToList();
        }

        internal void SetMessagesGroups(IEnumerable<LogMessagesGroup> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            MessagesGroups = messages.ToList();
        }

        public class CreateOptions
        {
            public HttpProperties HttpProperties { get; set; }
            public List<LogMessagesGroup> MessagesGroups { get; set; }
            public List<CapturedException> Exceptions { get; set; }
            public List<LoggedFile> Files { get; set; }
            public List<KeyValuePair<string, object>> CustomProperties { get; set; }
            public bool IsCreatedByHttpRequest { get; set; }
        }

        internal FlushLogArgs Clone()
        {
            return new FlushLogArgs(new CreateOptions
            {
                HttpProperties = HttpProperties.Clone(),
                MessagesGroups = MessagesGroups.Select(p => p.Clone()).ToList(),
                Exceptions = Exceptions.ToList(),
                Files = Files.Select(p => p.Clone()).ToList(),
                CustomProperties = CustomProperties.ToList(),
                IsCreatedByHttpRequest = IsCreatedByHttpRequest
            });
        }
    }
}
