using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Alman.SharedDefinitions;
using AlmanUI.Models;
using Avalonia;
using Business;
using System.Diagnostics;
using Alman.SharedModels;
using Avalonia.Data.Converters;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Media.TextFormatting.Unicode;
using AlmanUI.Controls;

namespace AlmanUI.ViewModels
{
    public partial class ChildrenPageViewModel : ViewModelBase
    {

        public ObservableCollection<IChildBase> Children { get; set; }
        
        [ObservableProperty]
        private IChildBase? _selectedChild = null;

        private IList<int> _childrenIdsToDelete;
        public ChildrenPageViewModel()
        {
            Children = new ObservableCollection<IChildBase>(BusinessChildrenApi.GetChildren());
            _childrenIdsToDelete = new List<int>();
        }

        public ObservableCollection<string> ChildrenContractNames { get; } = new ObservableCollection<string>
        {
            "Precontract",
            "Mother Capital",
            "Standard",
            "Staff child"
        };


        [RelayCommand]
        public void TriggerSaveCommand()
        {
            var retCode = ChildrenControl.SaveChildren(Children, _childrenIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong saving {nameof(IChildBase)}'s");
                return;
            }
            _childrenIdsToDelete.Clear();
            Children.Clear();
            foreach (var child in ChildrenControl.GetChildren())
            {
                Children.Add(child);
            }

        }

        [RelayCommand]
        public void TriggerAddNewChildCommand()
        {
            IChildBase child = new ChildUI { ChildState = 1, ChildContract = 1, ChildGroup = 1, ChildStartYear = DateTime.Now.Year, ChildStartMonth = DateTime.Now.Month};
            Children.Add(child);
        }

        [RelayCommand]
        public void TriggerRemoveChildCommand()
        {
            if (SelectedChild == null)
            {
                return;
            }
            
            if (SelectedChild.ChildId != 0)
            {
                _childrenIdsToDelete.Add(SelectedChild.ChildId);
            }
            Children.Remove(SelectedChild);
            SelectedChild = null;
        }

        public void ChooseChildContractType(string contractType)
        {

        }

    }

    public class ChildrenContractConverter : IValueConverter
    {
        public static readonly ChildrenContractConverter Instance = new();

        public object? Convert(object? value, Type targetType,
                                    object? parameter, CultureInfo culture)
        {

            if (value is int number && number > 0 && number <= (int)ContractType.StaffChild)
            {

                var contract = (ContractType)number;
                switch (contract)
                {
                    case ContractType.MotherCapital: { return "Mother Capital";}
                    case ContractType.Precontract: { return "Precontract"; }
                    case ContractType.OrdinaryContract: { return "Ordinary"; }
                    case ContractType.StaffChild: { return "Staff Child"; }
                    default: return "Unknown";
                    
                }
            }
            // converter used for the wrong type
            return new BindingNotification(new InvalidCastException(),
                                                    BindingErrorType.Error);
        }

        public object ConvertBack(object? value, Type targetType,
                               object? parameter, CultureInfo culture)
        {
            if (value is string contractName)
            {
                switch (contractName)
                {
                    case "Mother Capital": return (int)ContractType.MotherCapital;
                    case "Precontract": return (int)ContractType.Precontract;
                    case "Ordinary": return (int)ContractType.OrdinaryContract;
                    case "Staff Child": return (int)ContractType.StaffChild;
                }
            }
            // converter used for the wrong type
            return new BindingNotification(new InvalidCastException(),
                                                    BindingErrorType.Error);
        }
    }


}
