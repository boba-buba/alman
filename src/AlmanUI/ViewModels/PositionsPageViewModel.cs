using Alman.SharedModels;
using Alman.SharedDefinitions;
using Avalonia.Data.Converters;
using System.Globalization;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlmanUI.Controls;
using AlmanUI.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace AlmanUI.ViewModels;

public partial class PositionsPageViewModel : ViewModelBase
{
    public ObservableCollection<IPositionBase> Positions { get; private set; }
    
    private List<int> _positionsIdsToDelete;

    [ObservableProperty]
    private IPositionBase? _selectedPosition = null;
    public PositionsPageViewModel() 
    {
        Positions = new ObservableCollection<IPositionBase>(PositionsControl.GetPositions());
        _positionsIdsToDelete = new List<int>();
    }

    [RelayCommand]
    public void TriggerSaveCommand()
    {

        var retCode = PositionsControl.SavePositions(Positions, _positionsIdsToDelete);
        if (retCode != ReturnCode.OK)
        {
            //New error window
            Debug.WriteLine("Smth went wrong");
            return;
        }
        _positionsIdsToDelete.Clear();
        Positions.Clear();
        foreach (var item in PositionsControl.GetPositions())
        {
            Positions.Add(item);
        }
    }

    [RelayCommand]
    public void TriggerAddNewPositionCommand()
    {
        IPositionBase position = new PositionUI { PositionId = 0};
        Positions.Add(position);
    }

    [RelayCommand]
    public void TriggerRemovePositionCommand()
    {
        //nothong is chosen
        if (SelectedPosition == null)
        {
            return;
        }

        if (SelectedPosition.PositionId != 0)
        {
            _positionsIdsToDelete.Add(SelectedPosition.PositionId);   
        }

        Positions.Remove(SelectedPosition);
        SelectedPosition = null;
    }
}
