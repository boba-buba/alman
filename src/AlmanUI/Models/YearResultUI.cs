using Alman.SharedModels;

namespace AlmanUI.Models;

/// <summary>
/// UI model of the YearResult entity.
/// </summary>
public class YearResultUI : IYearResultBase
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int YearRemainder { get; set; }
}
