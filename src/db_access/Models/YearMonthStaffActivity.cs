using System;
using System.Collections.Generic;
using Alman.SharedModels;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

public partial class YearMonthStaffActivity : IYearMonthStaffActivityBase, IDeleteDependable
{
    public int Id { get; set; }

    public int StaffMemberId { get; set; }

    public int StaffActivityId { get; set; }

    public decimal? SumPaid { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public virtual StaffActivity StaffActivity { get; set; } = null!;

    public virtual StaffMember StaffMember { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext)
    {

    }
}
