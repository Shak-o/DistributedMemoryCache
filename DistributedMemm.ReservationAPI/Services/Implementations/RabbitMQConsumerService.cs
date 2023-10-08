using System.Text;
using System.Text.Json;
using DistributedMemm.ReservationAPI.Services.Interfaces;
using DistributedMemm.ReservationAPI.Services.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DistributedMemm.ReservationAPI.Services.Implementations
{
    public class RabbitMQConsumerService : IConsumerService
    {
        private readonly ICacheService _cacheService;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQConsumerService(ICacheService cacheService, IConfiguration config)
        {
            _cacheService = cacheService;
            var factory = new ConnectionFactory()
            {
                HostName = config.GetValue<string>("Rabbit:RabbitMQHost"),
                Port = config.GetValue<int>("Rabbit:RabbitMQPort")
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Topic);
            _queueName = _channel.QueueDeclare(autoDelete: false).QueueName;
            _channel.QueueBind(_queueName, exchange: "trigger", routingKey: "rk");
        }


        public void StartConsume()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.Span);
                Consume(message);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        private void Consume(string message)
        {
            var obj = JsonSerializer.Deserialize<EventModel>(message);

            if (obj is { Key: not null, Value: not null, EventType: EventType.Add })
                _cacheService.SaveToCacheKeyValueAsync(obj.Key, obj.Value);
        }
    }
}
