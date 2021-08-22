using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NServiceBus;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.Api");
    var transport = endpointConfiguration.UseTransport<LearningTransport>();
    var routing = transport.Routing();
    routing.RouteToEndpoint(Assembly.Load("Messages"), "Samples.Worker");
    endpointConfiguration.SendOnly();
    return endpointConfiguration;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Posts Api", Version = "v1" });
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection()
   .UseStaticFiles()
   .UseSwagger()
   .UseSwaggerUI(options => { options.SwaggerEndpoint("v1/swagger.json", "Posts Api"); })
   .UseRouting();

app.MapPost("/create", async(IMessageSession messageSession, [FromBody] Post post) =>
{
    await messageSession.Send(new Messages.Post(post.Topic, post.Description));
    return Results.Accepted();
});

app.MapPost("/add-comment", async (IMessageSession messageSession, [FromBody] Post post) =>
{
    await messageSession.Send(new Messages.Post(post.Topic, post.Description));
    return Results.Accepted();
});

app.Run();

public record Post(string Topic, string Description);
public record Comment(string PostId, string Content);


