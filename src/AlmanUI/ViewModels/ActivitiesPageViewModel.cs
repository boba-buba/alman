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
/// ViewModel for the Activities. Buttons commands, data.
/// </summary>
public partial class ActivitiesPageViewModel : ViewModelBase
{
    /// <summary>
    /// Read activities table.
    /// </summary>
    public ObservableCollection<IActivityBase> Activities { get; set; }

    /// <summary>
    /// The row that is selected in UI View at the moment.
    /// </summary>
    [ObservableProperty]
    private IActivityBase? _selectedActivity = null;

    /// <summary>
    /// Ids of the items that were deleted in UI and must be deleted from database.
    /// </summary>
    private IList<int> _activitiesIdsToDelete;

    /// <summary>
    /// ctor.
    /// </summary>
    public ActivitiesPageViewModel()
    {
        Activities = new ObservableCollection<IActivityBase>(ActivitiesControl.GetItems());
        _activitiesIdsToDelete = new List<int>();
    }

    /// <summary>
    /// Save the changes and read the table again. Reading the table again is neccessary to get ids of newly inserted items.
    /// </summary>
    [RelayCommand]
    public void TriggerSaveCommand()
    {
        var retCode = ActivitiesControl.SaveItems(Activities, _activitiesIdsToDelete);
        if (retCode != ReturnCode.OK)
        {
            //New error window
            Debug.WriteLine("Smth went wrong");
            return;
        }
        _activitiesIdsToDelete.Clear();
        Activities.Clear();
        foreach (var item in ActivitiesControl.GetItems())
        {
            Activities.Add(item);
        }
    }

    /// <summary>
    /// Add new item to the table in UI.
    /// </summary>
    [RelayCommand]
    public void TriggerAddNewActivityCommand()
    {
        IActivityBase activity = new ActivityUI();
        Activities.Add(activity);
    }

    /// <summary>
    /// Remove selected item <see cref="_selectedActivity"/> from UI table and set it to null.
    /// </summary>
    [RelayCommand]
    public void TriggerRemoveActivityCommand()
    {
        if (SelectedActivity == null)
        {
            return;
        }

        if (SelectedActivity.Id != 0)
        {
            _activitiesIdsToDelete.Add(SelectedActivity.Id);
        }

        Activities.Remove(SelectedActivity); 
        SelectedActivity = null;
    }
}
