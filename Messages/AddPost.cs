using System;
using NServiceBus;

namespace Messages
{
    public class AddPost : ICommand, IPostIdPartitionKey
    {
        public AddPost(string title, string description, string author)
        {
            Title = title;
            Description = description;
            Author = author;
            PostId = Guid.NewGuid().ToString();
        }        
        public string PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; }
    }
}