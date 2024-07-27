using Alman.SharedModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.ViewModels;

public partial class YearMonthActivitiesPageViewModel : ViewModelBase
{
    [ObservableProperty]
    public int _currentMonth = DateTime.Now.Month; //TODO converter

    [ObservableProperty]
    public int _currentYear = DateTime.Now.Year;

    public YearMonthActivitiesPageViewModel()
    {
        
    }

    [RelayCommand]
    public void TriggerPrevMonthCommand()
    {

    }


    [RelayCommand]
    public void TriggerNextMonthCommand()
    {

    }

    [RelayCommand]
    public void TriggerSaveCommand()
    {

    }
}

