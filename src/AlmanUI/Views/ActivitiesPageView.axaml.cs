using Alman.SharedModels;
using Avalonia.Controls;
using Avalonia.Data;

namespace AlmanUI.Views;

/// <summary>
/// View for Children activities.
/// </summary>
public partial class ActivitiesPageView : UserControl, IInitDataGrid
{
    /// <summary>
    /// ctor that initializes data grid.
    /// </summary>
    public ActivitiesPageView()
    {
        InitializeComponent();
        InitDataGrid();
    }

    public void InitDataGrid()
    {
        ActivitiesDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        ActivitiesDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.ChildrenResources.ChildActivityName, Binding = new Binding("ActivityName") });
        UIControlElements.AddNumericTextBoxToGrid<IActivityBase>(ActivitiesDataGrid, AlmanUI.Resources.ChildrenResources.Price, "ActivityPrice");
    }
}