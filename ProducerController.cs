using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace message_queue
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendMessage([FromBody] string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port=5672 };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "myQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "myQueue", basicProperties: null, body: body);
            return Ok($"Message sent: {message}");
        }
    }
}