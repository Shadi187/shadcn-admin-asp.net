using backend.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TaskItem>()
            .HasOne(t => t.AssignedUser)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<TaskItem>()
            .HasIndex(e=>e.CreatedAt);
    }

    public DbSet<TaskItem> Tasks { get; set; } = null!;
    
    


}