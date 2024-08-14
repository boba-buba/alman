using Avalonia.Controls.Templates;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Alman.SharedModels;
using AlmanUI.ViewModels;
using Avalonia;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmanUI;

public static class UIControlElements
{
    public static void AddNumericUpDownToGrid<TEntity>(DataGrid gridToAddTo, string headerName, string bindingName, int min, int max) where TEntity : class
    {
        gridToAddTo.Columns.Add(new DataGridTemplateColumn
        {
            Header = headerName,  // Example header for the column
            CellTemplate = new FuncDataTemplate<TEntity>((item, namescope) =>
            {
                // Create a NumericUpDown control
                var numericUpDown = new NumericUpDown
                {
                    Minimum = min,        // Define minimum value
                    Maximum = max,   // Define maximum value
                    Increment = 1,    // Define step size
                };

                // Define the binding for the value property
                var binding = new Binding(bindingName) // Bind to the 'Salary' property of the data item
                {
                    Mode = BindingMode.TwoWay,                         // Ensure two-way binding
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // Update the source whenever the value changes
                };

                // Bind the NumericUpDown's Value property to the Salary property
                numericUpDown.Bind(NumericUpDown.ValueProperty, binding);

                return numericUpDown; // Return the NumericUpDown control as the cell template
            })
        });

    }

    public static void AddCheckBoxToGrid<TEntity>(DataGrid gridToAddTo, string headerName, string bindingName) where TEntity : class
    {
        gridToAddTo.Columns.Add(new DataGridTemplateColumn
        {
            Header = headerName,
            CellTemplate = new FuncDataTemplate<TEntity>((x, _) =>
            {
                var checkBox = new CheckBox();
                var binding = new Binding(bindingName)
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new IntToBoolConverter(),  // Apply the converter here
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // Optional: Immediately update the source
                };
                checkBox.Bind(CheckBox.IsCheckedProperty, binding);
                return checkBox;
            })
        });
    }

    public static void AddNumericTextBoxToGrid<TEntity>(DataGrid gridToAddTo, string headerName, string bindingName) where TEntity : class
    {
        gridToAddTo.Columns.Add(new DataGridTemplateColumn
        {
            Header = headerName,
            CellTemplate = new FuncDataTemplate<TEntity>((x, _) => 
            {
                var textBox = new TextBox();
                var binding = new Binding(bindingName)
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new IntToStringConverter(),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;
                textBox.Bind(TextBox.TextProperty, binding);
                return textBox;
            })
        });
    }


    public static void AddComboBoxToDataGrid<TEntity>(DataGrid gridToAddTo, string headerName, string bindingName, IValueConverter converter)
    {
        gridToAddTo.Columns.Add(new DataGridTemplateColumn
        {
            Header = headerName,
            CellTemplate = new FuncDataTemplate<TEntity>((x, _) =>
            {
                var comboBox = new ComboBox
                {
                    ItemsSource = new string[] { "Option 1", "Option 2", "Option 3" },  // Options to display

                };

                var binding = new Binding(bindingName) // Bind to the 'Role' property of the data item
                {
                    Mode = BindingMode.TwoWay,                         // Ensure two-way binding
                    Converter = converter,            // Apply the converter
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // Update the source whenever the value changes
                };

                // Bind the ComboBox's SelectedItem property to the Role property
                comboBox.Bind(ComboBox.SelectedItemProperty, binding);

                return comboBox;
            })
        });
    }
}
