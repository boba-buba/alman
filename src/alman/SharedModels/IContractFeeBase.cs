namespace Alman.SharedModels;

/// <summary>
/// Represents common model for the Contract fee entity.
/// </summary>
public interface IContractFeeBase : IIdentifier
{
    /// <summary>
    /// Id of the child.
    /// </summary>
    public int CfchildId { get; set; }

    /// <summary>
    /// Month when the payment was made.
    /// </summary>
    public int Cfmonth { get; set; }

    /// <summary>
    /// Year when the payment was made.
    /// </summary>
    public int Cfyear { get; set; }

    /// <summary>
    /// Sum that was paid.
    /// </summary>
    public int CfsumPaid { get; set; }
}
