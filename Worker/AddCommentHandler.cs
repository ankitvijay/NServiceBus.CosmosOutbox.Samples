using System;
using System.Threading.Tasks;
using Messages;
using Microsoft.Azure.Cosmos;
using NServiceBus;
using Worker.Domain;

namespace Worker
{
    public class AddCommentHandler : IHandleMessages<AddComment>
    {
        public async Task Handle(AddComment message, IMessageHandlerContext context)
        {
            var cosmosSession = context.SynchronizedStorageSession.CosmosPersistenceSession();
            var postResource = await cosmosSession.Container.ReadItemAsync<Post>(message.PostId,
                new PartitionKey(message.PostId));

            if (postResource == null)
            {
                throw new Exception(
                    $"Post {message.PostId} does not exist. Cannot add comment for the post that does not exist");
            }

            postResource.Resource.LastUpdatedDate = DateTime.UtcNow;
            cosmosSession.Batch.UpsertItem(postResource.Resource, new TransactionalBatchItemRequestOptions
            {
                IfMatchEtag = postResource.ETag
            });

            var comment = new Comment(message.PostId, message.CommentId, message.Content,
                message.CommentBy);
            cosmosSession.Batch.CreateItem(comment);

            await context.Publish(new CommentAdded
            {
                PostId = message.PostId,
                CommentId = comment.Id,
                Content = comment.Content,
                CommentBy = comment.CommentedBy,
                CreatedDate = comment.CreatedDate
            });
        }
    }
}