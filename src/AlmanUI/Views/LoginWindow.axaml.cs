using AlmanUI.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SQLitePCL;
using System;
using Avalonia.Threading;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using AlmanUI.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using System.Threading;

namespace AlmanUI.Views;

public partial class LoginWindow : Window
{
    private TaskCompletionSource<bool>? _loginTaskCompletionSource;

    private string _selectedLanguage;
    public string SelectedLanguage
    {
        get => string.IsNullOrEmpty(_selectedLanguage) ? "lang" : _selectedLanguage;
        set => _selectedLanguage = value;
    }

    public LoginWindow()
    {
        InitializeComponent();
        UsernameTextBox.IsVisible = false;
        PasswordBox.IsVisible = false;
        UsernameText.IsVisible = false;
        PasswordText.IsVisible = false;
        EnterButton.IsVisible = false;
        WrongDataTextBlock.IsVisible = false;
        EnterButton.Click += OnLoginClick;
        LoginActivateButton.Click += OnLOginActivateClick;
    }


    public Task<bool> ShowLoginDialogAsync()
    {
        _loginTaskCompletionSource = new TaskCompletionSource<bool>();

        // Show the window non-modally (without blocking the calling thread)
        this.Show();

        // Return the task that will complete when the login button is clicked
        return _loginTaskCompletionSource.Task;
    }

    private void OnLOginActivateClick(object? sender, EventArgs e)
    {
        LoginActivateButton.IsVisible = false;
        UsernameTextBox.IsVisible = true;
        PasswordBox.IsVisible = true;
        UsernameText.IsVisible = true;
        PasswordText.IsVisible = true;
        EnterButton.IsVisible = true;
    }

    private void OnLoginClick(object? sender, RoutedEventArgs e)
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
            WrongDataTextBlock.IsVisible = true;
        }
    }

    private bool IsValidLogin(string? username, string? password)
    {
        return username is not null && password is not null && UserUIControl.UserInDb(username, password);
    }


    public bool IsLoginSuccessful { get; private set; } = false;


    private void OnLanguageChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {

            var selectedItem = (ComboBoxItem)e.AddedItems[0]!;
            var selectedLanguage = selectedItem.Tag.ToString();
            SetCulture(selectedLanguage);
            _loginTaskCompletionSource.SetResult(false);
            this.Close();
        }

    }

    private void SetCulture(string cultureString)
    {
        AlmanUI.Resources.Resources.Culture = new CultureInfo(cultureString);
        AlmanUI.Resources.ChildrenResources.Culture = new CultureInfo(cultureString);
        AlmanUI.Resources.CommonResources.Culture = new CultureInfo(cultureString);
        AlmanUI.Resources.HomePageResources.Culture = new CultureInfo(cultureString);
        AlmanUI.Resources.StaffResources.Culture = new CultureInfo(cultureString);
        AlmanUI.Resources.OtherResources.Culture = new CultureInfo(cultureString);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureString);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureString);

    }
}