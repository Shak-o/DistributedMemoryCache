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

        public RabbitMQConsumerService(ICacheService cacheService, string hostname, string exchangeName, string queueName, string routingKey)
        {
            _cacheService = cacheService;
            _queueName = queueName;
            
            var factory = new ConnectionFactory() { HostName = hostname };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.ExchangeDeclare(exchange: exchangeName, type: "topic");
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
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
                _cacheService.SaveToCacheKeyValueAsync(obj.Key, obj.Value.ToString()!);
        }
    }
}
