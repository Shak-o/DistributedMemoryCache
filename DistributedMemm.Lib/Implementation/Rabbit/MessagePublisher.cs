using System.Text;
using System.Text.Json;
using DistributedMemm.Infrastructure.Models;
using DistributedMemm.Lib.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;

namespace DistributedMemm.Lib.Implementation.Rabbit;

public class MessagePublisher : IMessagePublisher
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    public MessagePublisher(IConfiguration configuration)
    {
        _configuration = configuration;

        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"])
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("trigger", ExchangeType.Topic);
            _connection.ConnectionShutdown += RabbitMq_ConnectionShutDown;

            Log.Information("Connected To Message Bus");
        }
        catch (Exception ex)
        {
            Log.Error($"Could Not Connect to the Message Bus: {ex}");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish("trigger", "", null, body);
        Log.Information($"We Have sent {message}");
    }

    public void Publish(string key, GenericCacheModel model)
    {
        var json = JsonSerializer.Serialize(new { Key = key, Value = model });
        SendMessage(json);
    }

    private void RabbitMq_ConnectionShutDown(object sender, ShutdownEventArgs e)
    {
        Log.Information("RabbitMQ Connection Shut Down");
    }

    public void Dispose()
    {
        Log.Information("MessageBus Disposed");

        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}