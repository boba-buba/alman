using Alman.SharedModels;
using Avalonia.Controls;
using Avalonia.Data;

namespace AlmanUI.Views;

public partial class ChildrenPageView : UserControl, IInitDataGrid
{
    /// <summary>
    /// ctor that initializes data grid.
    /// </summary>
    public ChildrenPageView()
    {
        InitializeComponent();
        InitDataGrid();
    }

    public void InitDataGrid()
    {
        ChildrenDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        ChildrenDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.CommonResources.FirstName, Binding = new Binding("ChildName") });
        ChildrenDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.CommonResources.LastName, Binding = new Binding("ChildLastName") });
        UIControlElements.AddNumericUpDownToGrid<IChildBase>(ChildrenDataGrid, AlmanUI.Resources.CommonResources.StartYear, "ChildStartYear", 2000, 2100);
        UIControlElements.AddNumericUpDownToGrid<IChildBase>(ChildrenDataGrid, AlmanUI.Resources.CommonResources.StartMonth, "ChildStartMonth", 1, 12);
        UIControlElements.AddCheckBoxToGrid<IChildBase>(ChildrenDataGrid, AlmanUI.Resources.CommonResources.IsActive, "ChildState");
        UIControlElements.AddNumericTextBoxToGrid<IChildBase>(ChildrenDataGrid, AlmanUI.Resources.ChildrenResources.ChildGroup, "ChildGroup");
        UIControlElements.AddComboBoxToDataGrid<IChildBase>(ChildrenDataGrid, AlmanUI.Resources.ChildrenResources.ContractType, "ChildContract" , new ContractTypeConverter());
    }
}