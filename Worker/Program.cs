using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Worker;

await Host.CreateDefaultBuilder(args)
    .UseNServiceBus((_ =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.Worker");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        endpointConfiguration.UsePersistence<CosmosPersistence>()
            // Using Cosmos emulator
            .CosmosClient(new CosmosClient(
                "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))
            .DatabaseName("Twitbook")
            .DefaultContainer("Posts", "/PostId");

        endpointConfiguration.EnableOutbox();
        endpointConfiguration.Pipeline.Register(new PostIdAsPartitionKeyBehavior.Registration());

        endpointConfiguration.EnableInstallers();
        return endpointConfiguration;
    }))
    .Build()
    .RunAsync();