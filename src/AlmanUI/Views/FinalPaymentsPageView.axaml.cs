using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using DbAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlmanUI.Views;
//TODO: final payment id : salary + activities - prepayment (Compute automatically)?
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
            ComputeFinalPayment(newItem.FinalPayment);
            memberFinalPayments.Add(newItem);
        }
        _memberFinalPayments = memberFinalPayments;
    }

    private void ComputeFinalPayment(IFinalPaymentBase fp)
    {
        if (fp.Month is 0 || fp.Year is 0)
        {
            return;
        }

        int salary = 0;
        var member = StaffMembersControl.GetItemById(fp.StaffMemberId);
        if (member is not null)
        {
            salary = member.PositionSalary;
        }

        var activities = YearMonthStaffActivitiesControl.GetItemsByFilter(act => act.StaffMemberId == fp.StaffMemberId && act.Month == fp.Month && act.Year == fp.Year && act.WasPaid == (int)WasPaid.True);

        int activitiesPayment = 0;
        var activitiesSum = activities.Sum(act => act.SumPaid);
        if (activitiesSum is not null)
        {
            activitiesPayment = (int)activitiesSum;
        }

        fp.FinalPaymentSum = salary + activitiesPayment - (fp.PrepaymentWasPaid==1?fp.PrepaymentSum:0);
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

        FinalPaymentsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.StaffResources.StaffFirstName, Binding = new Binding("StaffMember.FirstName"), IsReadOnly = true });
        FinalPaymentsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = AlmanUI.Resources.StaffResources.StaffLastName, Binding = new Binding("StaffMember.LastName"), IsReadOnly = true });

        UIControlElements.AddNumericTextBoxToGrid<FinalPayementCompositeItem>(FinalPaymentsMainDataGrid, AlmanUI.Resources.StaffResources.Prepayment, "FinalPayment.PrepaymentSum");
        UIControlElements.AddCheckBoxToGrid<FinalPayementCompositeItem>(FinalPaymentsMainDataGrid, AlmanUI.Resources.StaffResources.PrepaymentWasPaid, "FinalPayment.PrepaymentWasPaid");
        //TODO tooltip
        UIControlElements.AddNumericTextBoxToGrid<FinalPayementCompositeItem>(FinalPaymentsMainDataGrid, AlmanUI.Resources.StaffResources.FinalPayment, "FinalPayment.FinalPaymentSum");
        UIControlElements.AddCheckBoxToGrid<FinalPayementCompositeItem>(FinalPaymentsMainDataGrid, AlmanUI.Resources.StaffResources.FinalPaymentWasPaid, "FinalPayment.FinalPaymentWasPaid");

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