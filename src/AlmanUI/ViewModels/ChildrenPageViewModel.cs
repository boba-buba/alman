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
            Children = new ObservableCollection<IChildBase>(ChildrenControl.GetItems());
            _childrenIdsToDelete = new List<int>();
        }

        [RelayCommand]
        public void TriggerSaveCommand()
        {
            var retCode = ChildrenControl.SaveItems(Children, _childrenIdsToDelete);
            if (retCode != ReturnCode.OK)
            {
                Debug.WriteLine($"Smth went wrong saving {nameof(IChildBase)}'s");
                return;
            }
            _childrenIdsToDelete.Clear();
            Children.Clear();
            foreach (var child in ChildrenControl.GetItems())
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
            
            if (SelectedChild.Id != 0)
            {
                _childrenIdsToDelete.Add(SelectedChild.Id);
            }
            Children.Remove(SelectedChild);
            SelectedChild = null;
        }
    }
}
