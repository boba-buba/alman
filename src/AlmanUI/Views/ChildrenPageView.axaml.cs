using AlmanUI.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System.Linq;

namespace AlmanUI.Views;

public partial class ChildrenPageView : UserControl
{
    public ChildrenPageView()
    {
        InitializeComponent();
        this.DataContext = new ChildrenPageViewModel();
        var col = ChildrenDataGrid.Columns.Last();
        col.Bind(ComboBox.ItemsSourceProperty, new Binding { Path = "DataContext.ChildrenContractNames", Source = DataContext });
    }


}