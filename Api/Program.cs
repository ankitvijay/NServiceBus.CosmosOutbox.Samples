using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NServiceBus.CosmosOutbox.Samples.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Samples.Api");
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    var routing = transport.Routing();
                    routing.RouteToEndpoint(Assembly.Load("Messages"), "Samples.Worker");
                    endpointConfiguration.SendOnly();
                    return endpointConfiguration;
                });
    }
}