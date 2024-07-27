using Alman.SharedModels;
namespace DbAccess.Models;

public partial class Activity : IActivityBase
{
    public int ActivityId { get; set; }

    public string ActivityName { get; set; } = null!;

    public int ActivityPrice { get; set; }

    public virtual ICollection<YearMonthActivity> YearMonthActivities { get; set; } = new List<YearMonthActivity>();
}
