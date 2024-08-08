using Alman.SharedModels;
using System;
using System.Collections.Generic;

namespace DbAccess.Models;

public partial class Child : IChildBase
{
    public int ChildId { get; set; }

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
}
