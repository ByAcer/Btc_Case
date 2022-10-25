using Instruction.Domain.Core;
using Instruction.Domain.MessageBroker;
using Instruction.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Instruction.Api.BackgroundServices
{
    public class OutboxMessagePublisherBackgroundService : BackgroundService
    {
        private readonly ILogger<OutboxMessagePublisherBackgroundService> _logger;
        private readonly MessageBrokerPublisher _publisher;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IGenericRepository<OutboxMessage> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OutboxMessagePublisherBackgroundService(ILogger<OutboxMessagePublisherBackgroundService> logger, MessageBrokerPublisher publisher,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _publisher = publisher;
            _serviceScopeFactory = serviceScopeFactory;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _genericRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<OutboxMessage>>();
                _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishEvents(stoppingToken);
                await Task.Delay(5000, stoppingToken);
            }
        }
        private async Task PublishEvents(CancellationToken stoppingToken)
        {
            try
            {
                var outboxList = await GetOutboxList();
                foreach (var model in outboxList)
                {
                    //var model = new InstructionOrder() //For Test
                    //{
                    //    ActionTime = DateTime.Now,
                    //    Amount = 1000,
                    //    CreatedDate = DateTime.Now,
                    //    Id = Guid.NewGuid(),
                    //    NotificationType = 1,
                    //    OrderStatusType = Domain.ValueObjects.OrderStatusType.Complated,
                    //    TransactionTime = DateTime.Now,
                    //    UserId = Guid.NewGuid(),
                    //    UpdatedDate = null
                    //};
                    _publisher.Publish(model);
                    _logger.LogInformation("Message published: {time}", DateTimeOffset.Now);
                    
                    
                    model.UpdatedDate = DateTime.Now;
                    model.ProcessedDate = DateTime.Now;
                    _genericRepository.Update(model);
                    await _unitOfWork.CommitAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await Task.Delay(5000, stoppingToken);
            }
        }
        private async Task<IList<OutboxMessage>> GetOutboxList()
        {
            return await _genericRepository
                .Where(x=>x.UpdatedDate == null)
                .OrderBy(x => x.CreatedDate).ToListAsync();
        }
    }
}
