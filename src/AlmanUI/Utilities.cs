using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI;

public static class UIUtilities
{
    public static void TextBox_NumericInput_KeyDown(object sender, KeyEventArgs e)
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


}

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


public interface IKeys
{
    public string[] Keys { get; }
}

public class ContractTypeConverter : IValueConverter, IKeys
{
    // Map string values to corresponding integer values
    private readonly Dictionary<string, int> stringToIntMap = new Dictionary<string, int>
    {
        { "Option 1", 1 },
        { "Option 2", 2 },
        { "Option 3", 3 }
    };

    // Map integer values back to corresponding string values
    private readonly Dictionary<int, string> intToStringMap = new Dictionary<int, string>
    {
        { 1, "Option 1" },
        { 2, "Option 2" },
        { 3, "Option 3" }
    };

    public string[] Keys { get; } = ["Option 1", "Option 2", "Option 3" ];


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



public class WayOfPayingConverter : IValueConverter, IKeys
{
    // Map string values to corresponding integer values
    private readonly Dictionary<string, int> stringToIntMap = new Dictionary<string, int>
    {
        { "Cash", 1 },
        { "Money Transfer", 2 }
    };

    // Map integer values back to corresponding string values
    private readonly Dictionary<int, string> intToStringMap = new Dictionary<int, string>
    {
        { 1, "Cash" },
        { 2, "Money Transfer" }
    };

    public string[] Keys { get; } = [ "Cash", "Money Transfer" ];


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
