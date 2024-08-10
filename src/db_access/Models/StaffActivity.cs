using System;
using System.Collections.Generic;
using Alman.SharedModels;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

public partial class StaffActivity : IStaffActivityBase, IDeleteDependable
{
    public int Id { get; set; }

    public string? ActivityName { get; set; }

    public virtual ICollection<YearMonthStaffActivity> YearMonthStaffActivities { get; set; } = new List<YearMonthStaffActivity>();

    public void DeleteDependable(DbContext dbContext)
    {
        AlmanContext ctx = (AlmanContext)dbContext;
        ctx.RemoveRange(ctx.YearMonthStaffActivities.Where(activity => activity.StaffActivityId == Id).ToList());
    }
}
