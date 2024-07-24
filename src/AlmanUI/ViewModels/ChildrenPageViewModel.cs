using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alman.Models;
using Avalonia;
using Business;
using System.Diagnostics;

namespace AlmanUI.ViewModels
{
    public partial class ChildrenPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isChanged = false;

        public ObservableCollection<Child>  Children { get; }
        
        [ObservableProperty]
        private Child? _selectedChild = null;
        public ChildrenPageViewModel()
        {
            var children = BusinessChildrenAPI.GetChildren();
            Children = new ObservableCollection<Child>(children);
        }

        /*partial void OnSelectedChildChanged(Child? value)
        {
            throw new NotImplementedException();
        }*/

        public string GetContractNameByChild(Child child)
        {
            ContractType type = (ContractType)child.ChildContract;
            switch (type)
            {
                case ContractType.Precontract:
                    return "Precontract";
                case ContractType.OrdinaryContract:
                    return "Standard";
                case ContractType.MotherCapital:
                    return "MotherContract";
                case ContractType.StaffChild:
                    return "StaffChild";
                default:
                    return "Unknown";
            }
        }

        [RelayCommand]
        public void TriggerSaveCommand()
        {
            //call save command
            IsChanged = false;
        }

        public void TriggerAddNewChildCommand()
        {

        }

        public void TriggerRemoveChildCommand()
        {
            if (SelectedChild == null)
            {
                return;
            }
            Debug.WriteLine(SelectedChild.ChildId);
            //business log
        }

    }
    
}
