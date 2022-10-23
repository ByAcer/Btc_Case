using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Instruction.Domain.Core;

namespace Instruction.Api.Filters;

public class ValidateFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            context.Result = new BadRequestObjectResult(BaseResponseDto<BaseDto>.Fail(400, errors));
        }
    }
}
