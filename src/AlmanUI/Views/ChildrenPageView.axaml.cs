using Alman.SharedModels;
using AlmanUI.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System.Linq;

namespace AlmanUI.Views;

public partial class ChildrenPageView : UserControl
{
    public ChildrenPageView()
    {
        InitializeComponent();
        InitDataGrid();
    }

    private void InitDataGrid()
    {
        ChildrenDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        ChildrenDataGrid.Columns.Add(new DataGridTextColumn { Header = "First Name", Binding = new Binding("ChildName") });
        ChildrenDataGrid.Columns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new Binding("ChildLastName") });
        UIControlElements.AddNumericUpDownToGrid<IChildBase>(ChildrenDataGrid, "Start Year", "ChildStartYear", 2000, 2100);
        UIControlElements.AddNumericUpDownToGrid<IChildBase>(ChildrenDataGrid, "Start Month", "ChildStartMonth", 1, 12);
        UIControlElements.AddCheckBoxToGrid<IChildBase>(ChildrenDataGrid, "Is Active", "ChildState");
        UIControlElements.AddNumericTextBoxToGrid<IChildBase>(ChildrenDataGrid, "Child Group", "ChildGroup");
        UIControlElements.AddComboBoxToDataGrid<IChildBase>(ChildrenDataGrid, "Contract type", "ChildContract" , new ContractTypeConverter());
    }
}