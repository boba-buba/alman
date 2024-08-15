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
                return CreateCheckBox(bindingName);
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
                return CreateNumericTextBox(bindingName);
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
                return CreateComboBox(converter, bindingName);
            })
        });
    }

    public static void AddMoneyTextBox<TEntity>(DataGrid gridToAddTo, string textBoxProperty, string WayOfPayProperty, string headerName)
    {
        var moneyTemplate = new FuncDataTemplate<TEntity>((x, _) =>
        {
            Grid cellGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Star)
                }
            };
            //TextBox
            TextBox moneyTextBox = UIControlElements.CreateNumericTextBox(textBoxProperty);
            //CombBox
            ComboBox wayOfPayCombobox = UIControlElements.CreateComboBox(new WayOfPayingConverter(), WayOfPayProperty);
            //Adding to cellGrid
            cellGrid.Children.Add(moneyTextBox);
            Grid.SetColumn(moneyTextBox, 0);
            cellGrid.Children.Add(wayOfPayCombobox);
            Grid.SetColumn(wayOfPayCombobox, 1);

            return cellGrid;
        });

        gridToAddTo.Columns.Add(new DataGridTemplateColumn
        {
            Header = headerName,
            CellTemplate = moneyTemplate,
        });
    }

    public static TextBox CreateNumericTextBox(string bindingName)
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
    }

    public static CheckBox CreateCheckBox(string bindingName)
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
    }

    public static ComboBox CreateComboBox(IValueConverter converter, string bindingName)
    {
        var comboBox = new ComboBox
        {
            ItemsSource = ((IKeys)converter).Keys,  // Options to display
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
    }
}
