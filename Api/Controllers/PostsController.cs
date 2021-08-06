using System;
using System.Threading.Tasks;
using Messages;
using Microsoft.AspNetCore.Mvc;

namespace NServiceBus.CosmosOutbox.Samples.Api.Controllers
{
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IMessageSession _messageSession;

        public PostsController(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        [HttpPost]
        [Route("/create")]
        public async Task<IActionResult> Create([FromBody]Post post)
        {
            await _messageSession.Send(new Messages.Post(post.Topic, post.Description));
            return Accepted();
        }

        [HttpPost]
        [Route("/add-comment")]
        public async Task<IActionResult> AddComment([FromBody]Comment comment)
        {
            await _messageSession.Send(new Messages.Comment(comment.PostId, comment.Content));
            return Accepted();
        }

        public record Post(string Topic, string Description);
        public record Comment(string PostId, string Content);
    }
}