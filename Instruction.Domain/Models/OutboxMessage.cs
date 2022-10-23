using Instruction.Domain.Core;
using Instruction.Domain.ValueObjects;

namespace Instruction.Domain.Models
{
    public class OutboxMessage:BaseEntity
    {
        public DateTime ProcessedDate { get; set; }
        public int[] NotificationType { get; set; }
        public string Payload { get; set; }
    }
}
