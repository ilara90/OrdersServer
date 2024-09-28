using Microsoft.AspNetCore.Mvc;
using OrderService.Controllers;
using OrderService.Models;
using OrderService.RabbitMq;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using NotificationModels;

namespace OrderService.Test
{
    public class OrdersControllerTests
    {
        private readonly OrderContext _context;
        private readonly IOrderPublisher _publisher;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            var options = new DbContextOptionsBuilder<OrderContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new OrderContext(options);
            _publisher = A.Fake<IOrderPublisher>();
            _controller = new OrdersController(_context, _publisher);
        }

        [Fact]
        public async Task GetOrder_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            int orderId = 1;

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetOrder_ReturnsOrder_WhenOrderExists()
        {
            // Arrange
            int orderId = 100;
            var order = new Order {
                Id = orderId,
                ProductName = "Test",
                Quantity = 1,            
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            var okResult = Assert.IsType<ActionResult<Order>>(result);
            Assert.Equal(order, okResult.Value);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtAction_WhenOrderIsCreated()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                ProductName = "Test",
                Quantity = 1,
            };

            string email = "test@example.com";

            // Act
            var result = await _controller.CreateOrder(order, email);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Order>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal("GetOrder", createdResult.ActionName);
            Assert.Equal(order.Id, ((Order)createdResult.Value).Id);

            // Проверка, что уведомление было отправлено
            A.CallTo(() => _publisher.PublishOrderCreated(A<Notification>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
