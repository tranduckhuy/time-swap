using Microsoft.AspNetCore.Http;
using MimeKit;

namespace TimeSwap.Application.Dtos.Email
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; } = [];
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFileCollection? Attachments { get; set; }


        public Message(IEnumerable<(string displayName, string email)> to, string subject, string content, IFormFileCollection? attachments)
        {
            To.AddRange(to.Select(x => new MailboxAddress(x.displayName, x.email)));
            Subject = subject;
            Content = content;

            if (attachments is not null)
            {
                Attachments = attachments;
            }
        }
    }
}
