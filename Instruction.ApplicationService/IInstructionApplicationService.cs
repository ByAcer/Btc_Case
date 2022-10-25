using Instruction.Domain.Core;
using Instruction.Domain.Models;
using Instruction.Domain.ValueObjects.DTOs.Requests;

namespace Instruction.ApplicationService;

public interface IInstructionApplicationService
{
    Task<BaseResponseDto<InstructionOrder>> GetInstructionOrderByUserId(Guid userId);
    Task<BaseResponseDto<bool>> CreateInstructionOrder(InstructionOrderCreateRequestDto request);
    Task<BaseResponseDto<string>> CancelInstructionOrder(Guid userId);
    Task<BaseResponseDto<string>> GetInstructionNotificationsByUserId(Guid userId);
    Task<BaseResponseDto<string>> ComplatedInstructionOrderByUserId(Guid userId);
}
