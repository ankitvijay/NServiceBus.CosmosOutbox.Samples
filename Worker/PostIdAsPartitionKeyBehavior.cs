using System;
using System.Threading.Tasks;
using Messages;
using Microsoft.Azure.Cosmos;
using NServiceBus.Persistence.CosmosDB;
using NServiceBus.Pipeline;

namespace Worker
{
    // See: https://github.com/Particular/docs.particular.net/blob/master/samples/cosmosdb/transactions/CosmosDB_2/Server/OrderIdAsPartitionKeyBehavior.cs
    public class PostIdAsPartitionKeyBehavior: Behavior<IIncomingLogicalMessageContext>
    {
        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            if (context.Message.Instance is IPostIdPartitionKey partitionKey)
            {
                var partitionKeyValue = partitionKey.PostId;
                context.Extensions.Set(new PartitionKey(partitionKeyValue));

                return next();
            }

            return next();
        }

        public class Registration : RegisterStep
        {
            public Registration() :
                base(nameof(PostIdAsPartitionKeyBehavior),
                    typeof(PostIdAsPartitionKeyBehavior),
                    "Determines the PartitionKey from the logical message",
                    b => new PostIdAsPartitionKeyBehavior())
            {
                InsertBefore(nameof(LogicalOutboxBehavior));
            }
        }
    }
}