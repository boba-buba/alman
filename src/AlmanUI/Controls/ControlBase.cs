using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using DbAccess;
using System;
using System.Collections.Generic;
namespace AlmanUI.Controls;


public abstract class ControlBase<TEntity, TIface>
    where TEntity : class, TIface, IIdentifier, IDeleteDependable, new()
    where TIface : class, IIdentifier
{
    private static BusinessEntity<TEntity, TIface> BusinessLog { get; set; } = new BusinessEntity<TEntity, TIface>();

    public static IReadOnlyList<TIface> GetItems() =>
        BusinessLog.GetItems();

    public static IReadOnlyList<TIface> GetItemsByFilter(Func<TIface, bool> filter) =>
        BusinessLog.GetItemsByFilter(filter);

    public static ReturnCode AddItems(IReadOnlyList<TIface> precontracts) =>
        BusinessLog.AddItems(precontracts);

    public static ReturnCode UpdateItems(IReadOnlyList<TIface> precontracts) =>
        BusinessLog.UpdateItems(precontracts);

    public static ReturnCode DeleteItems(IList<int> idsToDelete) =>
        BusinessLog.DeleteItems(idsToDelete);

    public static TIface? GetItemById(int id) =>
        BusinessLog.GetItemById(id);
}
