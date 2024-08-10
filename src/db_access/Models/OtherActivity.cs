using Alman.SharedDefinitions;
using Alman.SharedModels;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class OtherActivity : IOtherActivityBase, IDeleteDependable
{

    public int Id { get; set; }

    public string? OtherName { get; set; }

    public virtual ICollection<YearMonthOther> YearMonthOthers { get; set; } = new List<YearMonthOther>();

    public void DeleteDependable(DbContext dbContext)
    {
        AlmanContext ctx = (AlmanContext)dbContext;
        dbContext.RemoveRange(ctx.YearMonthOthers.Where(activity => activity.OtherActivityId == this.Id).ToList());
    }
}
