using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlmanUI.Views;
//TODO: final payment id : salary + activities - prepayment (Compute automatically)?

/// <summary>
/// View for staff final payments.
/// </summary>
public partial class FinalPaymentsPageView : UserControl, ILoadItemsWithParams, IInitDataGrid, IUpdateDataGrid
{
    /// <summary>
    /// Read from the database staff members table.
    /// </summary>
    private IReadOnlyList<IStaffMemberBase>? _staffMembersTable;

    /// <summary>
    /// Read from the database final payments table.
    /// </summary>
    private IReadOnlyList<IFinalPaymentBase>? _finalPaymentsTable;

    /// <summary>
    /// Collection to be shown in the view.
    /// </summary>
    private IReadOnlyList<FinalPayementCompositeItem> _memberFinalPayments { get; set; }

    public void LoadItems(int year, int month)
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
            CalculateFinalPayment(newItem.FinalPayment);
            memberFinalPayments.Add(newItem);
        }
        _memberFinalPayments = memberFinalPayments;
    }

    /// <summary>
    /// Based on salary + activities - prepayment calculate final payments for staff automatically.
    /// </summary>
    /// <param name="fp">Row with data from the table.</param>
    private void CalculateFinalPayment(IFinalPaymentBase fp)
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

        var activities = YearMonthStaffActivitiesControl.GetItemsByFilter(act => act.StaffMemberId == fp.StaffMemberId && act.Month == fp.Month && act.Year == fp.Year);

        int activitiesPayment = 0;
        var activitiesSum = activities.Sum(act => act.SumPaid);
        if (activitiesSum is not null)
        {
            activitiesPayment = (int)activitiesSum;
        }

        fp.FinalPaymentSum = salary + activitiesPayment - (fp.PrepaymentWasPaid==1?fp.PrepaymentSum:0);
    }

    /// <summary>
    /// ctor that initializes data for the view.
    /// </summary>
    public FinalPaymentsPageView()
    {
        LoadItems(DateTime.Now.Year, DateTime.Now.Month);
        if (_memberFinalPayments is null)
        {
            _memberFinalPayments = new List<FinalPayementCompositeItem>();
        }
        InitializeComponent();
        InitDataGrid();
        Mediator.Mediator.Instance.NotifyWithParams += OnNotifyWithParams;
        
    }

    /// <summary>
    /// Process notification that came from the Mediator.
    /// </summary>
    /// <param name="message">Message from view model.</param>
    /// <param name="year">1st param.</param>
    /// <param name="month">2nd param.</param>
    private void OnNotifyWithParams(string message, int year, int month)
    {
        if (message == "UpdateFinalPaymentsMainDataGrid")
            UpdateDataGrid(year, month);
    }

    public void InitDataGrid()
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

    public void UpdateDataGrid(int year, int month)
    {
        LoadItems(year, month);
        FinalPaymentsMainDataGrid.Columns.Clear();
        InitDataGrid();
    }
}