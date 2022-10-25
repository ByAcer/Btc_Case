using Instruction.Domain.Core;
using Instruction.Domain.ValueObjects;

namespace Instruction.Domain.Models.Events
{
    public class InstructionOrderEvent : BaseEntity
    {
        public Guid UserId { get; set; }
        public NotificationType NotificationType { get; set; }
        public OrderStatusType OrderStatusType { get; set; }
        public decimal Amount { get; set; }
        public DateTime? TransactionTime { get; set; }
        public DateTime? ActionTime { get; set; }
    }
}
