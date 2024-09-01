using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AlmanUI.ViewModels;

/// <summary>
/// Base class for every ViewModel. 
/// </summary>
public class ViewModelBase : ObservableObject
{
}

public interface IMonthButtons
{

    /// <summary>
    /// Set month to the previous. Send notification to the view to load data for new month.
    /// </summary>
    public void TriggerPrevMonthCommand();

    /// <summary>
    /// Set month to the next. Send notification to the view to load data for new month.
    /// </summary>
    public void TriggerNextMonthCommand();

}

public interface IAddRemoveButtons
{
    /// <summary>
    /// Add new row to the UI table.
    /// </summary>
    public void TriggerAddNewCommand();

    /// <summary>
    /// Remove the selected row (selected item) from the UI table.
    /// </summary>
    public void TriggerRemoveCommand();
}

public interface ISaveButtonWithParam<in TItem>
{
    /// <summary>
    /// Save modified UI table and fetch the latest data.
    /// </summary>
    /// <param name="items">Modified UI table.</param>
    public void TriggerSaveCommand(IReadOnlyList<TItem> items);
}

public interface ISaveButtonWithoutParam
{
    /// <summary>
    /// Save modified UI table and fetch the latest data.
    /// </summary>
    public void TriggerSaveCommand();
}

public interface ICurrentMonth
{
    /// <summary>
    /// The month that is shown and data are fetched for.
    /// </summary>
    public int CurrentMonth { get; set; }
}

public interface ICurrentYear
{
    /// <summary>
    /// The year that is shown and data are fetched for.
    /// </summary>
    public int CurrentYear { get; set; }
}

public interface IYearButtons
{
    /// <summary>
    /// Set year to the previous. Send notification to the view to load data for new year.
    /// </summary>
    public void TriggerPrevYearCommand();

    /// <summary>
    /// Set year to the next. Send notification to the view to load data for new year.
    /// </summary>
    public void TriggerNextYearCommand();

}