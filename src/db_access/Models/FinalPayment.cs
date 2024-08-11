using System;
using System.Collections.Generic;
using Alman.SharedModels;
using DatabaseAccess;
using Microsoft.EntityFrameworkCore;
namespace DbAccess.Models;

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

    public virtual StaffMember StaffMember { get; set; } = null!;

    public void DeleteDependable(DbContext dbContext) { }
}
