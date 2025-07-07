using Microsoft.AspNetCore.Identity;

namespace CampusCollabFPI.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? StudentId { get; set; }
        public string? DisplayName { get; set; }
        
        public string? CurrentGroupName { get; set; }
        public string Avatar { get; set; } = "Avatar1.jpg";
    }
}
