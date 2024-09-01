namespace Alman.SharedModels;

/// <summary>
/// Common model for yearly results (yearly remainder).
/// </summary>
public interface IYearResultBase : IIdentifier
{
    /// <summary>
    /// Year during for ehich the remainder is.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Sum that remained.
    /// </summary>
    public int YearRemainder { get; set; }
}
