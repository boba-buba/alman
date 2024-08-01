using System;

namespace AlmanUI.Mediator;
public class Mediator
{
    private static readonly Lazy<Mediator> lazy = new Lazy<Mediator>(() => new Mediator());

    public static Mediator Instance => lazy.Value;

    private Mediator() { }

    public event Action<string>? Notify;

    public event Action<string, int, int>? NotifyWithParams;

    public void Send(string message)
    {
        Notify?.Invoke(message);
    }

    public void SendWithParams(string message, int year, int month)
    {
        NotifyWithParams?.Invoke(message, year, month);
    }
}
