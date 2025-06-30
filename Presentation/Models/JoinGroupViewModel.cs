namespace Presentation.Models
{
    public class JoinGroupViewModel
    {
        public int GroupId { get; set; }

        // Optional: For display
        public string? GroupName { get; set; }

        // For current user (use from identity or mock for now)
        public string UserId { get; set; } = string.Empty;
    }

}
