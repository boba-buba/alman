using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlmanUI.Views;

public partial class FinalPaymentsPageView : UserControl
{
    private IReadOnlyList<IStaffMemberBase>? _staffMembersTable;

    private IReadOnlyList<IFinalPaymentBase>? _finalPaymentsTable;

    private IReadOnlyList<FinalPayementCompositeItem> _memberFinalPayments { get; set; }

    private void LoadItems(int year, int month)
    {
        DateTime now = new(year, month, 1);

        _staffMembersTable = StaffMembersControl.GetItemsByFilter(m =>
            new DateTime(m.StartYear, m.StartMonth, 1) <= now);

        _finalPaymentsTable = FinalPaymentsControl.GetItemsByFilter(fp =>
            fp.Year == year && fp.Month == month);

        var memberFinalPayments = new List<FinalPayementCompositeItem>();

        foreach(var member in _staffMembersTable)
        {
            var newItem = new FinalPayementCompositeItem { StaffMember = member};
            IFinalPaymentBase? newItemFinalPayment = _finalPaymentsTable.SingleOrDefault(fp => fp.StaffMemberId == member.Id);
            if (_finalPaymentsTable.Count == 0 || newItemFinalPayment is null)
            {
                newItemFinalPayment = new FinalPaymentUI(member.Id, year, month);
            }
            newItem.FinalPayment = newItemFinalPayment;
            memberFinalPayments.Add(newItem);
        }
        _memberFinalPayments = memberFinalPayments;
    }

    public FinalPaymentsPageView()
    {
        LoadItems(DateTime.Now.Year, DateTime.Now.Month);
        if (_memberFinalPayments is null)
        {
            _memberFinalPayments = new List<FinalPayementCompositeItem>();
        }
        InitializeComponent();
        InitiFinalPaymentsMainDataGrid();
        Mediator.Mediator.Instance.NotifyWithParams += OnNotifyWithParams;
        
    }

    private void OnNotifyWithParams(string message, int year, int month)
    {
        if (message == "UpdateFinalPaymentsMainDataGrid")
            UpdateFinalPaymentsMainDataGrid(year, month);
    }

    private void InitiFinalPaymentsMainDataGrid()
    {
        FinalPaymentsMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        FinalPaymentsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Staff Member Name", Binding = new Binding("StaffMember.FirstName"), IsReadOnly = true });
        FinalPaymentsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Staff Member LastName", Binding = new Binding("StaffMember.LastName"), IsReadOnly = true });

        UIControlElements.AddNumericTextBoxToGrid<FinalPayementCompositeItem>(FinalPaymentsMainDataGrid, "PrePayment", "FinalPayment.PrepaymentSum");
        UIControlElements.AddCheckBoxToGrid<FinalPayementCompositeItem>(FinalPaymentsMainDataGrid, "Prepayment Was Paid", "FinalPayment.PrepaymentWasPaid");

        UIControlElements.AddNumericTextBoxToGrid<FinalPayementCompositeItem>(FinalPaymentsMainDataGrid, "Final Payment", "FinalPayment.FinalPaymentSum");
        UIControlElements.AddCheckBoxToGrid<FinalPayementCompositeItem>(FinalPaymentsMainDataGrid, "Final payment Was Paid", "FinalPayment.FinalPaymentWasPaid");

        FinalPaymentsMainDataGrid.ItemsSource = _memberFinalPayments;
        SaveFinalPaymentsButton.CommandParameter = _memberFinalPayments;
    }

    private void UpdateFinalPaymentsMainDataGrid(int year, int month)
    {
        LoadItems(year, month);
        FinalPaymentsMainDataGrid.Columns.Clear();
        InitiFinalPaymentsMainDataGrid();
    }
}