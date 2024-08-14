using Alman.SharedModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace AlmanUI.Views;

public partial class ActivitiesPageView : UserControl
{
    public ActivitiesPageView()
    {
        InitializeComponent();
        InitDataGrid();
    }

    private void InitDataGrid()
    {
        ActivitiesDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        ActivitiesDataGrid.Columns.Add(new DataGridTextColumn { Header = "Activity Name", Binding = new Binding("ActivityName") });
        UIControlElements.AddNumericTextBoxToGrid<IActivityBase>(ActivitiesDataGrid, "Price", "ActivityPrice");
    }
}