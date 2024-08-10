using Alman.SharedModels;
using Alman.SharedDefinitions;
using AlmanUI.Controls;
using AlmanUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AlmanUI.ViewModels;

public partial class StaffPageViewModel : ViewModelBase
{
    public ObservableCollection<IStaffMemberBase> StaffMembers { get; set; }

    [ObservableProperty]
    private IStaffMemberBase? _selectedStaffMember = null;

    private IList<int> _staffMembersIdsToDelete;

    public StaffPageViewModel()
    {
        StaffMembers = new ObservableCollection<IStaffMemberBase>(StaffMembersControl.GetStaffMembers());
        _staffMembersIdsToDelete = new List<int>();
    }

    [RelayCommand]
    public void TriggerSaveCommand()
    {
        var retCode = StaffMembersControl.SaveStaffMembers(StaffMembers, _staffMembersIdsToDelete);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Smth went wrong saving {nameof(IStaffMemberBase)}'s");
            return;
        }
        _staffMembersIdsToDelete.Clear();
        StaffMembers.Clear();

        foreach (var member in StaffMembersControl.GetStaffMembers())
        {
            StaffMembers.Add(member);
        }
    }

    [RelayCommand]
    public void TriggerAddNewStaffMemberCommand()
    {
        IStaffMemberBase member = new StaffMemberUI { State = 1 };
        StaffMembers.Add(member);
    }

    [RelayCommand]
    public void TriggerDeleteStaffMemberCommand()
    {
        if (SelectedStaffMember is null) return;

        if (SelectedStaffMember.Id != 0)
        {
            _staffMembersIdsToDelete.Add(SelectedStaffMember.Id);
        }
        StaffMembers.Remove(SelectedStaffMember);
        SelectedStaffMember = null;
    }
}
