using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

public class FinalPaymentUI : IFinalPaymentBase
{
    public FinalPaymentUI(int id, int year, int month)
    {
        StaffMemberId = id;
        FinalPaymentSum = 0;
        FinalPaymentWasPaid = 0;
        PrepaymentSum = 0;
        PrepaymentWasPaid = 0;
        Year = year;
        Month = month;
    }
    public int Id { get; set; }

    public int StaffMemberId { get; set; }

    public int? FinalPaymentSum { get; set; }
    public int FinalPaymentWasPaid { get; set; }


    public int Month { get; set; }

    public int Year { get; set; }

    public int PrepaymentSum { get; set; }

    public int? PrepaymentWasPaid { get; set; }
}

public class FinalPayementCompositeItem
{
    public IStaffMemberBase? StaffMember { get; set; }
    public IFinalPaymentBase? FinalPayment { get; set; }
}
