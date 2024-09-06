using Alman.SharedDefinitions;
using Alman.SharedModels;
using Avalonia.Controls;
using Avalonia.Data;

namespace AlmanUI.Views;

/// <summary>
/// View for the expenses.
/// </summary>
public partial class ExpensesPageView : UserControl, IInitDataGrid, IUpdateDataGridWithoutParams
{
    /// <summary>
    /// ctor that initializes data grid.
    /// </summary>
    public ExpensesPageView()
    {
        InitializeComponent();
        InitDataGrid();
        Mediator.Mediator.Instance.Notify += OnNotify;
    }
    
    /// <summary>
    /// Process notification from view model.
    /// </summary>
    /// <param name="message">Message from view model.</param>
    private void OnNotify(string message)
    {
        if (message == "UpdateExpensesDataGrid") UpdateDataGrid();
    }

    public void UpdateDataGrid()
    {
        ExpensesDataGrid.Columns.Clear();
        InitDataGrid();
    }
    public void InitDataGrid()
    {
        ExpensesDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        ExpensesDataGrid.MinColumnWidth = 100;

        ExpensesDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.OtherResources.ExpenseName, Binding = new Binding("Name") });
        UIControlElements.AddNumericUpDownToGrid<IExpenseBase>(ExpensesDataGrid, AlmanUI.Resources.CommonResources.Year, "Year", (int)YearsBounds.StartYear, (int)YearsBounds.EndYear);
        UIControlElements.AddNumericUpDownToGrid<IExpenseBase>(ExpensesDataGrid, AlmanUI.Resources.CommonResources.Month, "Month", (int)Months.January, (int)Months.December);

        UIControlElements.AddMoneyTextBox<IExpenseBase>(ExpensesDataGrid, "ExpenseSum", "WayOfPaying", AlmanUI.Resources.CommonResources.PaidSum);

    }
}