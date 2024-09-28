using NotificationModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IConnection _connection;
        private IModel _channel;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "order_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var notification = JsonSerializer.Deserialize<Notification>(message);

                    if (notification != null)
                    {
                        _logger.LogInformation($"Send email to: {notification.Email}, order: {notification.Content}");
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing the message");
                }
            };

            _channel.BasicConsume("order_queue", false, consumer);
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
