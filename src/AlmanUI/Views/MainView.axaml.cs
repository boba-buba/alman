using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace AlmanUI.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    public void LogInButtonClicked(object source, RoutedEventArgs args)
    {
        Debug.Write("Click");
    }
}
