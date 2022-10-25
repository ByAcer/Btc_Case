using Instruction.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Instruction.Repository.Core
{
    public class AppDbContext : DbContext
    {
        public DbSet<InstructionOrder> InstructionOrder { get; set; }
        public DbSet<OutboxMessage> OutboxMessage { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder<AppDbContext> optionsBuilder)
        //{
        //    //optionsBuilder.UseInMemoryDatabase("InstructionOrderDb");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstructionOrder>(entity =>
            {
                entity.HasKey(x => x.Id);
            });
            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProcessedDate).IsRequired(false);
            });
        }
    }
}
