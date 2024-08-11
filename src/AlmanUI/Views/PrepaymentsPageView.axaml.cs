using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI;
using AlmanUI.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlmanUI.Views;

public partial class PrepaymentsPageView : UserControl
{
    private IReadOnlyList<IStaffMemberBase>? _staffMembersTable;

    private IReadOnlyList<IPrepaymentBase>? _prepaymentsTable;

    private IReadOnlyList<PrepaymentCompositeItem>? _memberPrepayments { get; set; }

    private void LoadItems(int year, int month)
    {
        DateTime now = new(year, month, 1);

        _staffMembersTable = StaffMembersControl.GetItemsByFilter(m => 
            new DateTime(m.StartYear, m.StartMonth, 1) <= now);

        _prepaymentsTable = PrepaymentsControl.GetItemsByFilter(pr => pr.Year == year && pr.Month == month);

        var memberPrepayments = new List<PrepaymentCompositeItem>();

        foreach (var member in _staffMembersTable)
        {
            var newItem = new PrepaymentCompositeItem { StaffMember = member };
            IPrepaymentBase? newItemPrepayment = _prepaymentsTable.SingleOrDefault(pr => pr.StaffMemberId == member.Id);

            if (_prepaymentsTable.Count == 0 || newItemPrepayment is null)
            {
                newItemPrepayment = new PrepaymentUI { Month = month, Year = year, PaidSum = 0, WasPaid = 0, StaffMemberId = member.Id };
            }
            newItem.Prepayment = newItemPrepayment;
            memberPrepayments.Add(newItem);
        }
        _memberPrepayments = memberPrepayments;
    }

    public PrepaymentsPageView()
    {
        LoadItems(DateTime.Now.Year, DateTime.Now.Month);
        if (_memberPrepayments is null)
        {
            _memberPrepayments = new List<PrepaymentCompositeItem>();
        }
        InitializeComponent();
        InitPrepaymentsMainDataGrid();
        Mediator.Mediator.Instance.NotifyWithParams += OnNotifyWithParams;
    }

    private void OnNotifyWithParams(string message, int year, int month)
    {
        if (message == "UpdatePrepaymentsMainDataGrid") 
            UpdatePrepaymentsMainDataGrid(year, month);
    }

    private void InitPrepaymentsMainDataGrid()
    {
        PrepaymentsMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        PrepaymentsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Staff Member Name", Binding = new Binding("StaffMember.FirstName"), IsReadOnly = true });
        PrepaymentsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Staff Member LastName", Binding = new Binding("StaffMember.LastName"), IsReadOnly = true });

        PrepaymentsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "Paid Sum",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("Prepayment.PaidSum", BindingMode.TwoWay)
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new IntToStringConverter(), // Apply the converter here
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

        PrepaymentsMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "Was Paid",  // Example header name
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var checkBox = new CheckBox();

                var binding = new Binding("Precontract.WasPaid")
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new IntToBoolConverter(),  // Apply the converter here
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // Optional: Immediately update the source
                };

                checkBox.Bind(CheckBox.IsCheckedProperty, binding);

                return checkBox;
            }),
        });

        PrepaymentsMainDataGrid.ItemsSource = _memberPrepayments;
        SavePrepaymentsButton.CommandParameter = _memberPrepayments;
    }

    private void UpdatePrepaymentsMainDataGrid(int year, int month)
    {
        LoadItems(year, month);
        PrepaymentsMainDataGrid.Columns.Clear();
        InitPrepaymentsMainDataGrid();
    }
}