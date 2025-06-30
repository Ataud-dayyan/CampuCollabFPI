
using Microsoft.AspNetCore.Identity;

namespace CampusCollabFPI.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
