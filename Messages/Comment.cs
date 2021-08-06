using System;
using Newtonsoft.Json;
using NServiceBus;

namespace Messages
{
    public class Comment : IMessage, IPostIdPartitionKey
    {
        public Comment(string postId, string content)
        {
            Content = content;
            PostId = postId;
            Id = Guid.NewGuid().ToString();
        }

        [JsonProperty("id")] public string Id { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
    }
}