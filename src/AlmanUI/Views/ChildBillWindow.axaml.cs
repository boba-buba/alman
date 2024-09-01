using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.ViewModels;
using Avalonia.Controls;

namespace AlmanUI.Views;

/// <summary>
/// Child bill window view.
/// </summary>
public partial class ChildBillWindow : Window, ILoadItems
{
    /// <summary>
    /// Bill for the particular child.
    /// </summary>
    public ChildBill Bill { get; private set; }

    /// <summary>
    /// ctor.
    /// </summary>
    public ChildBillWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Set <paramref name="childBill"/> to the public property <seealso cref="Bill"/> for the view.
    /// </summary>
    /// <param name="childBill">The bill to be set.</param>
    public void SetChildBill(ChildBill childBill)
    {
        Bill = childBill;
        DataContext = new ChildBillWindowViewModel(childBill);
        LoadItems();
    }

    public void LoadItems()
    {
        IChildBase child = ChildrenControl.GetItemById(Bill.ChildId)!;

        if (Bill.PrecontractSum is 0)
        {
            PrecontractTextTextBlock.IsVisible = false;
            PrecontractSumTextBlock.IsVisible = false;
        }
        else
        {
            PrecontractSumTextBlock.Text = Bill.PrecontractSum.ToString();
        }

        ChildTextBlock.Text = $"{child.ChildName} {child.ChildLastName}";
        YearTextBlock.Text = Bill.Year.ToString();
        MonthTextBlock.Text = Bill.Month.ToString();
        ActivitiesSumTextBlock.Text = Bill.ActivitiesSum.ToString();
        YearSubsTextBlock.Text = Bill.YearSubsSum.ToString();
        ContractFeeTextBlock.Text = Bill.ContractFeeSum.ToString();
        OverallTextBlock.Text = (Bill.ActivitiesSum + Bill.PrecontractSum + Bill.YearSubsSum + Bill.ContractFeeSum).ToString();
    }
   
}