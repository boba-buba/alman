using Alman.SharedModels;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class Child : IChildBase, IIdentifier, IDeleteDependable
{
    public int Id { get; set; }

    public string ChildName { get; set; } = null!;

    public string ChildLastName { get; set; } = null!;

    public int ChildContract { get; set; }

    public int ChildGroup { get; set; }

    public int ChildState { get; set; }

    public int ChildStartYear { get; set; }

    public int ChildStartMonth { get; set; }

    public virtual ICollection<ContractFee> ContractFees { get; set; } = new List<ContractFee>();

    public virtual ICollection<Precontract> Precontracts { get; set; } = new List<Precontract>();

    public virtual ICollection<YearMonthActivity> YearMonthActivities { get; set; } = new List<YearMonthActivity>();

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
