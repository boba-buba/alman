using AlmanUI.ViewModels;
using AlmanUI.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using System.Globalization;
using System.Threading;

namespace AlmanUI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                bool loginSuccessful = false;
                while(!loginSuccessful)
                {
                    var loginWindow = new LoginWindow();
                    // Wait for the login dialog to complete
                    loginSuccessful = await loginWindow.ShowLoginDialogAsync();
                }

                

                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT

                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
                desktop.MainWindow.Show();
                
                
            }
            
            base.OnFrameworkInitializationCompleted();
        }

    }
}