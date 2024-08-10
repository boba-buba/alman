using System;
using System.Collections.Generic;
using Alman.SharedModels;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Models;

public partial class Prepayment : IPrepaymentBase, IDeleteDependable
{
    public int Id { get; set; }

    public int StaffMemberId { get; set; }

    public int? PaidSum { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int? WasPaid { get; set; }

    public virtual StaffMember StaffMember { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext)
    {

    }
}
