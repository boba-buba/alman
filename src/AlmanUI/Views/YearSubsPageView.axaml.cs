using Alman.SharedModels;
using AlmanUI.Models;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using System.Collections.Generic;
using System.Linq;

namespace AlmanUI.Views;

public partial class YearSubsPageView : UserControl, IInitDataGrid, IUpdateDataGridWithoutParams
{
    /// <summary>
    /// ctor.
    /// </summary>
    public YearSubsPageView()
    {
        InitializeComponent();
        InitDataGrid();
        Mediator.Mediator.Instance.Notify += OnNotify;
    }

    /// <summary>
    /// Process notification that came from the Mediator.
    /// </summary>
    /// <param name="message">Message from view model.</param>
    /// <param name="year">1st param.</param>

    private void OnNotify(string message)
    {
        if (message == "UpdateYearSubsMainDataGrid") UpdateDataGrid();
    }

    public void InitDataGrid()
    {
        YearSubsMainDataGrid.MinColumnWidth = 150;

        YearSubsMainDataGrid.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Child name",
                Binding = new Binding("YsChild.ChildName"),
                IsReadOnly = true,
            });

        YearSubsMainDataGrid.Columns.Add(
            new DataGridTextColumn
            {
                Header = "Child Lastname",
                Binding = new Binding("YsChild.ChildLastName"),
                IsReadOnly = true,
            });
        
        var months = new List<string> {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

        foreach (var month in months)
        {
            var moneyTemplate = new FuncDataTemplate<YearSubCompositeItem>((x, _) =>
            {
                Grid cellGrid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions
                    {
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Star)
                    }
                };
                int indexMonth = months.IndexOf(month);
                IYearSubBase subscription = x.YsYearSubscriptions.Single(sub => sub.Month == indexMonth + 1);
                int index = x.YsYearSubscriptions.IndexOf(subscription);

                //TextBox
                TextBox moneyTextBox = UIControlElements.CreateNumericTextBox($"YsYearSubscriptions[{index}].Payment");
                //CombBox
                ComboBox wayOfPayCombobox = UIControlElements.CreateComboBox(new WayOfPayingConverter(), $"YsYearSubscriptions[{index}].WayOfaying");
                wayOfPayCombobox.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                //Adding to cellGrid
                cellGrid.Children.Add(moneyTextBox);
                Grid.SetColumn(moneyTextBox, 0);
                cellGrid.Children.Add(wayOfPayCombobox);
                if (x.YsChild.Id == 0) { wayOfPayCombobox.IsVisible = false; }
                Grid.SetColumn(wayOfPayCombobox, 1);
                
                return cellGrid;
            });

            YearSubsMainDataGrid.Columns.Add(new DataGridTemplateColumn
            {
                Header = AlmanUI.Resources.CommonResources.ResourceManager.GetString(month, AlmanUI.Resources.CommonResources.Culture)!,
                CellTemplate = moneyTemplate,
            });
        }
    }

    public void UpdateDataGrid()
    {
        YearSubsMainDataGrid.Columns.Clear();
        InitDataGrid();
    }
}