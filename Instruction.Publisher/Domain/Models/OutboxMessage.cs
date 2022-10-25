namespace Instruction.Publisher.Domain.Models;

public class OutboxMessage : BaseEntity
{
    public DateTime? ProcessedDate { get; set; }
    public int? NotificationType { get; set; }
    public string Payload { get; set; }
}
