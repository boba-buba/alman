using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

public class PrepaymentUI : IPrepaymentBase
{
    public int Id { get; set; }

    public int StaffMemberId { get; set; }

    public int? PaidSum { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int? WasPaid { get; set; }
}


public class PrepaymentCompositeItem
{
    public IStaffMemberBase? StaffMember { get; set; }
    public IPrepaymentBase? Prepayment { get; set; }
}