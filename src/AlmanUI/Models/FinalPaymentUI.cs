using Alman.SharedModels;

namespace AlmanUI.Models;

/// <summary>
/// UI model of the Final payment entity.
/// </summary>
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

/// <summary>
/// Utility class for FinalPaymentsView.
/// </summary>
public class FinalPayementCompositeItem
{
    /// <summary>
    /// Staff member.
    /// </summary>
    public IStaffMemberBase? StaffMember { get; set; }

    /// <summary>
    /// Staff member's payments for the particular month for the particular year.
    /// </summary>
    public IFinalPaymentBase? FinalPayment { get; set; }
}
