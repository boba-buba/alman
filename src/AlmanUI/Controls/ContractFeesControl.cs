using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlmanUI.Controls;

public static class ContractFeesControl
{
    private static BusinessEntity<ContractFee, IContractFeeBase> BusinessLog { get; set; } = new BusinessEntity<ContractFee, IContractFeeBase>();

    public static IReadOnlyList<IContractFeeBase> GetContractFees() =>
        BusinessLog.GetEntities();

    public static IReadOnlyList<IContractFeeBase> GetContractFeesByFilter(Func<IContractFeeBase, bool> filter) =>
        BusinessLog.GetEntitiesByFilter(filter);

    public static ReturnCode AddContractFees(IReadOnlyList<IContractFeeBase> contractFees) =>
        BusinessLog.AddEntities(contractFees);

    public static ReturnCode UpdateContractFees(IReadOnlyList<IContractFeeBase> contractFees) =>
        BusinessLog.UpdateEntities(contractFees);

    public static ReturnCode SaveContractFees(IReadOnlyList<IContractFeeBase> contractFeesToSave)
    {
        ReturnCode retCode = ReturnCode.OK;
        int year = contractFeesToSave[0].Cfyear;
        int month = contractFeesToSave[0].Cfmonth;

        var contractFeesFromDb = GetContractFeesByFilter(cf => cf.Cfyear == year && cf.Cfmonth == month);
        int dbCount = contractFeesFromDb.Count;
        int difference = contractFeesToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedContractFees = contractFeesToSave.Where(cf => cf.InGroup(contractFeesFromDb)).ToList();
            retCode = UpdateContractFees(updatedContractFees);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IContractFeeBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newContractFees = contractFeesToSave.Where(cf => !cf.InGroup(contractFeesFromDb)).ToList();
            retCode = AddContractFees(newContractFees);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IContractFeeBase)}");
            }
        }

        return retCode;
    }
}


public static class ContractFeesExtensions
{
    public static bool DbEquals(this IContractFeeBase item, IContractFeeBase other)
    {
        if (item.CfchildId != other.CfchildId) { return false; }
        if (item.Cfmonth != other.Cfmonth) { return false;}
        if (item.Cfyear != other.Cfyear) { return false;}
        return true;
    }

    public static bool InGroup(this IContractFeeBase item, IReadOnlyList<IContractFeeBase> group)
    {
        if (group.Count == 0) { return false; }
        var itemInGroup = group.SingleOrDefault(cf => cf.DbEquals(item));
        if (itemInGroup == null) {  return false; }
        return true;
    }
}