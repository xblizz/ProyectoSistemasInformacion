using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SS.Core.Mailer
{
    public sealed class Email
    {
        public Email()
        {
            To = new Collection<string>();
            Cc = new Collection<string>();
            From = string.Empty;
        }

        public string From { get; set; }
        public ICollection<string> To { get; set; }
        public ICollection<string> Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public bool IsHtml { get; set; }

    }
}
