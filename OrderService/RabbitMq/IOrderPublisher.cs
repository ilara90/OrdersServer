using NotificationModels;
using OrderService.Models;

namespace OrderService.RabbitMq
{
    public interface IOrderPublisher
    {
        void PublishOrderCreated(Notification notification);
    }
}
