using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

/// <summary>
/// Database model of the staff member entity.
/// </summary>
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

    /// <summary>
    /// Collection of the final payments tha belong to the staff member.
    /// </summary>
    public virtual ICollection<FinalPayment> FinalPayments { get; set; } = new List<FinalPayment>();

    /// <summary>
    /// Collection of the other activities that were carried out by the staff member.
    /// </summary>
    public virtual ICollection<YearMonthOther> YearMonthOthers { get; set; } = new List<YearMonthOther>();

    /// <summary>
    /// Collection of the monthly staff activities.
    /// </summary>
    public virtual ICollection<YearMonthStaffActivity> YearMonthStaffActivities { get; set; } = new List<YearMonthStaffActivity>();
    
    public void DeleteDependable(DbContext dbContext)
    {
        AlmanContext ctx = (AlmanContext)dbContext;
        ctx.RemoveRange(ctx.FinalPayments.Where(payment => payment.StaffMemberId == Id).ToList());
        ctx.RemoveRange(ctx.YearMonthStaffActivities.Where(activity => activity.StaffMemberId == Id).ToList());
    }
}
