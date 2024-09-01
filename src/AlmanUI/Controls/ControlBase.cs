using Alman.SharedDefinitions;
using Alman.SharedModels;
using Business;
using DbAccess;
using System;
using System.Collections.Generic;
namespace AlmanUI.Controls;

/// <summary>
/// API for the viewmodel. Intermediate layer between business logic and ViewModels.
/// </summary>
public abstract class ControlBase<TEntity, TIface>
    where TEntity : class, TIface, IIdentifier, IDeleteDependable, new()
    where TIface : class, IIdentifier
{
    /// <summary>
    /// Provides functionality for mapping <typeparamref name="TIface"/> to <typeparamref name="TEntity"/>
    /// </summary>
    private static BusinessEntity<TEntity, TIface> BusinessLog { get; set; } = new BusinessEntity<TEntity, TIface>();

    /// <summary>
    /// Get the table.
    /// </summary>
    /// <returns>Read-only list of all rows from the table.</returns>
    public static IReadOnlyList<TIface> GetItems() =>
        BusinessLog.GetItems();

    /// <summary>
    /// Get some of the rows from the table on condition.
    /// </summary>
    /// <param name="filter">Condition by which the rows are chosen.</param>
    /// <returns>Read-only list of chosen rows.</returns>
    public static IReadOnlyList<TIface> GetItemsByFilter(Func<TIface, bool> filter) =>
        BusinessLog.GetItemsByFilter(filter);

    /// <summary>
    /// Add new rows to the table.
    /// </summary>
    /// <param name="items">New rows to add.</param>
    /// <returns>ReturnCode.OK if successfully added, ERR otherwise.</returns>
    public static ReturnCode AddItems(IReadOnlyList<TIface> items) =>
        BusinessLog.AddItems(items);

    /// <summary>
    /// Update rows from the table.
    /// </summary>
    /// <param name="items">Changed rows.</param>
    /// <returns>ReturnCode.OK if successfully added, ERR otherwise.</returns>
    public static ReturnCode UpdateItems(IReadOnlyList<TIface> items) =>
        BusinessLog.UpdateItems(items);

    /// <summary>
    /// Delete rows from the table.
    /// </summary>
    /// <param name="idsToDelete">ids of the rows to delete.</param>
    /// <returns>ReturnCode.OK if successfully added, ERR otherwise.</returns>
    public static ReturnCode DeleteItems(IList<int> idsToDelete) =>
        BusinessLog.DeleteItems(idsToDelete);

    /// <summary>
    /// Get row by row id.
    /// </summary>
    /// <param name="id">id of the row.</param>
    /// <returns>Found row or null if not found.</returns>
    public static TIface? GetItemById(int id) =>
        BusinessLog.GetItemById(id);
}
