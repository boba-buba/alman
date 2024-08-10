using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Controls;

//Delete here not necessary because there is 1 precontract for every child 
// and it cannot be deleted
public static class PrecontractsControl
{
    private static BusinessEntity<Precontract, IPrecontractBase> BusinessLog { get; set; } = new BusinessEntity<Precontract, IPrecontractBase>();

    public static IReadOnlyList<IPrecontractBase> GetPrecontracts() =>
        BusinessLog.GetEntities();

    public static IReadOnlyList<IPrecontractBase> GetPrecontractsByFilter(Func<IPrecontractBase, bool> filter) =>
        BusinessLog.GetEntitiesByFilter(filter);

    public static ReturnCode AddPrecontracts(IReadOnlyList<IPrecontractBase> precontracts) =>
        BusinessLog.AddEntities(precontracts);

    public static ReturnCode UpdatePrecontracts(IReadOnlyList<IPrecontractBase> precontracts) =>
        BusinessLog.UpdateEntities(precontracts);

    public static ReturnCode SavePrecontracts(IReadOnlyList<IPrecontractBase> precontractsToSave)
    {
        ReturnCode retCode = ReturnCode.OK;
        int? year = precontractsToSave[0].PYear;
        int? month = precontractsToSave[0].PMonth;

        var precontractsFromDb = GetPrecontractsByFilter(pr => pr.PYear == year && pr.PMonth == month);
        int dbCount = precontractsFromDb.Count;
        int difference = precontractsToSave.Count - dbCount;

        if (dbCount > 0)
        {
            var updatedPrecontracts = precontractsToSave.Where(pr => pr.InGroup(precontractsFromDb)).ToList();
            retCode = UpdatePrecontracts(updatedPrecontracts);
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
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong with adding {nameof(IPrecontractBase)}");
            }
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
        var itemInGroup = group.SingleOrDefault(pr => pr.DbEquals(item));
        if (itemInGroup == null) { return false; }
        return true;
    }
}