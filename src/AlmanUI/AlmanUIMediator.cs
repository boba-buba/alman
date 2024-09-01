using Alman.SharedModels;
using System;
using System.Collections.Generic;

namespace AlmanUI.Mediator;

/// <summary>
/// Mediator that sends signals to view from viewmodel when model changed.
/// </summary>
public class Mediator
{
    /// <summary>
    /// 
    /// </summary>
    private static readonly Lazy<Mediator> lazy = new Lazy<Mediator>(() => new Mediator());
    
    /// <summary>
    /// Instance of the Mediator.
    /// </summary>
    public static Mediator Instance => lazy.Value;

    /// <summary>
    /// ctor.
    /// </summary>
    private Mediator() { }

    /// <summary>
    /// Action without params.
    /// </summary>
    public event Action<string>? Notify;

    /// <summary>
    /// Action with 2 int parameters.
    /// </summary>
    public event Action<string, int, int>? NotifyWithParams;

    /// <summary>
    /// Action with one int param.
    /// </summary>
    public event Action<string, int>? NotifyWithOneParam;

    /// <summary>
    /// Action with 2 int and 1 read-only list params.
    /// </summary>
    public event Action<string, int, int, IReadOnlyList<IYearMonthOtherBase>>? NotifyWithThreeParams;

    /// <summary>
    /// Send message without params with name <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The message that is sent to the view.</param>
    public void Send(string message)
    {
        Notify?.Invoke(message);
    }

    /// <summary>
    /// Send message with 2 params.
    /// </summary>
    /// <param name="message">Message that is sent to the view.</param>
    /// <param name="year">1st int param.</param>
    /// <param name="month">2nd int params.</param>
    public void SendWithParams(string message, int year, int month)
    {
        NotifyWithParams?.Invoke(message, year, month);
    }

    /// <summary>
    /// Send message with 3 params.
    /// </summary>
    /// <param name="message">Message that is sent to the view.</param>
    /// <param name="year">1st int param.</param>
    /// <param name="month">2nd int params.</param>
    /// <param name="items">list of items to update view with.</param>
    public void SendWithThreeParams(string message, int year, int month, IReadOnlyList<IYearMonthOtherBase> items)
    {
        NotifyWithThreeParams?.Invoke(message, year, month, items);
    }

    /// <summary>
    /// Send message with 1 param.
    /// </summary>
    /// <param name="message">Message that is sent to the view.</param>
    /// <param name="year">1st int param.</param>
    public void SendWithOneParam(string message, int year)
    {
        NotifyWithOneParam?.Invoke(message, year);
    }
}
