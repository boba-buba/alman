using Alman.SharedModels;

namespace AlmanUI.Models;

/// <summary>
/// UI model for contract fee entity.
/// </summary>
public class ContractFeeUI : IContractFeeBase
{
    public int Id { get; set; }
    public int CfchildId { get; set; }

    public int Cfmonth { get; set; }

    public int Cfyear { get; set; }

    public int CfsumPaid { get; set; }
}

/// <summary>
/// Utility class for ContractFees View.
/// </summary>
public class ContractFeeCompositeItem
{
    /// <summary>
    /// Child.
    /// </summary>
    public IChildBase? CFchild { get; set; }

    /// <summary>
    /// Child's contract fee for the particular month for the particular year.
    /// </summary>
    public IContractFeeBase? CFcontractFee { get; set; }

}