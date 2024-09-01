using Alman.SharedModels;
using Microsoft.EntityFrameworkCore;
namespace DbAccess.Models;

/// <summary>
/// Database model for the contract fee entity.
/// </summary>
public partial class ContractFee : IContractFeeBase, IDeleteDependable
{
    public int Id { get; set; }
    public int CfchildId { get; set; }

    public int Cfmonth { get; set; }

    public int Cfyear { get; set; }

    public int CfsumPaid { get; set; }

    /// <summary>
    /// Child that the contract fee belong to.
    /// </summary>
    public virtual Child Cfchild { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext) { }
}
