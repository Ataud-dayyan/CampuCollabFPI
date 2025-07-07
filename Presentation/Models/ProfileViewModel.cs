using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{

    public class ProfileViewModel : StudentRegisterViewModel
    {
        public string? CurrentGroupName { get; set; }

        [Display(Name = "Avatar")]
        public string? SelectedAvatar { get; set; }

        public List<string> AvatarOptions { get; set; } = new();
    }
}
