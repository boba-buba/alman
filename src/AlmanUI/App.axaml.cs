using AlmanUI.ViewModels;
using AlmanUI.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

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

                var loginWindow = new LoginWindow();

                // Wait for the login dialog to complete
                bool loginSuccessful = await loginWindow.ShowLoginDialogAsync();
                if (loginSuccessful)
                {
                    // Line below is needed to remove Avalonia data validation.
                    // Without this line you will get duplicate validations from both Avalonia and CT
                    BindingPlugins.DataValidators.RemoveAt(0);
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = new MainWindowViewModel(),
                    };
                    desktop.MainWindow.Show();
                }
                else
                {
                    // Exit the application if the login was not successful
                    desktop.Shutdown();
                }
                
                
            }
            
            base.OnFrameworkInitializationCompleted();
        }
    }
}