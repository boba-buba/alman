using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;
namespace DbAccess.Models;

/// <summary>
/// Database model for the final payment entity.
/// </summary>
public partial class FinalPayment : IFinalPaymentBase, IDeleteDependable
{
    public int Id { get; set; }

    public int StaffMemberId { get; set; }

    public int? FinalPaymentSum { get; set; }
    public int FinalPaymentWasPaid { get; set; }


    public int Month { get; set; }

    public int Year { get; set; }

    public int PrepaymentSum { get; set; }

    public int? PrepaymentWasPaid { get; set; }

    /// <summary>
    /// Staff member that the final payment belongs to.
    /// </summary>
    public virtual StaffMember StaffMember { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext) { }
}
