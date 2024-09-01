using Alman.SharedModels;
using Avalonia.Controls;
using Avalonia.Data;

namespace AlmanUI.Views;

/// <summary>
/// View for monthly otehr expenses.
/// </summary>
public partial class YearMonthOtherPageView : UserControl, IInitDataGrid, IUpdateDataGridWithoutParams
{
    /// <summary>
    /// ctor.
    /// </summary>
    public YearMonthOtherPageView()
    {
        InitializeComponent();
        InitDataGrid();
        
        Mediator.Mediator.Instance.Notify += OnNotify;
    }

    /// <summary>
    /// Process notification from view model.
    /// </summary>
    /// <param name="message"></param>
    private void OnNotify(string message)
    {
        if (message == "UpdateYearMonthOtherDataGrid") UpdateDataGrid();
    }

    public void UpdateDataGrid()
    {
        YearMonthOtherMainDataGrid.Columns.Clear();
        InitDataGrid();
    }

    public void InitDataGrid()
    {
        YearMonthOtherMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        
        YearMonthOtherMainDataGrid.MinColumnWidth = 200;

        YearMonthOtherMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.OtherResources.OtherActivityName, Binding = new Binding("OtherActivityName")});

        var weeks = new string[5] { "First", "Second", "Third", "Fourth", "Fifth" };
        for (int i = 0; i < 5; i++)
        {
            string moneyString = AlmanUI.Resources.OtherResources.ResourceManager.GetString($"{weeks[i]}Week", AlmanUI.Resources.OtherResources.Culture)!;
            string PayWay = $"PayingWay{weeks[i]}";
            UIControlElements.AddMoneyTextBox<IYearMonthOtherBase>(YearMonthOtherMainDataGrid, $"{weeks[i]}Week", PayWay, moneyString);
        }
    }

}