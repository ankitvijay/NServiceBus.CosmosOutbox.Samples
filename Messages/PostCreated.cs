using NServiceBus;

namespace Messages
{
    public class PostCreated : IEvent
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}