using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlmanUI.Resources;

namespace AlmanUI;

/// <summary>
/// Class that peovides utility checks for UI controls.
/// </summary>
public static class UIUtilities
{
    /// <summary>
    /// Secures that textbox that accept numeric input will accept only numbers and nothing els.
    /// </summary>
    /// <param name="sender">Control (TextBox here)</param>
    /// <param name="e"></param>
    public static void TextBox_NumericInput_KeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is TextBox)
        {
            // Allow control keys (Backspace, Delete, Arrow keys, etc.)
            if (e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.Left || e.Key == Key.Right ||
                e.Key == Key.Tab || e.Key == Key.Enter)
            {
                return;
            }

            // Check if the pressed key is a digit
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9) &&
                !(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                // If not a digit, mark the event as handled, so the key is not processed
                e.Handled = true;
            }
        }
    }

    /// <summary>
    /// Doesn't accept anything from keyboard (except for Delete key).
    /// </summary>
    /// <param name="sender">NumericUpDown control instance.</param>
    /// <param name="e">event that holds info about what what key was pressed.</param>
    public static void NumericUpDown_KeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is NumericUpDown)
        {
            e.Handled = true;
        }
    }
}

/// <summary>
/// Secure that if there is nothing in textbox that accepts numeric input, there is 0 by default.
/// It is done to get rid of the exception that is shown when user deletes everything from textbox.
/// </summary>
public class IntToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Convert int to string for display in the DataGrid
        return value?.ToString() ?? "0";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Convert string back to int, returning 0 if the string is empty
        if (int.TryParse(value as string, out int result))
        {
            return result;
        }
        return 0;
    }
}

/// <summary>
/// Boolean Coverter for the checkbox control.
/// </summary>
public class IntToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is int intValue && intValue == 1;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return 0;
        }
        return (bool)value ? 1 : 0;
    }
}

/// <summary>
/// Requires the arry of the keys for the converters.
/// </summary>
public interface IKeys
{
    /// <summary>
    /// Array of keys of possible values.
    /// </summary>
    public string[] Keys { get; }
}

public interface ICustomValueConverter : IValueConverter, IKeys
{ }

/// <summary>
/// Child contract type converter, because in database the types stored as numbers.
/// </summary>
public class ContractTypeConverter : ICustomValueConverter
{
    /// <summary>
    /// Map string values to corresponding integer values
    /// </summary>
    private readonly Dictionary<string, int> stringToIntMap = new Dictionary<string, int>
    {
        { Resources.ChildrenResources.PrecontractType, 0 },
        { Resources.ChildrenResources.MotherCapitalType, 1 },
        { Resources.ChildrenResources.OrdinaryContractType, 2 },
        { Resources.ChildrenResources.StaffChildType, 3},
    };

    /// <summary>
    /// Map integer values back to corresponding string values
    /// </summary>
    private readonly Dictionary<int, string> intToStringMap = new Dictionary<int, string>
    {
        { 0, Resources.ChildrenResources.PrecontractType },
        { 1, Resources.ChildrenResources.MotherCapitalType},
        { 2, Resources.ChildrenResources.OrdinaryContractType},
        { 3, Resources.ChildrenResources.StaffChildType},
    };

    public string[] Keys { get; } = [Resources.ChildrenResources.PrecontractType, Resources.ChildrenResources.MotherCapitalType, Resources.ChildrenResources.OrdinaryContractType, Resources.ChildrenResources.StaffChildType];

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue && intToStringMap.ContainsKey(intValue))
        {
            return intToStringMap[intValue];
        }
        return string.Empty; // Default if no match
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue && stringToIntMap.ContainsKey(stringValue))
        {
            return stringToIntMap[stringValue];
        }
        return 0; // Default to 0 if no match
    }
}

/// <summary>
/// Way of paying converter, because in database it is tored as number.
/// </summary>
public class WayOfPayingConverter : ICustomValueConverter
{
    /// <summary>
    /// Map string values to corresponding integer values
    /// </summary>
    private readonly Dictionary<string, int> stringToIntMap = new Dictionary<string, int>
    {
        { Resources.CommonResources.CashType, 1 },
        { Resources.CommonResources.MoneyTransferType, 2 }
    };

    /// <summary>
    /// Map integer values back to corresponding string values
    /// </summary>
    private readonly Dictionary<int, string> intToStringMap = new Dictionary<int, string>
    {
        { 1, Resources.CommonResources.CashType},
        { 2, Resources.CommonResources.MoneyTransferType }
    };

    public string[] Keys { get; } = [Resources.CommonResources.CashType, Resources.CommonResources.MoneyTransferType ];


    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue && intToStringMap.ContainsKey(intValue))
        {
            return intToStringMap[intValue];
        }
        return string.Empty; // Default if no match
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue && stringToIntMap.ContainsKey(stringValue))
        {
            return stringToIntMap[stringValue];
        }
        return 0; // Default to 0 if no match
    }
}

/// <summary>
/// Language converter.
/// </summary>
public class CultureConverter : ICustomValueConverter
{

    private readonly Dictionary<string, string> UIToCultureStrMap = new Dictionary<string, string>
    {
        { "RUS", "ru-RU" },
        { "EN", "en-EN"}
    };

    private readonly Dictionary<string, string> CultureToUIStrMap = new Dictionary<string, string>
    {
        { "ru-RU", "RUS" },
        { "en-EN", "EN"}
    };

    public string[] Keys { get; } = ["RUS", "EN"];


    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string cultureStr && CultureToUIStrMap.ContainsKey(cultureStr))
        {
            return CultureToUIStrMap[cultureStr];
        }
        return string.Empty; // Default if no match
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue && UIToCultureStrMap.ContainsKey(stringValue))
        {
            return UIToCultureStrMap[stringValue];
        }
        return 0; // Default to 0 if no match
    }
}
