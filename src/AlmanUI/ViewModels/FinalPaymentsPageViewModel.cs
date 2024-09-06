using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using AlmanUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for fetching and managing Final Payments.
/// </summary>
public partial class FinalPaymentsPageViewModel : ViewModelBase, ILoadItems
{
    /// <summary>
    /// The month that is shown and data are fetched for.
    /// </summary>
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month;

    /// <summary>
    /// The year that is shown and data are fetched for.
    /// </summary>
    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

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
    public ObservableCollection<FinalPayementCompositeItem> MemberFinalPayments { get; set; }

    public void LoadItems()
    {
        DateTime now = new(CurrentYear, CurrentMonth, 1);

        _staffMembersTable = StaffMembersControl.GetItemsByFilter(m =>
            new DateTime(m.StartYear, m.StartMonth, 1) <= now && m.State == (int)StaffMemberState.Active);

        _finalPaymentsTable = FinalPaymentsControl.GetItemsByFilter(fp =>
            fp.Year == CurrentYear && fp.Month == CurrentMonth);

        if (MemberFinalPayments is null)
        {
            MemberFinalPayments = new();
        }
        else if (MemberFinalPayments.Count > 0)
        {
            MemberFinalPayments.Clear();
        }

        foreach (var member in _staffMembersTable)
        {
            var newItem = new FinalPayementCompositeItem { StaffMember = member };
            IFinalPaymentBase? newItemFinalPayment = _finalPaymentsTable.SingleOrDefault(fp => fp.StaffMemberId == member.Id);
            if (_finalPaymentsTable.Count == 0 || newItemFinalPayment is null)
            {
                newItemFinalPayment = new FinalPaymentUI(member.Id, CurrentYear, CurrentMonth);

            }
            newItem.FinalPayment = newItemFinalPayment;
            CalculateFinalPayment(newItem.FinalPayment);
            MemberFinalPayments.Add(newItem);
        }
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

        fp.FinalPaymentSum = salary + activitiesPayment - (fp.PrepaymentWasPaid == 1 ? fp.PrepaymentSum : 0);
    }

    /// <summary>
    /// ctor.
    /// </summary>
    public FinalPaymentsPageViewModel() { LoadItems(); }

    /// <summary>
    /// Set month to the previous. Send notification to the view to load data for new month.
    /// </summary>
    [RelayCommand]
    public void TriggerPrevMonth()
    {
        if (CurrentMonth == (int)Months.January)
        {
            CurrentMonth = (int)Months.December;
            CurrentYear = CurrentYear - 1;
        }
        else
        {
            CurrentMonth = CurrentMonth - 1;
        }
        LoadItems();
        Mediator.Mediator.Instance.SendWithParams("UpdateFinalPaymentsMainDataGrid", CurrentYear, CurrentMonth);
    }

    /// <summary>
    /// Set month to the next. Send notification to the view to load data for new month.
    /// </summary>
    [RelayCommand]
    public void TriggerNextMonthCommand()
    {
        if (CurrentMonth == (int)Months.December)
        {
            CurrentMonth = (int)Months.January;
            CurrentYear = CurrentYear + 1;
        }
        else
        {
            CurrentMonth = CurrentMonth + 1;
        }
        LoadItems();
        Mediator.Mediator.Instance.SendWithParams("UpdateFinalPaymentsMainDataGrid", CurrentYear, CurrentMonth);

    }

    /// <summary>
    /// Save the modified UI vIew table and fetch the latest data from the database.
    /// </summary>
    [RelayCommand]
    public void TriggerSaveCommand()
    {
        if (MemberFinalPayments.Count == 0) { return; }
        List<IFinalPaymentBase> finalPayments = new List<IFinalPaymentBase>();
        foreach (var item in MemberFinalPayments)
        {
            if (item.StaffMember is null || item.FinalPayment is null)
            {
                Debug.WriteLine($"Null {nameof(item.StaffMember)} or {nameof(item.FinalPayment)}");
                continue;
            }
            finalPayments.Add(item.FinalPayment);
        }
        ReturnCode retCode = FinalPaymentsControl.SaveItems(finalPayments);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong saving {nameof(FinalPaymentUI)}'s. Changes were not saved.");
        }
        LoadItems();
        Mediator.Mediator.Instance.SendWithParams("UpdateFinalPaymentsMainDataGrid", CurrentYear, CurrentMonth);
    }
}
