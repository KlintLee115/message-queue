using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace message_queue
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController(ConcurrentQueue<BasicDeliverEventArgs> messageQueue) : ControllerBase
    {
        private readonly ConcurrentQueue<BasicDeliverEventArgs> _messageQueue = messageQueue;

        [HttpGet("next")]
        public IActionResult GetNextMessage()
        {
            if (_messageQueue.TryDequeue(out var ea))
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                return Ok($"Processed message: {message}");
            }

            return NotFound("No messages in the queue.");
        }
    }
}
