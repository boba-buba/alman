using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using DatabaseAccess;
using System;
using System.Collections.Generic;
namespace AlmanUI.Controls;


public abstract class ControlBase<TEntity, TIface>
    where TEntity : class, TIface, IIdentifier, IDeleteDependable
    where TIface : class, IIdentifier
{
    private static BusinessEntity<TEntity, TIface> BusinessLog { get; set; } = new BusinessEntity<TEntity, TIface>();

    public static IReadOnlyList<TIface> GetItems() =>
        BusinessLog.GetEntities();

    public static IReadOnlyList<TIface> GetItemsByFilter(Func<TIface, bool> filter) =>
        BusinessLog.GetItemsByFilter(filter);

    public static ReturnCode AddItems(IReadOnlyList<TIface> precontracts) =>
        BusinessLog.AddEntities(precontracts);

    public static ReturnCode UpdateItems(IReadOnlyList<TIface> precontracts) =>
        BusinessLog.UpdateEntities(precontracts);

    public static ReturnCode DeleteItems(IList<int> idsToDelete) =>
        BusinessLog.DeleteEntities(idsToDelete);
}
