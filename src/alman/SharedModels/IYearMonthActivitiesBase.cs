namespace Alman.SharedModels;

/// <summary>
/// Common model for the monthly child activities expenses entity. 
/// </summary>
public interface IYearMonthActivityBase : IIdentifier
{
    /// <summary>
    /// Id of the child.
    /// </summary>
    public int YmchildId { get; set; }

    /// <summary>
    /// Id of the child activity.
    /// </summary>
    public int YmactivityId { get; set; }

    /// <summary>
    /// Sum that is paid for the activity participation.
    /// </summary>
    public int YmactivitySum { get; set; }

    /// <summary>
    /// Month when the child participated in the activity.
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Year when the child partiipated in the activity.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Way of paing for the activity (Cash or money transfer).
    /// </summary>
    public int YmwayOfPaying { get; set; }

    /// <summary>
    /// Flag that indicates whether the sum was paid or not yet.
    /// </summary>
    public int YmwasPaid { get; set; }
}