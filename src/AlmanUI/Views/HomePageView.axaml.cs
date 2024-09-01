using AlmanUI.Controls;
using Avalonia.Controls;
using System.Threading.Tasks;

namespace AlmanUI.Views;

/// <summary>
/// View for the home page.
/// </summary>
public partial class HomePageView : UserControl
{
    /// <summary>
    /// ctor.
    /// </summary>
    public HomePageView()
    {
        InitializeComponent();
        ExportToExcelDumpButton.Click += async (sender, e) => await OnExportToExcelDumpButtonClickAsync();
    }

    /// <summary>
    /// Process click on button to dump database to Excel file.
    /// </summary>
    /// <returns></returns>
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