using CampusCollabFPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Data.Model;
public class GroupModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;

    public string? Department { get; set; }
    public string? Faculty { get; set; }
    public string? SchoolLevel { get; set; }

    public string CreatedById { get; set; } = string.Empty;
    public ApplicationUser? CreatedByUser { get; set; }

    public ICollection<GroupMembership>? Members { get; set; } = new List<GroupMembership>();
    public ICollection<GroupPost> Posts { get; set; } = new List<GroupPost>();


}
