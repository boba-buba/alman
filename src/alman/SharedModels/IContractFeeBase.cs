namespace Alman.SharedModels;

public interface IContractFeeBase : IIdentifier
{
    //public int Id { get; set; }

    public int CfchildId { get; set; }

    public int Cfmonth { get; set; }

    public int Cfyear { get; set; }

    public int? CfsumPaid { get; set; }

}
