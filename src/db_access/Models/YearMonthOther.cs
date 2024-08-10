using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class YearMonthOther : IDeleteDependable
{
    public int OtherActivityId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int? FirstWeek { get; set; }

    public int? SecondWeek { get; set; }

    public int? ThirdWeek { get; set; }

    public int? FourthWeek { get; set; }

    public int? FifthWeek { get; set; }

    public virtual OtherActivity OtherActivity { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext) { }
}
