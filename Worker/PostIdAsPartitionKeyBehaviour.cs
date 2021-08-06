using System;
using System.Threading.Tasks;
using Messages;
using Microsoft.Azure.Cosmos;
using NServiceBus.Persistence.CosmosDB;
using NServiceBus.Pipeline;

namespace Worker
{
    // See:  See: https://github.com/Particular/docs.particular.net/blob/master/samples/previews/cosmosdb/transactions/CosmosDB_0/Server/OrderIdAsPartitionKeyBehavior.cs
    public class PostIdAsPartitionKeyBehaviour: Behavior<IIncomingLogicalMessageContext>
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
                base(nameof(PostIdAsPartitionKeyBehaviour),
                    typeof(PostIdAsPartitionKeyBehaviour),
                    "Determines the PartitionKey from the logical message",
                    b => new PostIdAsPartitionKeyBehaviour())
            {
                InsertBefore(nameof(LogicalOutboxBehavior));
            }
        }
    }
}