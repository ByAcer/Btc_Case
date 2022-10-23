using Instruction.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Instruction.Repository.Core
{
    public class AppDbContext : DbContext
    {
        public DbSet<InstructionOrder> InstructionOrders { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder<AppDbContext> optionsBuilder)
        //{
        //    //optionsBuilder.UseInMemoryDatabase("InstructionOrderDb");
        //}
    }
}
