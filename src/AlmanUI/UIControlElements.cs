using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Material.Icons;
using Material.Icons.Avalonia;
using System.Diagnostics;

namespace AlmanUI;

/// <summary>
/// API to create controls for UI.
/// </summary>
public static class UIControlElements
{
    /// <summary>
    /// Create numeric upDown and add it to the datagrid <paramref name="gridToAddTo"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity from which the data are bound to the control.</typeparam>
    /// <param name="gridToAddTo">DataGrid to whic the controll is added.</param>
    /// <param name="headerName">Name of the column in datagrid.</param>
    /// <param name="bindingName">Name of the property in <typeparamref name="TEntity"/> that is bound as source for the control.</param>
    /// <param name="min">Minimal value (down)</param>
    /// <param name="max">Maximal value (up)</param>
    public static void AddNumericUpDownToGrid<TEntity>(DataGrid gridToAddTo, string headerName, string bindingName, int min, int max) where TEntity : class
    {
        gridToAddTo.Columns.Add(new DataGridTemplateColumn
        {
            Header = headerName,  // Example header for the column
            CellTemplate = new FuncDataTemplate<TEntity>((item, namescope) =>
            {
                Debug.WriteLine(nameof(TEntity));

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

    /// <summary>
    /// Create the checkbox and add to the datagrid <paramref name="gridToAddTo"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity from which the data are bound to the control.</typeparam>
    /// <param name="gridToAddTo">DataGrid to whic the controll is added.</param>
    /// <param name="headerName">Name of the column in datagrid.</param>
    /// <param name="bindingName">Name of the property in <typeparamref name="TEntity"/> that is bound as source for the control.</param>
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

    /// <summary>
    /// Create numeric text box and add it to the datagrid.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity from which the data are bound to the control.</typeparam>
    /// <param name="gridToAddTo">DataGrid to whic the controll is added.</param>
    /// <param name="headerName">Name of the column in datagrid.</param>
    /// <param name="bindingName">Name of the property in <typeparamref name="TEntity"/> that is bound as source for the control.</param>
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

    /// <summary>
    /// Create combobox and add it to the datagrid <paramref name="gridToAddTo"/>. 
    /// </summary>
    /// <typeparam name="TEntity">Type of entity from which the data are bound to the control.</typeparam>
    /// <param name="gridToAddTo">DataGrid to whic the controll is added.</param>
    /// <param name="headerName">Name of the column in datagrid.</param>
    /// <param name="bindingName">Name of the property in <typeparamref name="TEntity"/> that is bound as source for the control.</param>
    /// <param name="converter">Converter for the possible values in combobox as the values in database usually stored as numbers.</param>
    public static void AddComboBoxToDataGrid<TEntity>(DataGrid gridToAddTo, string headerName, string bindingName, ICustomValueConverter converter)
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

    /// <summary>
    /// Create textbox that consists of textbox (for sum input) and combobox (for way of paying) and add it to the datagrid <paramref name="gridToAddTo"/>. 
    /// </summary>
    /// <typeparam name="TEntity">Type of entity from which the data are bound to the control.</typeparam>
    /// <param name="gridToAddTo">DataGrid to whic the controll is added.</param>
    /// <param name="headerName">Name of the column in datagrid.</param>
    /// <param name="textBoxProperty">Property of the entity <typeparamref name="TEntity"/> that is bound to the textbox as source.</param>
    /// <param name="WayOfPayProperty">Property of the entity <typeparamref name="TEntity"/> that is bound to the combobox as source.</param>
    public static void AddMoneyTextBox<TEntity>(DataGrid gridToAddTo, string textBoxProperty, string WayOfPayProperty, string headerName)
    {
        var moneyTemplate = new FuncDataTemplate<TEntity>((x, _) =>
        {
            Debug.WriteLine(nameof(TEntity));
            Grid cellGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Star)
                }
            };
            //TextBox
            TextBox moneyTextBox = CreateNumericTextBox(textBoxProperty);
            //CombBox
            ComboBox wayOfPayCombobox = CreateComboBox(new WayOfPayingConverter(), WayOfPayProperty);
            //Adding to cellGrid
            cellGrid.Children.Add(moneyTextBox);
            Grid.SetColumn(moneyTextBox, 0);
            cellGrid.Children.Add(wayOfPayCombobox);
            Grid.SetColumn(wayOfPayCombobox, 1);

            return cellGrid;
        }, true);

        gridToAddTo.Columns.Add(new DataGridTemplateColumn
        {
            Header = headerName,
            CellTemplate = moneyTemplate,
        });
    }

    /// <summary>
    /// Create textbox that accepts only numeric input.
    /// </summary>
    /// <param name="bindingName">Property of the entity that is bound to the textbox as source.</param>
    /// <returns>Instance of textbox.</returns>
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

    /// <summary>
    /// Create checkbox.
    /// </summary>
    /// <param name="bindingName">Property of the entity that is bound to the Checkbox as source.</param>
    /// <returns>Instance of the checkbox.</returns>
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

    /// <summary>
    /// Create combobox with converter <paramref name="converter"/>.
    /// </summary>
    /// <param name="converter">Converter for possible values of thecombobox, because in the databse the values are represented as numbers.</param>
    /// <param name="bindingName">Property of the entity that is bound to the Combobox as source.</param>
    /// <returns>Insatnce of the Combobox.</returns>
    public static ComboBox CreateComboBox(ICustomValueConverter converter, string bindingName)
    {
        var comboBox = new ComboBox
        {
            ItemsSource = (converter).Keys,  // Options to display
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

    /// <summary>
    /// Create icon for the buttons.
    /// </summary>
    /// <param name="kind">Kind of the icon (name).</param>
    /// <param name="height">Height of the icon inside the control.</param>
    /// <param name="width">Width of the icon inside the control.</param>
    /// <returns>Instance of the Icon.</returns>
    public static MaterialIcon CreateIcon(MaterialIconKind kind, int height, int width)
    {
        var icon = new MaterialIcon
        {
            Kind = kind,
            Width = height,
            Height = width
        };
        return icon;
    }
}
