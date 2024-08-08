using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Models;

public class ContractFeeUI : IContractFeeBase
{
    public int CfchildId { get; set; }

    public int Cfmonth { get; set; }

    public int Cfyear { get; set; }

    public int? CfsumPaid { get; set; }
}

public class ContractFeeCompositeItem
{
    public IChildBase CFchild { get; set; }
    public IContractFeeBase CFcontractFee { get; set; }

}