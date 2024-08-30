namespace Alman.SharedModels;

/// <summary>
/// Common model for the Final payment entity.
/// </summary>
public interface IFinalPaymentBase : IIdentifier
{
    /// <summary>
    /// Id of the staff member that receives the payments.
    /// </summary>
    public int StaffMemberId { get; set; }

    /// <summary>
    /// Sum that was paid after all prepayments.
    /// </summary>
    public int? FinalPaymentSum { get; set; }

    /// <summary>
    /// Flag that indicates whether the money were given to the staff member or no.
    /// </summary>
    public int FinalPaymentWasPaid { get; set; }

    /// <summary>
    /// Month for which the payments are.
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Year for which the payments are.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Sum of the prepayment, that staff member can ask for.
    /// </summary>
    public int PrepaymentSum { get; set; }

    /// <summary>
    /// Flag that indicates, whether the money were given to the staff member.
    /// </summary>
    public int? PrepaymentWasPaid { get; set; }

}
