using Alman.SharedModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace AlmanUI.Views;

public partial class StaffPageView : UserControl
{
    public StaffPageView()
    {
        InitializeComponent();
        InitStaffMainDataGrid();
    }

    private void InitStaffMainDataGrid()
    {
        StaffMembersMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "First Name", Binding = new Binding("FirstName") });
        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new Binding("LastName") });
        UIControlElements.AddCheckBoxToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, "Is Active", "State");
        UIControlElements.AddNumericUpDownToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, "Start month", "StartMonth", 1, 12);
        UIControlElements.AddNumericUpDownToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, "Start year", "StartYear", 2000, 2100);
        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Position Name", Binding = new Binding("PositionName") });
        UIControlElements.AddNumericTextBoxToGrid<IStaffMemberBase>(StaffMembersMainDataGrid, "Position Salary", "PositionSalary");
    }
}