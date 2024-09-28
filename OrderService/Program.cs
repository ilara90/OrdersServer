using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderContext>(options =>
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("OrderDb"))
);

builder.Services.AddSingleton<IOrderPublisher>(new RabbitMQPublisher("localhost", "order_queue"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
