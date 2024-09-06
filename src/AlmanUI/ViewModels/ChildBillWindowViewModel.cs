using AlmanUI.Controls;
using System.Collections.ObjectModel;

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for getting data for the child bill window.
/// </summary>
public partial class ChildBillWindowViewModel : ViewModelBase
{
    /// <summary>
    /// All activities child took part in during particular month in particular year.
    /// </summary>
    public ObservableCollection<ActivityMonth> ActivityMonths { get; set; }
    public ChildBillWindowViewModel()
    {
        ActivityMonths = new ObservableCollection<ActivityMonth>();
    }

    /// <summary>
    /// ctor that sets <see cref="ActivityMonths"/>.
    /// </summary>
    /// <param name="bill">Created bill.</param>
    public ChildBillWindowViewModel(ChildBill bill)
    {
        ActivityMonths = new ObservableCollection<ActivityMonth>(bill.MonthlyActivities);
    }
}
