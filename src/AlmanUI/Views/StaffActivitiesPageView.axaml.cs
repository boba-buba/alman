using Avalonia.Controls;
using Avalonia.Data;

namespace AlmanUI.Views;

/// <summary>
/// View for the staff activities.
/// </summary>
public partial class StaffActivitiesPageView : UserControl, IInitDataGrid
{
    /// <summary>
    /// ctor.
    /// </summary>
    public StaffActivitiesPageView()
    {
        InitializeComponent();
        InitDataGrid();
    }

    public void InitDataGrid()
    {
        StaffActivitiesMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        StaffActivitiesMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.StaffResources.StaffActivityName, Binding = new Binding("ActivityName")});
    }
}