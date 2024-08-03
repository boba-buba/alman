using Alman.SharedModels;
using AlmanUI.Controls;
using Alman.SharedDefinitions;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using AlmanUI.Views;
using System.Diagnostics;
using AlmanUI.Mediator;
using AlmanUI.Models;
using Avalonia.Data.Converters;
using System.Globalization;
using System.Linq;

namespace AlmanUI.ViewModels;

public partial class YearMonthActivitiesPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    //public List<string> PaymentMethods { get; } = new List<string>() { "Cash", "Transfer" };
    public List<int> PaymentMethods { get; } = new List<int>() { (int)WayOfPaying.Cash, (int)WayOfPaying.Transfer};

    public YearMonthActivitiesPageViewModel() {}


    [RelayCommand]
    public void TriggerPrevMonthCommand()
    {
        if (CurrentMonth == 1)
        {
            CurrentMonth = 12;
            CurrentYear = CurrentYear - 1;
        }
        else
        {
            CurrentMonth = CurrentMonth - 1;
        }

        Mediator.Mediator.Instance.SendWithParams("UpdateDataGrid", CurrentYear, CurrentMonth);
    }


    [RelayCommand]
    public void TriggerNextMonthCommand()
    {
        if (CurrentMonth == 12)
        {
            CurrentMonth = 1;
            CurrentYear = CurrentYear + 1;
        }
        else
        {
            CurrentMonth = CurrentMonth + 1;
        }
        Mediator.Mediator.Instance.SendWithParams("UpdateDataGrid", CurrentYear, CurrentMonth);
    }


    [RelayCommand]
    public void TriggerSaveCommand(IReadOnlyList<CompositeItem> items)
    {
       
        List<YearMonthActivityUI> yearMonthActivities = new List<YearMonthActivityUI>();

        foreach (var item in items)
        {
            if (item.YMActivities == null || item.YMChild == null)
            {
                Debug.WriteLine($"Null {nameof(item.YMActivities)} or {nameof(item.YMChild)}");
                return;
            }
            foreach (var activity in item.YMActivities) {

                yearMonthActivities.Add(new YearMonthActivityUI{
                    Year = CurrentYear,
                    Month = CurrentMonth,
                    YmactivityId = activity.YmactivityId,
                    YmactivitySum = activity.YmactivitySum,
                    YmchildId = item.YMChild.ChildId,
                    YmwasPaid = activity.YmwasPaid,
                    YmwayOfPaying = activity.YmwayOfPaying,
                }); 
            }
        }

        ReturnCode retval = YearMonthActivitiesControl.SaveYearMonthActivities(yearMonthActivities, CurrentYear, CurrentMonth);
        if (retval != ReturnCode.OK)
        {
            Debug.WriteLine($"Something went wrong updating {nameof(YearMonthActivityUI)}'s. Changes were not saved.");
        }
    }



}


public class PaymentMethodConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        if (value is WayOfPaying paymentMethod)
        {
            return paymentMethod switch
            {
                WayOfPaying.Cash => "Cash",
                WayOfPaying.Transfer => "Transfer",
                _ => "Unknown"
            };
        }
        return "Unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return str switch
            {
                "Cash" => WayOfPaying.Cash,
                "Transfer" => WayOfPaying.Transfer,
                _ => WayOfPaying.Cash
            };
        }
        return (int)WayOfPaying.Cash;
    }
}
