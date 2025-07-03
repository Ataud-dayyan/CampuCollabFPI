using System.ComponentModel.DataAnnotations;
using Data.Model;

namespace Presentation.Models
{
    public class ProfileViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public string? CurrentGroupName { get; set; }

        [Display(Name = "Avatar")]
        public string? SelectedAvatar { get; set; }

        public List<string> AvatarOptions { get; set; } = new();
    }
}
