using System;
using Newtonsoft.Json;

namespace Worker.Domain
{
    public class Comment
    {
        public Comment(string postId,
            string commentId,
            string content,
            string commentedBy)
        {
            Id = commentId;
            PostId = postId;
            Content = content;
            CommentedBy = commentedBy;
            CreatedDate = DateTime.UtcNow;
        }

        [JsonProperty("id")] public string Id { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
        public string CommentedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}