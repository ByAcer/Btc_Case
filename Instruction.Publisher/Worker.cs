using Instruction.Publisher.Domain.Core;
using Instruction.Publisher.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Instruction.Publisher
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly MessageBroker.Publisher _publisher;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IGenericRepository<OutboxMessage> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Worker(ILogger<Worker> logger, MessageBroker.Publisher publisher,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _publisher = publisher;
            _serviceScopeFactory = serviceScopeFactory;
            var scope = _serviceScopeFactory.CreateScope();
            _genericRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<OutboxMessage>>();
            _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
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
                    _publisher.Publish(model);
                    _logger.LogInformation("Message published: {time}", DateTimeOffset.Now);
                    await EntitySetAsComplated(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task EntitySetAsComplated(OutboxMessage model)
        {
            model.UpdatedDate = DateTime.Now;
            model.ProcessedDate = DateTime.Now;
            _genericRepository.Update(model);
            await _unitOfWork.CommitAsync();
        }

        private async Task<IList<OutboxMessage>> GetOutboxList()
        {
            return await _genericRepository
                .Where(x => x.UpdatedDate == null)
                .OrderBy(x => x.CreatedDate).ToListAsync();
        }
    }
}