using Microsoft.AspNetCore.Mvc;
using NotificationModels;
using OrderService.Models;
using OrderService.RabbitMq;
using System.Text.Json;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;
        private readonly IOrderPublisher _publisher;

        public OrdersController(OrderContext context, IOrderPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order, string email)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var notification = new Notification()
            {
                Email = email,
                Content = JsonSerializer.Serialize(order)
            };

            _publisher.PublishOrderCreated(notification);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return order;
        }
    }
}
