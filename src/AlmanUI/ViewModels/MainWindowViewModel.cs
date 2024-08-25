using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Material.Icons.Avalonia;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Material.Icons;


namespace AlmanUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isPaneOpen = true;

    [ObservableProperty]
    private ViewModelBase _currentPage = new HomePageViewModel();

    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    [RelayCommand]
    public void TriggerPaneCommand()
    {
        IsPaneOpen = !IsPaneOpen;
    }

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

public class ListItemTemplate
{
    public ListItemTemplate(Type type, MaterialIconKind icon, string label)
    {
        ModelType = type;
        Label = label;
        ListItemIcon = icon;
    }
    public string Label { get; }
    public Type ModelType { get; }

    public MaterialIconKind ListItemIcon { get; }
}
