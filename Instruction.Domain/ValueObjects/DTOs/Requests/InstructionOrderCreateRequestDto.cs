using Instruction.Domain.Core;
using System.Text.Json.Serialization;

namespace Instruction.Domain.ValueObjects.DTOs.Requests
{
    public class InstructionOrderCreateRequestDto : BaseDto
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public int NotificationType { get; set; }
        public decimal Amount { get; set; }
    }
}
