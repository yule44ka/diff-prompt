using Microsoft.EntityFrameworkCore;

namespace CodeDiffPrompt.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PromptRecord> PromptRecords => Set<PromptRecord>();
    public DbSet<CodeSnapshot> CodeSnapshots => Set<CodeSnapshot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PromptRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Before)
                .WithMany()
                .HasForeignKey(e => e.BeforeId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasOne(e => e.After)
                .WithMany()
                .HasForeignKey(e => e.AfterId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<CodeSnapshot>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}


