using Alman.SharedDefinitions;
using AlmanUI.Models;
using AlmanUI.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.ViewModels
{

    public partial class ContractFeesPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        public int _currentMonth = DateTime.Now.Month;

        [ObservableProperty]
        public int _currentYear = DateTime.Now.Year;

        public ContractFeesPageViewModel() { }

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

            Mediator.Mediator.Instance.SendWithParams("UpdateContractFeesMainDataGrid", CurrentYear, CurrentMonth);

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
            Mediator.Mediator.Instance.SendWithParams("UpdateContractFeesMainDataGrid", CurrentYear, CurrentMonth);

        }

        [RelayCommand]
        public void TriggerSaveCommand(IReadOnlyList<ContractFeesCompositeItem> items)
        {
            if (items.Count == 0) { return; }
            List<ContractFeeUI> contractFees = new List<ContractFeeUI>();
            foreach (var item in items)
            {
                if (item.CFchild is null || item.CFcontractFee is null)
                {
                    Debug.WriteLine($"Null {nameof(item.CFchild)} or {nameof(item.CFcontractFee)}");
                    continue;
                }

                contractFees.Add(new ContractFeeUI
                {
                    CfchildId = item.CFchild.ChildId,
                    Cfmonth = item.CFcontractFee.Cfmonth,
                    Cfyear = item.CFcontractFee.Cfyear,
                    CfsumPaid = item.CFcontractFee.CfsumPaid
                });   
            }

            ReturnCode retCode = ContractFeesControl.SaveContractFees(contractFees);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Something went wrong saving {nameof(ContractFeeUI)}'s. Changes were not saved.");
            }

        }
    }
}
