using Instruction.Notification.Domain.Models;

namespace Instruction.Notification.Domain.Events;

public class InstructionOrderEvent : BaseEntity
{
    public Guid UserId { get; set; }
    public int NotificationType { get; set; }
    public int OrderStatusType { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionTime { get; set; }
    public DateTime ActionTime { get; set; }
}
