using AutoMapper;
using Instruction.Domain.Core;
using Instruction.Domain.Models;
using Instruction.Domain.Repositories;
using Instruction.Domain.ValueObjects;
using Instruction.Domain.ValueObjects.DTOs.Requests;
using Instruction.Repository.Core;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Instruction.ApplicationService
{
    public class InstructionApplicationService : IInstructionApplicationService
    {
        private readonly IInstructionOrderRepository _instructionOrderRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<OutboxMessage> _genericRepository;
        public InstructionApplicationService(IInstructionOrderRepository instructionOrderRepository, IMapper mapper, IUnitOfWork unitOfWork, IGenericRepository<OutboxMessage> genericRepository)
        {
            _instructionOrderRepository = instructionOrderRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<BaseResponseDto<string>> CancelInstructionOrder(Guid userId)
        {
            var isExistsActiveInstractionOrder = await _instructionOrderRepository.Where(x => x.UserId == userId && x.OrderStatusType == OrderStatusType.Created).FirstOrDefaultAsync();
            if (isExistsActiveInstractionOrder is null)
                return BaseResponseDto<string>.Fail(400, Messages.NO_ACTIVE_INSTRUCTURE);
            isExistsActiveInstractionOrder.OrderStatusType = OrderStatusType.Canceled;
            isExistsActiveInstractionOrder.ActionTime = DateTime.Now;

            _instructionOrderRepository.Update(isExistsActiveInstractionOrder);
            await _unitOfWork.CommitAsync();
            return BaseResponseDto<string>.Success(200, Messages.SUCCESS);
        }

        public async Task<BaseResponseDto<string>> ComplatedInstructionOrderByUserId(Guid userId)
        {
            var isExistsActiveInstractionOrder = await _instructionOrderRepository.Where(x => x.UserId == userId && x.OrderStatusType == OrderStatusType.Created).FirstOrDefaultAsync();
            if (isExistsActiveInstractionOrder is null)
                return BaseResponseDto<string>.Fail(400, Messages.NO_ACTIVE_INSTRUCTURE);

            isExistsActiveInstractionOrder.OrderStatusType = OrderStatusType.Complated;
            isExistsActiveInstractionOrder.ActionTime = DateTime.Now;

            _instructionOrderRepository.Update(isExistsActiveInstractionOrder);

            var outboxMessages = new OutboxMessage { NotificationType = isExistsActiveInstractionOrder.NotificationType,
                Payload = JsonSerializer.Serialize(isExistsActiveInstractionOrder),
                CreatedDate = DateTime.Now                
            };
            await _genericRepository.AddAsync(outboxMessages);
            await _unitOfWork.CommitAsync();
            return BaseResponseDto<string>.Success(200,Messages.SUCCESS);
        }

        public async Task<BaseResponseDto<bool>> CreateInstructionOrder(InstructionOrderCreateRequestDto request)
        {
            var instructionOrderEntity = _mapper.Map<InstructionOrder>(request);
            var isExistsInstractionOrder = await _instructionOrderRepository.AnyAsync(x => x.UserId == request.UserId && x.OrderStatusType == OrderStatusType.Created);
            if (isExistsInstractionOrder)
                return BaseResponseDto<bool>.Fail(400,Messages.EXISTS_ACTIVE_INSTRUCTURE);
            instructionOrderEntity.TransactionTime = DateTime.Now;
            instructionOrderEntity.OrderStatusType = OrderStatusType.Created;
            var result = await _instructionOrderRepository.AddAsync(instructionOrderEntity);
            await _unitOfWork.CommitAsync();
            return BaseResponseDto<bool>.Success(200);
        }

        public async Task<BaseResponseDto<int[]>> GetInstructionNotificationsByUserId(Guid userId)
        {
            var instructionOrders = await _instructionOrderRepository.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (instructionOrders is null)
                return BaseResponseDto<int[]>.Fail(404, Messages.NO_ACTIVE_INSTRUCTURE);
            return BaseResponseDto<int[]>.Success(200, instructionOrders.NotificationType);
        }

        public async Task<BaseResponseDto<InstructionOrder>> GetInstructionOrderByUserId(Guid userId)
        {
            var instructionOrders = await _instructionOrderRepository.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (instructionOrders is null)
                return BaseResponseDto<InstructionOrder>.Fail(404, Messages.NO_ACTIVE_INSTRUCTURE);
            return BaseResponseDto<InstructionOrder>.Success(200, instructionOrders);
        }

        public async Task<BaseResponseDto<List<InstructionOrder>>> GetInstructionOrderList()
        {
            var instructionOrders = await _instructionOrderRepository
                .Where(x => x.OrderStatusType == OrderStatusType.Created)
                .OrderByDescending(y => y.TransactionTime)
                .ToListAsync();
            return BaseResponseDto<List<InstructionOrder>>.Success(200, instructionOrders);
        }
    }
}
