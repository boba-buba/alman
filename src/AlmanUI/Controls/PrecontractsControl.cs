using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Controls;

public static class PrecontractsControl
{
    public static IReadOnlyList<IPrecontractBase> GetPrecontracts() =>
        BusinessPrecontractsApi.GetPrecontracts();

    public static IReadOnlyList<IPrecontractBase> GetPrecontractsByFilter(Func<IPrecontractBase, bool> filter) =>
        BusinessPrecontractsApi.GetPrecontractsByFilter(filter);


    public static ReturnCode AddPrecontracts(IReadOnlyList<IPrecontractBase> precontracts) =>
        BusinessPrecontractsApi.AddPrecontracts(precontracts);

    public static ReturnCode UpdatePrecontracts(IReadOnlyList<IPrecontractBase> precontracts) =>
        BusinessPrecontractsApi.UpdatePrecontracts(precontracts);

    public static ReturnCode SavePrecontracts(IReadOnlyList<IPrecontractBase> precontractsToSave)
    {
        ReturnCode retCode = ReturnCode.OK;
        var precontractsFromDb = GetPrecontracts();
        int dbCount = precontractsFromDb.Count;
        int difference = precontractsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedPrecontracts = precontractsToSave.Where(pr => pr.InGroup(precontractsFromDb)).ToList();
            retCode = BusinessPrecontractsApi.UpdatePrecontracts(updatedPrecontracts);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with updating {nameof(IPrecontractBase)}");
                return retCode;
            }
        }

        if (difference > 0)
        {
            var newPrecontracts = precontractsToSave.Where(pr => !pr.InGroup(precontractsFromDb)).ToList();
            retCode = AddPrecontracts(newPrecontracts);
        }
        return retCode;
    }
}


public static class PrecontractsBaseExtensions
{
    public static bool DbEquals(this IPrecontractBase item,  IPrecontractBase other)
    {
        if (item.PchildId == other.PchildId) { return true; }
        return false;
    }

    public static bool InGroup(this IPrecontractBase item, IReadOnlyList<IPrecontractBase> group)
    {
        if (group.Count == 0) { return false; }
        var itemInGroup = group.Single(pr => pr.DbEquals(item));
        if (itemInGroup == null) { return false; }
        return true;
    }
}