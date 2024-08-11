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
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.ViewModels;

public partial class StaffActivitiesPageViewModel : ViewModelBase
{
    public ObservableCollection<IStaffActivityBase> StaffActivities { get; set; }
    
    [ObservableProperty]
    private IStaffActivityBase? _selectedStaffActivity = null;

    private IList<int> _staffActivitiesIdsToDelete;

    public StaffActivitiesPageViewModel()
    {
        StaffActivities = new ObservableCollection<IStaffActivityBase>(StaffActivitiesControl.GetItems());
        _staffActivitiesIdsToDelete = new List<int>();
    }

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

    [RelayCommand]
    public void TriggerAddNewStaffActivity()
    {
        IStaffActivityBase activity = new StaffActivityUI();
        StaffActivities.Add(activity);
    }

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
