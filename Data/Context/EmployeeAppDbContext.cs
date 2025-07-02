using Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CampusCollabFPI.Data.Models;

namespace Data.Context;

public class EmployeeAppDbContext : IdentityDbContext<IdentityUser>
{
    public EmployeeAppDbContext(DbContextOptions<EmployeeAppDbContext> options)
        : base(options) {}
    //public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<GroupModel> Groups { get; set; }
    public DbSet<GroupMembership> GroupMemberships { get; set; }
    public DbSet<GroupPost> GroupPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<GroupModel>()
        .HasOne(g => g.CreatedByUser)
        .WithMany()
        .HasForeignKey(g => g.CreatedById)
        .OnDelete(DeleteBehavior.Restrict);

    }

}
