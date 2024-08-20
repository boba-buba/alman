using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace AlmanUI.ViewModels
{

    
    public partial class HomePageViewModel : ViewModelBase
    {
        [ObservableProperty]
        public int _currentYear = DateTime.Now.Year;

        [ObservableProperty]
        public int _lastYearRemainder = 0; //monthRemainder from last month

        [ObservableProperty]
        public int _yearSubsSum = 0; // All yearSubs for year

        [ObservableProperty]
        public int _yearRemainder = 0; // monthBalance from this year minus all expenses
        
        private void LoadItems()
        {
            LastYearRemainder = YearResultsControl.GetYearRemainder(CurrentYear - 1);
            YearSubsSum = HomeControl.ClaculateYearSubsSum(CurrentYear);
            ReturnCode retCode = YearResultsControl.CalculateYearRemainder(CurrentYear);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong during calculating {nameof(YearResultsControl.CalculateYearRemainder)}");
                return;
            }
            YearRemainder = YearResultsControl.GetYearRemainder(CurrentYear);
        }
        
        public HomePageViewModel()
        {
            LoadItems();
        }

        [RelayCommand]
        public void TriggerPrevYearCommand()
        {
            CurrentYear -= 1;
            LoadItems();
        }

        [RelayCommand]
        public void TriggerNextYearCommand()
        {
            CurrentYear += 1;
            LoadItems();
        }

        [RelayCommand]
        public void TriggerCalculateRemainder()
        {
            ReturnCode retCode = YearResultsControl.CalculateYearRemainder(CurrentYear);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong during calculating {nameof(YearResultsControl.CalculateYearRemainder)}");
                return;
            }
            LoadItems();
        }

    }
}
