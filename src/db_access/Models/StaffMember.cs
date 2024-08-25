using Alman.SharedModels;
using DbAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class StaffMember : IStaffMemberBase, IDeleteDependable
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? PositionId { get; set; }

    public int StartYear { get; set; }

    public int StartMonth { get; set; }

    public int State { get; set; }

    public string? PositionName { get; set; }

    public int PositionSalary { get; set; }

    public virtual ICollection<FinalPayment> FinalPayments { get; set; } = new List<FinalPayment>();

    public virtual ICollection<YearMonthOther> YearMonthOthers { get; set; } = new List<YearMonthOther>();

    public virtual ICollection<YearMonthStaffActivity> YearMonthStaffActivities { get; set; } = new List<YearMonthStaffActivity>();
    
    public void DeleteDependable(DbContext dbContext)
    {
        AlmanContext ctx = (AlmanContext)dbContext;
        ctx.RemoveRange(ctx.FinalPayments.Where(payment => payment.StaffMemberId == Id).ToList());
        ctx.RemoveRange(ctx.YearMonthStaffActivities.Where(activity => activity.StaffMemberId == Id).ToList());
    }
}
