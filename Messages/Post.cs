using System;
using Newtonsoft.Json;
using NServiceBus;

namespace Messages
{
    public interface IPostIdPartitionKey
    {
        string PostId { get; set; }
    }

    public class Post : IMessage, IPostIdPartitionKey
    {
        public Post(string topic, string description)
        {
            Topic = topic;
            Description = description;
            Id = Guid.NewGuid().ToString();
            PostId = Id;
            LastModified = DateTime.UtcNow;
        }

        [JsonProperty("id")] public string Id { get; set; }
        public string PostId { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
    }
}