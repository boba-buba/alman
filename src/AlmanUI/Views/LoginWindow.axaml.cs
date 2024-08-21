using AlmanUI.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SQLitePCL;
using System;
using Avalonia.Threading;
using System.Threading.Tasks;
using Avalonia.Interactivity;

namespace AlmanUI.Views;

public partial class LoginWindow : Window
{
    private TaskCompletionSource<bool> _loginTaskCompletionSource;

    public LoginWindow()
    {
        InitializeComponent();
        LoginButton.Click += OnLoginClick;
    }

    public Task<bool> ShowLoginDialogAsync()
    {
        _loginTaskCompletionSource = new TaskCompletionSource<bool>();

        // Show the window non-modally (without blocking the calling thread)
        this.Show();

        // Return the task that will complete when the login button is clicked
        return _loginTaskCompletionSource.Task;
    }

    private void OnLoginClick(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text;
        var password = PasswordBox.Text;

        if (IsValidLogin(username, password))
        {
            _loginTaskCompletionSource.SetResult(true);  // Set the result to true on successful login
            this.Close();
        }
        else
        {
            // Show error message or handle failed login
            _loginTaskCompletionSource.SetResult(false); // Optionally handle false for failed login
        }
    }

    private bool IsValidLogin(string username, string password)
    {
        // Replace with your actual validation logic
        return username is not null && password is not null && UserUIControl.UserInDb(username, password);
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

}