using System;
using Newtonsoft.Json;

namespace Worker.Domain
{
    public class Post
    {
        public Post(string postId,
            string title,
            string description,
            string author)
        {
            Id = postId;
            PostId = postId;
            Title = title;
            Description = description;
            Author = author;
            LastUpdatedDate = DateTime.UtcNow;
            CreatedDate = DateTime.UtcNow;
        }

        [JsonProperty("id")] public string Id { get; set; }
        public string PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}