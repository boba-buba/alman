using AlmanUI.Controls;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace AlmanUI.Views;

/// <summary>
/// View for login window.
/// </summary>
public partial class LoginWindow : Window
{
    /// <summary>
    /// Completeion of the login.
    /// </summary>
    private TaskCompletionSource<bool> _loginTaskCompletionSource;

    /// <summary>
    /// Chosen language for the UI.
    /// </summary>
    private string _selectedLanguage = "";

    /// <summary>
    /// Selected language for the UI.
    /// </summary>
    public string SelectedLanguage
    {
        get => string.IsNullOrEmpty(_selectedLanguage) ? "lang" : _selectedLanguage;
        set => _selectedLanguage = value;
    }

    /// <summary>
    /// ctor that initializes all controls.
    /// </summary>
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
        _loginTaskCompletionSource = new();
    }

    /// <summary>
    /// Simulation of show dialog (like windows have).
    /// </summary>
    /// <returns>Task result that indicates if the login was successful or not.</returns>
    public Task<bool> ShowLoginDialogAsync()
    {
        _loginTaskCompletionSource = new TaskCompletionSource<bool>();

        // Show the window non-modally (without blocking the calling thread)
        this.Show();

        // Return the task that will complete when the login button is clicked
        return _loginTaskCompletionSource.Task;
    }

    /// <summary>
    /// Login button is clicked, the load the tex boxes to write password and name.
    /// </summary>
    /// <param name="sender">Button <seealso cref="LoginActivateButton"/>.</param>
    /// <param name="e"></param>
    private void OnLOginActivateClick(object? sender, EventArgs e)
    {
        LoginActivateButton.IsVisible = false;
        UsernameTextBox.IsVisible = true;
        PasswordBox.IsVisible = true;
        UsernameText.IsVisible = true;
        PasswordText.IsVisible = true;
        EnterButton.IsVisible = true;
    }

    /// <summary>
    /// Enter button is clicked, then check if login is valid.
    /// </summary>
    /// <param name="sender">Button <seealso cref="EnterButton"/>.</param>
    /// <param name="e"></param>
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

    /// <summary>
    /// Check  if login data are valid.
    /// </summary>
    /// <param name="username">User's username.</param>
    /// <param name="password">User's password.</param>
    /// <returns>True if user with such name and password is in database, false otherwise.</returns>
    private bool IsValidLogin(string? username, string? password)
    {
        return username is not null && password is not null && UserUIControl.UserInDb(username, password);
    }


    /// <summary>
    /// Another language is chosen, load the necessary resources.
    /// </summary>
    /// <param name="sender">Combobox.</param>
    /// <param name="e"></param>
    private void OnLanguageChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems is not null && e.AddedItems.Count > 0)
        {

            ComboBoxItem selectedItem = (ComboBoxItem)e.AddedItems[0]!;
            string selectedLanguage = selectedItem.Tag!.ToString()!;
            SetCulture(selectedLanguage);
            _loginTaskCompletionSource.SetResult(false);
            this.Close();
        }
    }

    /// <summary>
    /// Set culture based on user's choise.
    /// </summary>
    /// <param name="cultureString">Language string.</param>
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