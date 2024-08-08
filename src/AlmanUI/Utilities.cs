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
        if (sender is TextBox textBox)
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
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Convert int to string for display in the DataGrid
        return value?.ToString() ?? "0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Convert string back to int, returning 0 if the string is empty
        if (int.TryParse(value as string, out int result))
        {
            return result;
        }
        return 0;
    }
}
