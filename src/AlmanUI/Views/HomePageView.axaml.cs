using AlmanUI.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace AlmanUI.Views;

public partial class HomePageView : UserControl
{
    public HomePageView()
    {
        InitializeComponent();
        ExportToExcelDumpButton.Click += async (sender, e) => await OnExportToExcelDumpButtonClickAsync();
    }

    private async Task OnExportToExcelDumpButtonClickAsync()
    {
        // Disable the entire window
        this.IsEnabled = false;

        try
        {
            // Simulate an async operation
            await DumpDbControl.DumpDbAsync();
        }
        finally
        {
            // Re-enable the window after the operation is complete
            this.IsEnabled = true;
        }
    }

}