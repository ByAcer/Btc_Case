using Instruction.Domain.Models;
using Instruction.Domain.Repositories;
using Instruction.Repository.Core;

namespace Instruction.Repository.Repositories
{
    public class InstructionOrderRepository : GenericRepository<InstructionOrder>, IInstructionOrderRepository
    {
        public InstructionOrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}
