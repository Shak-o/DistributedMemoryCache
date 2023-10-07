using DistributedMemm.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Threading.Channels;
using Serilog;
using System.Text;

namespace DistributedMemm.Rabbit;

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

    public async Task PublishAsync(string key, string value, CancellationToken cancellationToken)
    {
        //SendMessage(""); //TODO publish something
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