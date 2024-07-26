using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class YearMonthStaffActivity
{
    public int StaffMemberId { get; set; }

    public int StaffActivityId { get; set; }

    public decimal? SumPaid { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public virtual StaffActivity StaffActivity { get; set; } = null!;

    public virtual StaffMember StaffMember { get; set; } = null!;
}
