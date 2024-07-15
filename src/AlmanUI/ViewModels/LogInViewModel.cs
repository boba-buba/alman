using AlmanUI.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Transactions;

using AlmanUi.Models;
public partial class LogInViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string? _password;

    public LogInViewModel() { }

    public LogInViewModel(LogIn login)
    {
        Username = login.Username;
        Password = login.Password;
    }

    public LogIn GetLogIn()
    {
        return new LogIn() { Username = this.Username, Password = this.Password };
    }
}
