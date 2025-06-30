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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
