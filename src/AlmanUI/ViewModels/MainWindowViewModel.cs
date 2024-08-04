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


namespace AlmanUI.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isPaneOpen = true;

        [ObservableProperty]
        private ViewModelBase _currentPage = new PrecontractsPageViewModel();

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
            new ListItemTemplate(typeof(HomePageViewModel),  MaterialIconKind.Home),
            new ListItemTemplate(typeof(ChildrenPageViewModel),  MaterialIconKind.BabyFaceOutline),
            new ListItemTemplate(typeof(StaffPageViewModel), MaterialIconKind.AccountGroupOutline),
            new ListItemTemplate(typeof(ActivitiesPageViewModel), MaterialIconKind.PaletteOutline),
            new ListItemTemplate(typeof(YearMonthActivitiesPageViewModel), MaterialIconKind.CalendarMonthOutline),
            new ListItemTemplate(typeof(PositionsPageViewModel), MaterialIconKind.BriefcaseOutline),
            new ListItemTemplate(typeof(PrecontractsPageViewModel), MaterialIconKind.FileSign),
        };

    }

    public class ListItemTemplate
    {
        public ListItemTemplate(Type type, MaterialIconKind icon)
        {
            ModelType = type;
            Label = type.Name.Replace("PageViewModel", ""); 
            ListItemIcon = icon;
        }
        public string Label { get; }
        public Type ModelType { get; }

        public MaterialIconKind ListItemIcon { get; }
    }
}
