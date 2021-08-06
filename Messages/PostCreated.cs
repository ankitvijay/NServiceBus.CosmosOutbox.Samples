using NServiceBus;

namespace Messages
{
    public class PostCreated : IEvent
    {
        public string PostId { get; set; }
    }
}