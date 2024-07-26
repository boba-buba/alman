using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alman.SharedDefinitions;
using AlmanUI.Models;
using Avalonia;
using Business;
using System.Diagnostics;
using Alman.SharedModels;

namespace AlmanUI.ViewModels
{
    public partial class ChildrenPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isChanged = false;

        public ObservableCollection<IChildBase> Children { get; }
        
        [ObservableProperty]
        private IChildBase? _selectedChild = null;
        public ChildrenPageViewModel()
        {
            var children = BusinessChildrenAPI.GetChildren();
            Children = new ObservableCollection<IChildBase>(children);
        }

        /*partial void OnSelectedChildChanged(Child? value)
        {
            throw new NotImplementedException();
        }*/

        public string GetContractNameByChild(ChildUI child)
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
            BusinessChildrenAPI.SaveChildren(Children);

            IsChanged = false;
        }

        public void TriggerAddNewChildCommand()
        {
            IChildBase child = (IChildBase)new ChildUI();
            Children.Add(child);
        }

        public void TriggerRemoveChildCommand()
        {
            if (SelectedChild == null)
            {
                return;
            }
            if (SelectedChild.ChildId == 0)
            {
                Children.Remove(SelectedChild);
            }
            Debug.WriteLine(SelectedChild.ChildId);
            //business log
        }

        public void ChooseChildContractType(string contractType)
        {

        }

    }
    
}
