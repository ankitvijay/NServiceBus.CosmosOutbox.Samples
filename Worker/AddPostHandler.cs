using System.Threading.Tasks;
using Messages;
using NServiceBus;
using Worker.Domain;

namespace Worker
{
    public class AddPostHandler : IHandleMessages<AddPost>
    {
        public async Task Handle(AddPost message, IMessageHandlerContext context)
        {
            var cosmosSession = context.SynchronizedStorageSession.CosmosPersistenceSession();
            var post = new Post(message.PostId, message.Title, message.Description, message.Author);
            cosmosSession.Batch.CreateItem(post);
            await context.Publish(new PostCreated
            {
                PostId = post.PostId,
                Author = post.Author,
                Description = post.Description,
                Title = post.Title
            });
        }
    }
}