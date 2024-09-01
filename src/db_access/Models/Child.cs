using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

/// <summary>
/// Database model for child entity.
/// </summary>
public partial class Child : IChildBase, IDeleteDependable
{
    public int Id { get; set; }

    public string ChildName { get; set; } = null!;

    public string ChildLastName { get; set; } = null!;

    public int ChildContract { get; set; }

    public int ChildGroup { get; set; }

    public int ChildState { get; set; }

    public int ChildStartYear { get; set; }

    public int ChildStartMonth { get; set; }

    /// <summary>
    /// Collection of contract fees that belong to the child.
    /// </summary>
    public virtual ICollection<ContractFee> ContractFees { get; set; } = new List<ContractFee>();

    /// <summary>
    /// Collection of the precontracts that belong to the child.
    /// </summary>
    public virtual ICollection<Precontract> Precontracts { get; set; } = new List<Precontract>();

    /// <summary>
    /// Collection of monthly activities that the child took part in.
    /// </summary>
    public virtual ICollection<YearMonthActivity> YearMonthActivities { get; set; } = new List<YearMonthActivity>();

    /// <summary>
    /// Collection of yearly child's subscriptions.
    /// </summary>
    public virtual ICollection<YearSub> YearSubs { get; set; } = new List<YearSub>();

    public void DeleteDependable(DbContext dbContext)
    {
        AlmanContext ctx = (AlmanContext)dbContext;
        ctx.RemoveRange(ctx.Precontracts.Where(pr => pr.PchildId == Id).ToList());
        ctx.RemoveRange(ctx.YearMonthActivities.Where(ymAc => ymAc.YmchildId == Id).ToList());
        ctx.RemoveRange(ctx.YearSubs.Where(ys => ys.YchildId == Id).ToList());
        ctx.RemoveRange(ctx.ContractFees.Where(cf => cf.CfchildId == Id).ToList());
    }
}
