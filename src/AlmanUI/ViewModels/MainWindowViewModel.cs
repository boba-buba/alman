using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using System;
using System.Collections.ObjectModel;

namespace AlmanUI.ViewModels;

/// <summary>
/// ViewModel for the main window data.
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Flag that indicates if the pane is open.
    /// </summary>
    [ObservableProperty]
    private bool _isPaneOpen = true;

    /// <summary>
    /// The page that is shown currenb=ntly in the View.
    /// </summary>
    [ObservableProperty]
    private ViewModelBase _currentPage = new HomePageViewModel();

    /// <summary>
    /// Selected page in the view.
    /// </summary>
    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    /// <summary>
    /// Change the state of the pane (close/open) to the opposite.
    /// </summary>
    [RelayCommand]
    public void TriggerPaneCommand()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    /// <summary>
    /// Load newly chosen page to show in the view.
    /// </summary>
    /// <param name="value"></param>
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null)
        {
            return;
        }
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance is null)
        {
            return;
        }
        try
        {
            CurrentPage = (ViewModelBase)instance;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Collection of possible pages.
    /// </summary>
    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(HomePageViewModel),  MaterialIconKind.Home, AlmanUI.Resources.HomePageResources.HomePageName),
        new ListItemTemplate(typeof(ChildrenPageViewModel),  MaterialIconKind.BabyFaceOutline, AlmanUI.Resources.ChildrenResources.ChildrenPageName),
        new ListItemTemplate(typeof(StaffPageViewModel), MaterialIconKind.AccountGroupOutline, AlmanUI.Resources.StaffResources.StaffPageName),
        new ListItemTemplate(typeof(ActivitiesPageViewModel), MaterialIconKind.PaletteOutline, AlmanUI.Resources.ChildrenResources.ActivitiesPageName),
        new ListItemTemplate(typeof(YearMonthActivitiesPageViewModel), MaterialIconKind.CalendarMonthOutline, AlmanUI.Resources.ChildrenResources.YearMonthActivitiesPageName),
        new ListItemTemplate(typeof(PrecontractsPageViewModel), MaterialIconKind.FileSign, AlmanUI.Resources.ChildrenResources.PrecontractsPageName),
        new ListItemTemplate(typeof(ContractFeesPageViewModel), MaterialIconKind.AccountCreditCardOutline, AlmanUI.Resources.ChildrenResources.ContractFeesPageName),
        new ListItemTemplate(typeof(YearSubsPageViewModel), MaterialIconKind.CashClock, AlmanUI.Resources.ChildrenResources.YearSubsPageName),
        new ListItemTemplate(typeof(FinalPaymentsPageViewModel), MaterialIconKind.Cash, AlmanUI.Resources.StaffResources.StaffPaymentsPageName),
        new ListItemTemplate(typeof(StaffActivitiesPageViewModel), MaterialIconKind.DrawingBox, AlmanUI.Resources.StaffResources.StaffActivitiesPageName),
        new ListItemTemplate(typeof(YearMonthStaffActivitiesPageViewModel), MaterialIconKind.CalendarOutline, AlmanUI.Resources.StaffResources.YearMonthStaffActivitiesPageName),
        new ListItemTemplate(typeof(YearMonthOtherPageViewModel), MaterialIconKind.Flower, AlmanUI.Resources.OtherResources.YearMonthOtherPageName),
        new ListItemTemplate(typeof(ExpensesPageViewModel), MaterialIconKind.CreditCardOutline, AlmanUI.Resources.OtherResources.ExpensesPageName),
    };
}

/// <summary>
/// Utility class to store data of the page.
/// </summary>
public class ListItemTemplate
{
    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="type">Type of the page.</param>
    /// <param name="icon">Icon for the pane.</param>
    /// <param name="label">Name of the page.</param>
    public ListItemTemplate(Type type, MaterialIconKind icon, string label)
    {
        ModelType = type;
        Label = label;
        ListItemIcon = icon;
    }

    /// <summary>
    /// Name of the page for the view.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Type of the ViewModel.
    /// </summary>
    public Type ModelType { get; }

    /// <summary>
    /// Icon for the pane.
    /// </summary>
    public MaterialIconKind ListItemIcon { get; }
}
