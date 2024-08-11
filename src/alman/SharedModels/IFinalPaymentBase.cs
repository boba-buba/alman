using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alman.SharedModels;

public interface IFinalPaymentBase : IIdentifier
{
    public int StaffMemberId { get; set; }

    public int? FinalPaymentSum { get; set; }
    public int FinalPaymentWasPaid { get; set; }


    public int Month { get; set; }

    public int Year { get; set; }

    public int PrepaymentSum { get; set; }

    public int? PrepaymentWasPaid { get; set; }

}
