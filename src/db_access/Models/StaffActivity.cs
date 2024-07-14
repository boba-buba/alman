using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class StaffActivity
{
    public int StaffActivityId { get; set; }

    public string? ActivityName { get; set; }

    public virtual ICollection<YearMonthStaffActivity> YearMonthStaffActivities { get; set; } = new List<YearMonthStaffActivity>();
}
