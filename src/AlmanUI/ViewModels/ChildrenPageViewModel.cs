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
/// ViewModel for getting and managing data for the Children.
/// </summary>
public partial class ChildrenPageViewModel : ViewModelBase
{
    /// <summary>
    /// Read from database children database.
    /// </summary>
    public ObservableCollection<IChildBase> Children { get; set; }
    
    /// <summary>
    /// The row that is selected in UI View at the moment.
    /// </summary>
    [ObservableProperty]
    private IChildBase? _selectedChild = null;

    /// <summary>
    /// Ids of the items that were deleted in UI and must be deleted from database.
    /// </summary>
    private IList<int> _childrenIdsToDelete;

    /// <summary>
    /// ctor that initializes <seealso cref="Children"/> and <seealso cref="_childrenIdsToDelete"/>
    /// </summary>
    public ChildrenPageViewModel()
    {
        Children = new ObservableCollection<IChildBase>(ChildrenControl.GetItems());
        _childrenIdsToDelete = new List<int>();
    }

    /// <summary>
    /// Save the changes made in UI View and read the table again.
    /// </summary>
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

    /// <summary>
    /// Add new row in UI View Table.
    /// </summary>
    [RelayCommand]
    public void TriggerAddNewChildCommand()
    {
        IChildBase child = new ChildUI { ChildState = 1, ChildContract = 1, ChildGroup = 1, ChildStartYear = DateTime.Now.Year, ChildStartMonth = DateTime.Now.Month};
        Children.Add(child);
    }

    /// <summary>
    /// Remove the <seealso cref="_selectedChild"/> row from the UI View.
    /// </summary>
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
