using NServiceBus;

namespace Messages
{
    public class CommentAdded : IEvent
    {
        public string PostId { get; set; }
        public string CommentId { get; set; }
    }
}