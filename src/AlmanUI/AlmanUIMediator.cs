using Alman.SharedModels;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Runtime.InteropServices.Marshalling;

namespace AlmanUI.Mediator;
public class Mediator
{
    private static readonly Lazy<Mediator> lazy = new Lazy<Mediator>(() => new Mediator());

    public static Mediator Instance => lazy.Value;

    private Mediator() { }

    public event Action<string>? Notify;

    public event Action<string, int, int>? NotifyWithParams;

    public event Action<string, int>? NotifyWithOneParam;

    public event Action<string, int, int, IReadOnlyList<IYearMonthOtherBase>>? NotifyWithThreeParams;

    public void Send(string message)
    {
        Notify?.Invoke(message);
    }

    public void SendWithParams(string message, int year, int month)
    {
        NotifyWithParams?.Invoke(message, year, month);
    }

    public void SendWithThreeParams(string message, int year, int month, IReadOnlyList<IYearMonthOtherBase> items)
    {
        NotifyWithThreeParams?.Invoke(message, year, month, items);
    }
    public void SendWithOneParam(string message, int year)
    {
        NotifyWithOneParam?.Invoke(message, year);
    }
}
