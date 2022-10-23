using Instruction.Domain.Core;

namespace Instruction.Domain.ValueObjects.DTOs.Requests
{
    public class InstructionOrderCreateRequestDto : BaseDto
    {
        public Guid UserId { get; set; }
        public int[] NotificationType { get; set; }
        public decimal Amount { get; set; }
    }
}
