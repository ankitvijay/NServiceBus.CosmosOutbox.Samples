using System;
using NServiceBus;

namespace Messages
{
    public class AddComment : ICommand, IPostIdPartitionKey
    {
        public AddComment(string postId, string content, string commentBy)
        {
            PostId = postId;
            CommentId = Guid.NewGuid().ToString();
            Content = content;
            CommentBy = commentBy;
        }

        public string CommentId { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
        public string CommentBy { get; set; }
    }
}