using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System;
using Avalonia.Interactivity;

namespace AlmanUI.Views;

public partial class YMActivitiesCalendarWindow : Window
{
    public List<DateTime> SelectedDates { get; private set; }
    public YMActivitiesCalendarWindow()
    {
        InitializeComponent();
    }

    private void OnSaveChoiceClick(object sender, RoutedEventArgs e)
    {
        SelectedDates = new List<DateTime>(Calendar.SelectedDates);
        if (SelectedDates.Count == 0 )
        {
            Close();
        }
        int year = SelectedDates[0].Year;
        int month = SelectedDates[0].Month;
        

        Close();
    }
}