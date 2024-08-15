using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace AlmanUI.Views;

public partial class StaffActivitiesPageView : UserControl
{
    public StaffActivitiesPageView()
    {
        InitializeComponent();
        InitDataGrid();
    }

    private void InitDataGrid()
    {
        StaffActivitiesMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        StaffActivitiesMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Activity Name", Binding = new Binding("ActivityName")});
    }
}