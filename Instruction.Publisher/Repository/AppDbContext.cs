using Instruction.Publisher.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Instruction.Publisher.Repository;

public class AppDbContext : DbContext
{
    public DbSet<OutboxMessage> OutboxMessage { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(a => a.ProcessedDate).IsRequired(false);
        });
    }
}
