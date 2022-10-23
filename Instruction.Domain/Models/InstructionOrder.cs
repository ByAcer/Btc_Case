using Instruction.Domain.Core;
using Instruction.Domain.ValueObjects;

namespace Instruction.Domain.Models
{
    public class InstructionOrder : BaseEntity
    {
        public Guid UserId { get; set; }
        public int[] NotificationType { get; set; }
        public OrderStatusType OrderStatusType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionTime { get; set; }
        public DateTime ActionTime { get; set; }
    }
}
