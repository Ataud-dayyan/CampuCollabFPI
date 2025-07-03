using Microsoft.AspNetCore.Identity;

namespace CampusCollabFPI.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public string? Avatar { get; set; }
    }
}
