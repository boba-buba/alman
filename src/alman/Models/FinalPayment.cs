using System;
using System.Collections.Generic;

namespace Alman.Models;

public partial class FinalPayment
{
    public int StaffMemberId { get; set; }

    public int? PaidSum { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int? WasPaid { get; set; }

    public virtual StaffMember StaffMember { get; set; } = null!;
}
