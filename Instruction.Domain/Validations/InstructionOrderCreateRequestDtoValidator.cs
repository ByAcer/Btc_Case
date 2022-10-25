using FluentValidation;
using Instruction.Domain.ValueObjects;
using Instruction.Domain.ValueObjects.DTOs.Requests;

namespace Instruction.Domain.Validations
{
    public class InstructionOrderCreateRequestDtoValidator : AbstractValidator<InstructionOrderCreateRequestDto>
    {
        public InstructionOrderCreateRequestDtoValidator()
        {
            RuleFor(x=>x).Must(IsBetweenAcceptableDays)
                .WithMessage(Messages.IS_BETWEEN_ACCEPTABLE_DAYS_VALIDATION);
            RuleFor(x => x.Amount).InclusiveBetween(100, 20000)
                .WithMessage(Messages.AMOUNT_VALIDATION);
        }
        private bool IsBetweenAcceptableDays(InstructionOrderCreateRequestDto requestDto)
        {
            return (Enumerable.Range(1, 28).Contains(DateTime.Today.Day));
        }
    }
}
