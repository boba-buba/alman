using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.Views;

public interface IInitDataGrid
{
    /// <summary>
    /// Initialize data grid of the table for the UI view.
    /// </summary>
    public void InitDataGrid();
}

public interface IUpdateDataGridWithoutParams
{
    /// <summary>
    /// Get latest data from the database and reload data grid.
    /// </summary>
    public void UpdateDataGrid();
}

public interface IUpdateDataGrid
{
    /// <summary>
    /// Get latest data from the database and reload data grid.
    /// </summary>
    /// <param name="year">Year for which data are collected.</param>
    /// <param name="month">Monthfor which data are collected.</param>
    public void UpdateDataGrid(int year, int month);
}

public interface ILoadItems
{
    /// <summary>
    /// Fetch necessary data from the database and prepare it for the view.
    /// </summary>
    public void LoadItems();
}

public interface ILoadItemsWithParams
{
    /// <summary>
    /// Fetch necessary data from the database and prepare it for the view.
    /// </summary>
    /// <param name="year">Year for which data are fetched.</param>
    /// <param name="month">Month for which data are fetched.</param>
    public void LoadItems(int year, int month);
}
