using Data.Model;
namespace Presentation.Models;

public class DashboardViewModel
{
    public int ActiveGroups { get; set; }
    public int GroupLeaders { get; set; }
    public int TotalStudents { get; set; }
    public int MyGroups { get; set; }
    public List<ActivityItem> RecentActivities { get; set; } = new();
    public List<TrendingGroupViewModel> TrendingGroups { get; set; } = new();
}

public class ActivityItem
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime Time { get; set; }
}