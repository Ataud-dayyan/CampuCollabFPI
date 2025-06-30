using Microsoft.AspNetCore.Identity;


namespace Data.Model;
public class GroupMembership
{
    public int Id { get; set; }

    public int GroupId { get; set; }
    public GroupModel Group { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public bool IsAdmin { get; set; }
}
