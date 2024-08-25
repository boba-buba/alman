using Alman.SharedModels;
using DbAccess;
using Microsoft.EntityFrameworkCore;
namespace DbAccess.Models;

public partial class Activity : IActivityBase, IDeleteDependable
{
    public int Id { get; set; }

    public string ActivityName { get; set; } = null!;

    public int ActivityPrice { get; set; }

    public virtual ICollection<YearMonthActivity> YearMonthActivities { get; set; } = new List<YearMonthActivity>();

    public void DeleteDependable(DbContext db)
    {
        AlmanContext ctx = (AlmanContext)db;
        ctx.RemoveRange(ctx.YearMonthActivities.Where(ymAc => ymAc.YmactivityId == Id).ToList());
    }
}
