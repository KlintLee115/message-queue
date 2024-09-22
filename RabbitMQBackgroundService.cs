using System.Collections.Concurrent;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace message_queue
{
    public class RabbitMQBackgroundService(ConcurrentQueue<BasicDeliverEventArgs> messageQueue) : BackgroundService
    {
        private static readonly ConnectionFactory factory = new() { HostName = "localhost", Port = 5672 };
        private readonly IConnection _connection = factory.CreateConnection();
        public readonly ConcurrentQueue<BasicDeliverEventArgs> _messageQueue = messageQueue;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var _channel = _connection.CreateModel(); // Create the channel here
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                _messageQueue.Enqueue(ea);
                Console.WriteLine("New message received and added to the queue.");
            };

            _channel.BasicConsume(queue: "myQueue", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            return base.StopAsync(cancellationToken);
        }
    }
}