using CampusCollabFPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class GroupPost
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime PostedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;

        public int GroupId { get; set; }
        public GroupModel Group { get; set; } = default!;
    }
}
