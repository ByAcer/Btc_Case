using Instruction.Domain.Core;

namespace Instruction.Domain.Services.Events;

public class InstructionOrderComplatedEvent : BaseEntity
{
    public Guid UserId { get; set; }
    public int[] NotificationType { get; set; }
    public int OrderStatusType { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionTime { get; set; }
    public DateTime ActionTime { get; set; }
}
