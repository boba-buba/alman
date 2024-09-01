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

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for fetching and managing data for Staff.
/// </summary>
public partial class StaffPageViewModel : ViewModelBase
{
    /// <summary>
    /// Read Staff table from the database.
    /// </summary>
    public ObservableCollection<IStaffMemberBase> StaffMembers { get; set; }

    /// <summary>
    /// Selected row in the UI table.
    /// </summary>
    [ObservableProperty]
    private IStaffMemberBase? _selectedStaffMember = null;

    /// <summary>
    /// Ids of the rows to be deleted from the database.
    /// </summary>
    private IList<int> _staffMembersIdsToDelete;

    /// <summary>
    /// ctor that initializes <seealso cref="StaffMembers"/> and <seealso cref="_staffMembersIdsToDelete"/>.
    /// </summary>
    public StaffPageViewModel()
    {
        StaffMembers = new ObservableCollection<IStaffMemberBase>(StaffMembersControl.GetItems());
        _staffMembersIdsToDelete = new List<int>();
    }


    /// <summary>
    /// Save the modified UI vIew table and fetch the latest data from the database.
    /// </summary>
    [RelayCommand]
    public void TriggerSaveCommand()
    {
        var retCode = StaffMembersControl.SaveItems(StaffMembers, _staffMembersIdsToDelete);
        if (retCode != ReturnCode.OK)
        {
            Debug.WriteLine($"Smth went wrong saving {nameof(IStaffMemberBase)}'s");
            return;
        }
        _staffMembersIdsToDelete.Clear();
        StaffMembers.Clear();

        foreach (var member in StaffMembersControl.GetItems())
        {
            StaffMembers.Add(member);
        }
    }

    /// <summary>
    /// Add new row to the UI table.
    /// </summary>
    [RelayCommand]
    public void TriggerAddNewStaffMemberCommand()
    {
        IStaffMemberBase member = new StaffMemberUI { State = 1 , StartYear = DateTime.Now.Year, StartMonth = DateTime.Now.Month};
        StaffMembers.Add(member);
    }

    /// <summary>
    /// Remove the selected row from the UI table. Set <seealso cref="_selectedStaffMember"/> to null.
    /// </summary>
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
