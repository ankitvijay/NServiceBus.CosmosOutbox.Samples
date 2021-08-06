using System;
using System.Threading.Tasks;
using Messages;
using Microsoft.Azure.Cosmos;
using NServiceBus;

namespace Worker
{
    public class Handler : IHandleMessages<Post>, IHandleMessages<Comment>
    {
        public Task Handle(Post message, IMessageHandlerContext context)
        {
            var cosmosSession = context.SynchronizedStorageSession.CosmosPersistenceSession();
            cosmosSession.Batch.CreateItem(message, new TransactionalBatchItemRequestOptions());
            return Task.CompletedTask;
        }

        public async Task Handle(Comment message, IMessageHandlerContext context)
        {
            var cosmosSession = context.SynchronizedStorageSession.CosmosPersistenceSession();
            var postResource = await cosmosSession.Container.ReadItemAsync<Post>(message.PostId,
                new PartitionKey(message.PostId));
            postResource.Resource.LastModified = DateTime.UtcNow;
            cosmosSession.Batch.CreateItem(message, new TransactionalBatchItemRequestOptions());
            cosmosSession.Batch.UpsertItem(postResource.Resource, new TransactionalBatchItemRequestOptions
            {
                IfMatchEtag = postResource.ETag
            });
        }
    }
}