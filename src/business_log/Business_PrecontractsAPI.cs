using Alman.SharedDefinitions;
using Alman.SharedModels;
using DatabaseAccess;
using DbAccess.Models;
using System.Collections.ObjectModel;

namespace Business;

public static class BusinessPrecontractsApi
{
    public static IReadOnlyList<IPrecontractBase> GetPrecontracts()
    {
        var db = new DbChildren();
        return db.GetPrecontracts(pr => true);
    }

    public static IReadOnlyList<IPrecontractBase> GetPrecontractsByFilter(Func<IPrecontractBase, bool> filter)
    {
        var db = new DbChildren();
        return db.GetPrecontracts(filter);
    }

    public static ReturnCode AddPrecontracts(IReadOnlyList<IPrecontractBase> newPrecontracts)
    {
        var db = new DbChildren();
        Collection<Precontract> precontracts = new Collection<Precontract>();

        foreach (var newPrecontract in newPrecontracts)
        {
            precontracts.Add(new Precontract
            {
                PchildId = newPrecontract.PchildId,
                Psum = newPrecontract.Psum,
                Pcomment = newPrecontract.Pcomment,
                PMonth = newPrecontract.PMonth,
                PYear = newPrecontract.PYear,
            });
        }
        return db.AddPrecontracts(precontracts);
    }

    public static ReturnCode UpdatePrecontracts(IReadOnlyList<IPrecontractBase> updatedPrecontracts)
    {
        var db = new DbChildren();
        var precontractsFromDb = db.GetPrecontracts(pr => true);

        foreach (var updatedPrecontract in updatedPrecontracts)
        {
            var precontractToUpdate = precontractsFromDb
                .Single(pr => pr.PchildId == updatedPrecontract.PchildId);

            precontractToUpdate.Pcomment = updatedPrecontract.Pcomment;
            precontractToUpdate.PMonth = updatedPrecontract.PMonth;
            precontractToUpdate.Psum = updatedPrecontract.Psum;
            precontractToUpdate.PYear = updatedPrecontract.PYear;
        }

        return db.UpdatePrecontracts(precontractsFromDb);
    }
}
