namespace Alman.SharedModels;

/// <summary>
/// Common model for monthly staff activity entity.
/// </summary>
public interface IYearMonthStaffActivityBase : IIdentifier
{
   /// <summary>
   /// Id of the staff member.
   /// </summary>
    public int StaffMemberId { get; set; }

    /// <summary>
    /// Id of the staff activity.
    /// </summary>
    public int StaffActivityId { get; set; }

    /// <summary>
    /// Sum that will be added to the staff member's pay.
    /// </summary>
    public decimal? SumPaid { get; set; }

    /// <summary>
    /// Month during whih the activity took place.
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Year during which the activity took place.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Flag that indicates whether the payment was paid.
    /// </summary>
    public int WasPaid {  get; set; }

}
