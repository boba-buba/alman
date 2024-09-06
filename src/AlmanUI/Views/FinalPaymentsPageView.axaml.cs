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
public partial class FinalPaymentsPageView : UserControl, IInitDataGrid, IUpdateDataGrid
{
    /// <summary>
    /// ctor that initializes data for the view.
    /// </summary>
    public FinalPaymentsPageView()
    {
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
    }

    public void UpdateDataGrid(int year, int month)
    {
        FinalPaymentsMainDataGrid.Columns.Clear();
        InitDataGrid();
    }
}