using Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CampusCollabFPI.Data.Models;

namespace Data.Context;

public class EmployeeAppDbContext : IdentityDbContext<ApplicationUser>
{
    public EmployeeAppDbContext(DbContextOptions<EmployeeAppDbContext> options)
        : base(options) {}
     
    public DbSet<GroupModel> Groups { get; set; }
    public DbSet<GroupMembership> GroupMemberships { get; set; }
    public DbSet<GroupPost> GroupPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>();

        modelBuilder.Entity<GroupModel>()
            .HasOne(g => g.CreatedByUser)
            .WithMany()
            .HasForeignKey(g => g.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }



}
