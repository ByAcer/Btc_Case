using Instruction.ApplicationService;
using Instruction.Domain.ValueObjects.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Instruction.Api.Controllers
{
    [Route("api/instruction")]
    public class InstructionController : BaseController
    {
        private readonly IInstructionApplicationService _instructionApplicationService;

        public InstructionController(IInstructionApplicationService instructionApplicationService)
        {
            _instructionApplicationService = instructionApplicationService;
        }

        [HttpGet("{userId:guid}/active")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var response = await _instructionApplicationService.GetInstructionOrderByUserId(userId);
            return CreateActionResult(response);
        }

        [HttpGet("{userId:guid}/notification-channel")]
        public async Task<IActionResult> GetNotification(Guid userId)
        {
            var response = await _instructionApplicationService.GetInstructionNotificationsByUserId(userId);
            return CreateActionResult(response);
        }

        [HttpPost("{userId:guid}/create")]
        public async Task<IActionResult> CreateInstructionOrder(InstructionOrderCreateRequestDto request,Guid userId)
        {
            request.UserId = userId;
            var response = await _instructionApplicationService.CreateInstructionOrder(request);
            return CreateActionResult(response);
        }

        [HttpDelete("{userId:guid}/cancel")]
        public async Task<IActionResult> CancelInstructionOrder(Guid userId)
        {
            var response = await _instructionApplicationService.CancelInstructionOrder(userId);
            return CreateActionResult(response);
        }

        [HttpPut("{userId:guid}/complated")]
        public async Task<IActionResult> CompletedInstructionOrder(Guid userId)
        {
            var response = await _instructionApplicationService.ComplatedInstructionOrderByUserId(userId);
            return CreateActionResult(response);
        }
    }
}
