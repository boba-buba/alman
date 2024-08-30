namespace Alman.SharedModels;

/// <summary>
/// Common model for the monthly other expenses entity.
/// </summary>
public interface IYearMonthOtherBase : IIdentifier
{
    /// <summary>
    /// Name of the expense.
    /// </summary>
    public string OtherActivityName { get; set; }

    /// <summary>
    /// Month during which the expense occurred.
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Year during which the expense occurred.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Sum for the first week.
    /// </summary>
    public int? FirstWeek { get; set; }

    /// <summary>
    /// Way of the paying for the expense for the first week.
    /// </summary>
    public int PayingWayFirst { get; set; }

    /// <summary>
    /// Sum for the second week.
    /// </summary>
    public int? SecondWeek { get; set; }

    /// <summary>
    /// Way of the paying for the expense for the second week.
    /// </summary>
    public int PayingWaySecond { get; set; }

    /// <summary>
    /// Sum for the third week.
    /// </summary>
    public int? ThirdWeek { get; set; }

    /// <summary>
    /// Way of the paying for the expense for the third week.
    /// </summary>
    public int PayingWayThird { get; set; }

    /// <summary>
    /// Sum for the fourth week.
    /// </summary>
    public int? FourthWeek { get; set; }

    /// <summary>
    /// Way of the paying for the expense for the fourth week.
    /// </summary>
    public int PayingWayFourth { get; set; }

    /// <summary>
    /// Sum for the fifth week.
    /// </summary>
    public int? FifthWeek { get; set; }

    /// <summary>
    /// Way of the paying for the expense for the fifth week.
    /// </summary>
    public int PayingWayFifth { get; set; }

}
