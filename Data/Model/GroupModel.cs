using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model;
public class GroupModel
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GroupMembership> Members { get; set; } = default!;
}
