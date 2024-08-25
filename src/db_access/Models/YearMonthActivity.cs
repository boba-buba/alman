using Alman.SharedModels;
using DbAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class YearMonthActivity : IYearMonthActivityBase, IDeleteDependable
{
    public int Id { get; set; }

    public int YmchildId { get; set; }

    public int YmactivityId { get; set; }

    public int YmactivitySum { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int YmwayOfPaying { get; set; }

    public int YmwasPaid { get; set; }

    public virtual Activity Ymactivity { get; set; } = null!;

    public virtual Child Ymchild { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext) { }
}
