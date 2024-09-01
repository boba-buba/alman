using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;

namespace AlmanUI.Views;

public partial class ChildBillWindow : Window
{

    public ChildBill Bill { get; private set; }
    public ChildBillWindow()
    {
        InitializeComponent();
    }

    public void SetChildBill(ChildBill childBill)
    {
        Bill = childBill;
        DataContext = new ChildBillWindowViewModel(childBill);
        LoadItems();
    }

    private void LoadItems()
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