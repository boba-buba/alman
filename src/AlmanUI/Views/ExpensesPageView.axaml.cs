using Alman.SharedModels;
using AlmanUI.Models;
using AlmanUI.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using SkiaSharp;

namespace AlmanUI.Views;

public partial class ExpensesPageView : UserControl
{
    public ExpensesPageView()
    {
        InitializeComponent();
        InitDataGrid();
        //Mediator.Mediator.Instance.Notify += OnNotify;
    }

    private void OnNotify(string message)
    {
        //if (message == "UpdateExpensesDataGrid") UpdateDataGrid();
    }

    private void UpdateDataGrid()
    {
        ExpensesDataGrid.Columns.Clear();
        //InitDataGrid();
    }
    private void InitDataGrid()
    {
        ExpensesDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        ExpensesDataGrid.MinColumnWidth = 100;

        ExpensesDataGrid.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name") });
        UIControlElements.AddNumericUpDownToGrid<IExpenseBase>(ExpensesDataGrid, "Year", "Year", 2000, 2100);
        UIControlElements.AddNumericUpDownToGrid<IExpenseBase>(ExpensesDataGrid, "Month", "Month", 1, 12);

        UIControlElements.AddMoneyTextBox<IExpenseBase>(ExpensesDataGrid, "ExpenseSum", "WayOfPaying", "Paid");

    }
}