using Alman.SharedModels;
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

namespace AlmanUI.ViewModels
{
    public partial class ActivitiesPageViewModel : ViewModelBase
    {

        public ObservableCollection<IActivityBase> Activities { get; set; }

        [ObservableProperty]
        private IActivityBase? _selectedActivity = null;

        private IList<int> _activitiesIdsToDelete;

        public ActivitiesPageViewModel()
        {
            Activities = new ObservableCollection<IActivityBase>(ActvitiesControl.GetActivities());
            _activitiesIdsToDelete = new List<int>();
        }

        [RelayCommand]
        public void TriggerSaveCommand()
        {
            var retCode = ActvitiesControl.SaveActivities(Activities, _activitiesIdsToDelete);
            _activitiesIdsToDelete.Clear();
        }

        [RelayCommand]
        public void TriggerAddNewActivityCommand()
        {
            IActivityBase activity = new ActivityUI();
            Activities.Add(activity);
        }

        [RelayCommand]
        public void TriggerRemoveActivityCommand()
        {
            if (SelectedActivity == null)
            {
                return;
            }

            if (SelectedActivity.ActivityId == 0)
            {
                Activities.Remove(SelectedActivity);
                SelectedActivity = null;
            }
            else
            {
                _activitiesIdsToDelete.Add(SelectedActivity.ActivityId);
                Activities.Remove(SelectedActivity); 
                SelectedActivity = null;
            }
            
        }
    }
}
