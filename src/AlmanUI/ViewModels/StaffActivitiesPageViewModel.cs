using Alman.SharedDefinitions;
using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for fetching and managing data for Staff activities.
/// </summary>
public partial class StaffActivitiesPageViewModel : ViewModelBase
{
    /// <summary>
    /// Read from the database StaffActivities table.
    /// </summary>
    public ObservableCollection<IStaffActivityBase> StaffActivities { get; set; }
    
    /// <summary>
    /// Selected row of th UI table.
    /// </summary>
    [ObservableProperty]
    private IStaffActivityBase? _selectedStaffActivity = null;

    /// <summary>
    /// Ids of the rows to be deleted from the database.
    /// </summary>
    private IList<int> _staffActivitiesIdsToDelete;

    /// <summary>
    /// ctor ttha initializes <seealso cref="StaffActivities"/> and <seealso cref="_staffActivitiesIdsToDelete"/>.
    /// </summary>
    public StaffActivitiesPageViewModel()
    {
        StaffActivities = new ObservableCollection<IStaffActivityBase>(StaffActivitiesControl.GetItems());
        _staffActivitiesIdsToDelete = new List<int>();
    }

    /// <summary>
    /// Save the modified UI vIew table and fetch the latest data from the database.
    /// </summary>
    [RelayCommand]
    public void TriggerSaveCommand()
    {
        var retCode = StaffActivitiesControl.SaveItems(StaffActivities, _staffActivitiesIdsToDelete);
        if (retCode != ReturnCode.OK)
        {
            //New error window
            Debug.WriteLine("Smth went wrong");
            return;
        }
        _staffActivitiesIdsToDelete.Clear();
        StaffActivities.Clear();
        foreach (var item in StaffActivitiesControl.GetItems())
        {
            StaffActivities.Add(item);
        }
    }

    /// <summary>
    /// Add new row to the UI table.
    /// </summary>
    [RelayCommand]
    public void TriggerAddNewStaffActivity()
    {
        IStaffActivityBase activity = new StaffActivityUI();
        StaffActivities.Add(activity);
    }

    /// <summary>
    /// Remove the selected row from the UI table. Set <seealso cref="_selectedStaffActivity"/> to null.
    /// </summary>
    [RelayCommand]
    public void TriggerRemoveStaffActivityCommand()
    {
        if (SelectedStaffActivity == null)
        {
            return;
        }

        if (SelectedStaffActivity.Id != 0)
        {
            _staffActivitiesIdsToDelete.Add(SelectedStaffActivity.Id);
        }

        StaffActivities.Remove(SelectedStaffActivity);
        SelectedStaffActivity = null;
    }
}
