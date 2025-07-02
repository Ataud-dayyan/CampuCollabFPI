using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace Data.Model;
public class GroupModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Department { get; set; }
    public string? Faculty { get; set; }
    public string? SchoolLevel { get; set; }

    public string CreatedById { get; set; } = string.Empty;
    public IdentityUser? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<GroupMembership>? Members { get; set; } = new List<GroupMembership>();
    public ICollection<GroupPost> Posts { get; set; } = new List<GroupPost>();

}
