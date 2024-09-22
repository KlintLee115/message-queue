using System.Collections.Concurrent;
using message_queue;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ConcurrentQueue<BasicDeliverEventArgs>>();
builder.Services.AddHostedService<RabbitMQBackgroundService>();

var app = builder.Build();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
