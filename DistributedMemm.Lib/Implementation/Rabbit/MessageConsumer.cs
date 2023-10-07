using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using DistributedMemm.Lib.Interfaces;

namespace DistributedMemm.Lib.Implementation.Rabbit;

public class MessageConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;
    private bool _subscribed = false;
    private const int MaxRetryCount = 3;

    public MessageConsumer(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
        TryInternalizeRabbitMQ();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        int retryCount = 0;
        consumer.Received += (ModuleHandle, ea) =>
        {
            Log.Information("Event Recieved!");
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            TryProcessReceivedMessage(ref retryCount, ea, notificationMessage);
        };

        if (_subscribed)
            _channel.BasicConsume(
            queue: _queueName,
            autoAck: false,
            consumer: consumer);
        return Task.CompletedTask;
    }

    private void TryProcessReceivedMessage(ref int retryCount, BasicDeliverEventArgs ea, string notificationMessage)
    {
        try
        {
            _eventProcessor.ProcessEvent(notificationMessage);
            _channel.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _channel.BasicNack(ea.DeliveryTag, false, retryCount <= 3);
            retryCount++;
            Log.Error(ex.Message);
        }
    }

    protected void TryInternalizeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration.GetValue<string>("Rabbit:RabbitMQHost"),
            Port = _configuration.GetValue<int>("Rabbit:RabbitMQPort"),
            VirtualHost = "memm"
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("trigger", ExchangeType.Topic);
            _queueName = _channel.QueueDeclare(autoDelete: false).QueueName;
            _channel.QueueBind(_queueName, exchange: "trigger", routingKey: RoutingKeyGenerator.Instance.Key);
            _subscribed = true;
            Log.Information("Listening on Message Bus");
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
        }
        catch (Exception ex)
        {
            _subscribed = false;
            Log.Error(ex, "Could not subscribe to Message Bus");
        }
    }

    private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
    {
        Log.Information("Connection Shutdown");
    }

    public override void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
        base.Dispose();
    }
}