using Alman.SharedModels;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
namespace DbAccess.Models;

public partial class ContractFee : IContractFeeBase, IDeleteDependable
{
    public int Id { get; set; }
    public int CfchildId { get; set; }

    public int Cfmonth { get; set; }

    public int Cfyear { get; set; }

    public int CfsumPaid { get; set; }

    public virtual Child Cfchild { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext) { }
}
