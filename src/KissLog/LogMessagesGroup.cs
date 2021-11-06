using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    public class LogMessagesGroup
    {
        public string CategoryName { get; }
        public IEnumerable<LogMessage> Messages { get; }

        internal LogMessagesGroup(string categoryName, IEnumerable<LogMessage> messages)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentNullException(nameof(categoryName));

            if(messages == null)
                throw new ArgumentNullException(nameof(messages));

            CategoryName = categoryName;
            Messages = messages.ToList();
        }

        internal LogMessagesGroup Clone()
        {
            var messages = Messages.Select(p => p.Clone()).ToList();
            return new LogMessagesGroup(CategoryName, messages);
        }
    }
}
