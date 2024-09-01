using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

namespace AlmanUI.Views;

/// <summary>
/// View for the Calendar window.
/// </summary>
public partial class YMActivitiesCalendarWindow : Window
{
    /// <summary>
    /// Selected by user dates.
    /// </summary>
    public List<DateTime> SelectedDates { get; private set; }

    /// <summary>
    /// ctor
    /// </summary>
    public YMActivitiesCalendarWindow()
    {
        SelectedDates = new();
        InitializeComponent();
    }

    /// <summary>
    /// Save button clicked, save the chosen dates and close the window.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSaveChoiceClick(object sender, RoutedEventArgs e)
    {
        SelectedDates = new List<DateTime>(Calendar.SelectedDates);
        if (SelectedDates.Count == 0 )
        {
            Close();
        }
        Close();
    }
}