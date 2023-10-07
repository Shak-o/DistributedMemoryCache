using DistributedMemm.ReservationAPI.Services.Interfaces;

namespace DistributedMemm.ReservationAPI.Services.Implementations
{
    public class RabbitMQHostedService : IHostedService
    {
        private readonly IConsumerService _consumerService;

        public RabbitMQHostedService(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumerService.StartConsume();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
