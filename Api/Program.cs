using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NServiceBus;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNServiceBus(_ =>
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

app.MapPost("/create", async (IMessageSession messageSession, [FromBody] Post post) =>
{
    var addPostCommand = new Messages.AddPost(post.Title, post.Description, post.Author);
    await messageSession.Send(addPostCommand);
    return Results.Accepted(null, new { addPostCommand.PostId });
});

app.MapPost("/add-comment", async (IMessageSession messageSession, [FromBody] Comment comment) =>
{
    var addCommentCommand = new Messages.AddComment(comment.PostId, comment.Content, comment.CommentBy);
    await messageSession.Send(addCommentCommand);
    return Results.Accepted(null, new { addCommentCommand.CommentId });
});

app.Run();

public record Post(string Title, string Description, string Author);
public record Comment(string PostId, string Content, string CommentBy);