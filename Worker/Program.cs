using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messages;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNServiceBus((context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Samples.Worker");
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
                    endpointConfiguration.UsePersistence<CosmosPersistence>()
                        // Using Cosmos emulator
                        .CosmosClient(new CosmosClient(
                            "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))
                        .DatabaseName("Social")
                        .DefaultContainer(
                            "Posts",
                            "/PostId");

                    endpointConfiguration.EnableOutbox();
                    endpointConfiguration.Pipeline.Register(new PostIdAsPartitionKeyBehaviour.Registration());

                    endpointConfiguration.EnableInstallers();
                    return endpointConfiguration;
                }));
    }
}