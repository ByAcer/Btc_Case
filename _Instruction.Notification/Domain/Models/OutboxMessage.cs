namespace Instruction.Notification.Domain.Models;
internal class OutboxMessage : BaseEntity
{
    public DateTime ProcessedDate { get; set; }
    public int NotificationType { get; set; }
    public string Payload { get; set; }
}
