using System;
using NServiceBus;

namespace Messages
{
    public class CommentAdded : IEvent
    {
        public string PostId { get; set; }
        public string CommentId { get; set; }
        public string Content { get; set; }
        public string CommentBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}