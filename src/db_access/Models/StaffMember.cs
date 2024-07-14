using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class StaffMember
{
    public int StaffMemberId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? PositionId { get; set; }

    public int State { get; set; }

    public virtual ICollection<FinalPayment> FinalPayments { get; set; } = new List<FinalPayment>();

    public virtual Position? Position { get; set; }

    public virtual ICollection<Prepayment> Prepayments { get; set; } = new List<Prepayment>();

    public virtual ICollection<YearMonthOther> YearMonthOthers { get; set; } = new List<YearMonthOther>();

    public virtual ICollection<YearMonthStaffActivity> YearMonthStaffActivities { get; set; } = new List<YearMonthStaffActivity>();
}
