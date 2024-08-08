using Alman.SharedDefinitions;
using Alman.SharedModels;
using DatabaseAccess;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business;

public static class BusinessContractFeesApi
{
    public static IReadOnlyList<IContractFeeBase> GetContractFees()
    {
        var db = new DbChildren();
        return db.GetContractFees(cf => true);
    }

    public static IReadOnlyList<IContractFeeBase> GetContractFeesByFilter(Func<IContractFeeBase, bool> filter)
    {
        var db = new DbChildren();
        return db.GetContractFees(filter);
    }

    public static ReturnCode AddContractFees(IReadOnlyList<IContractFeeBase> newContractFees)
    {
        var db = new DbChildren();
        Collection<ContractFee> contractFees = new Collection<ContractFee>();
        foreach (var newContractFee in newContractFees)
        {
            contractFees.Add( new ContractFee
            {
                CfchildId = newContractFee.CfchildId,
                CfsumPaid = newContractFee.CfsumPaid,
                Cfmonth = newContractFee.Cfmonth,
                Cfyear = newContractFee.Cfyear,
            });
        }
        return db.AddContractFees(contractFees);
    }


    public static ReturnCode UpdateContractFees(IReadOnlyList<IContractFeeBase> updatedContractFees)
    {
        var db = new DbChildren();
        int year = updatedContractFees[0].Cfyear;
        int month = updatedContractFees[0].Cfmonth;

        var contractFeesFromDb = db.GetContractFees(cf => cf.Cfyear == year && cf.Cfmonth == month);

        foreach (var updatedContractFee in updatedContractFees)
        {
            var contractFeeToUpdate = contractFeesFromDb
                .SingleOrDefault(cf => cf.CfchildId ==  updatedContractFee.CfchildId);
            if (contractFeeToUpdate == null) continue;
            
            contractFeeToUpdate.CfsumPaid = updatedContractFee.CfsumPaid;
        }
        return db.UpdateContractFees(contractFeesFromDb);
    }
}


