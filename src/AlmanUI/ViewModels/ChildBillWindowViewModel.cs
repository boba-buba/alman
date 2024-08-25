using AlmanUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI.ViewModels;

public partial class ChildBillWindowViewModel : ViewModelBase
{
    public ObservableCollection<ActivityMonth> ActivityMonths { get; set; }
    public ChildBillWindowViewModel()
    {

    }

    public ChildBillWindowViewModel(ChildBill bill)
    {
        ActivityMonths = new ObservableCollection<ActivityMonth>(bill.MonthlyActivities);
    }
}
