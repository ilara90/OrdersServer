using NotificationModels;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderService.RabbitMq
{
    public class RabbitMQPublisher : IOrderPublisher
    {
        private readonly string _hostName;
        private readonly string _queueName;

        public RabbitMQPublisher(string hostName, string queueName)
        {
            _hostName = hostName;
            _queueName = queueName;
        }

        public void PublishOrderCreated(Notification notification)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = JsonSerializer.Serialize(notification);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: _queueName,
                                     basicProperties: properties,
                                     body: body);
            }
        }
    }
}
