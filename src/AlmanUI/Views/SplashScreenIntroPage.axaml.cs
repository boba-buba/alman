using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System.Threading.Tasks;
using System;
using AlmanUI.Controls;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Windows;
namespace AlmanUI.Views;

public partial class SplashScreenIntroPage : Window
{
    public SplashScreenIntroPage()
    {
        InitializeComponent();
    }
    
    public bool IsLoginSuccessful { get; private set; } = false;

    private void OnLoginClicked(object? sender, EventArgs e)
    {
        var username = UsernameTextBox.Text;
        var password = PasswordBox.Text;

        // Validate the username and password (for demo purposes, we'll hard-code valid credentials)
        if (username is not null && password is not null && UserUIControl.UserInDb(username, password))
        {
            IsLoginSuccessful = true;
            // Close the login window and open the main window
            this.Close();

            //var mainWindow = new MainWindow();
            //mainWindow.Show();
        }
        else
        {
            // Display an error message
            var messageBox = new Window
            {
                Content = new TextBlock { Text = "Invalid username or password!" },
                Width = 300,
                Height = 100,
            };
            messageBox.ShowDialog(this);
        }
    }


    public async Task InitApp()
    {

        var start = DateTime.Now.Ticks;
        var time = start;
        var progressValue = 0;

        while ((time - start) < TimeSpan.TicksPerSecond)
        {
            progressValue++;
            //Dispatcher.UIThread.Post(() => ProgressBar1.Value = progressValue);
            await Task.Delay(25);
            time = DateTime.Now.Ticks;
        }

        start = time;
        //Dispatcher.UIThread.Post(() => LoadingText.Text = "Initializing Application Settings...");
        var limit = TimeSpan.TicksPerSecond * 2;
        while ((time - start) < limit)
        {
            progressValue += 1;
            //Dispatcher.UIThread.Post(() => ProgressBar1.Value = progressValue);
            await Task.Delay(50);
            time = DateTime.Now.Ticks;
        }

        //Dispatcher.UIThread.Post(() => LoadingText.Text = "Preparing App...");

        while (progressValue < 100)
        {
            progressValue += 1;
            //Dispatcher.UIThread.Post(() => ProgressBar1.Value = progressValue);
            await Task.Delay(10);
        }
    }

    public async Task LogIn()
    {
        var login = new LoginWindow();

        await login.ShowDialog(this);
    }
}
