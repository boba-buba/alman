using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace AlmanUI.Views;

public partial class StaffPageView : UserControl
{
    public StaffPageView()
    {
        InitializeComponent();
        InitStaffMainDataGrid();
    }

    private void InitStaffMainDataGrid()
    {

        StaffMembersMainDataGrid.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);

        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "First Name", Binding = new Binding("FirstName") });
        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new Binding("LastName") });

        StaffMembersMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "IsActive",  // Example header name
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var checkBox = new CheckBox();

                var binding = new Binding("State")
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new IntToBoolConverter(),  // Apply the converter here
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // Optional: Immediately update the source
                };

                checkBox.Bind(CheckBox.IsCheckedProperty, binding);

                return checkBox;
            }),
        });
        StaffMembersMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "Start Month",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var numericUpDown = new NumericUpDown
                {
                    Minimum = 1,   // Set the minimum value
                    Maximum = 12,  // Set the maximum value
                    Increment = 1,   // Set the increment value (step size)
                };

                var binding = new Binding("StartMonth")
                {
                    Mode = BindingMode.TwoWay, // Ensure two-way binding to update source and UI
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // Update source immediately when the value changes
                };

                numericUpDown.Bind(NumericUpDown.ValueProperty, binding);

                return numericUpDown;
            }),
        });

        StaffMembersMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "Start Year",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var numericUpDown = new NumericUpDown
                {
                    Minimum = 2000,   // Set the minimum value
                    Maximum = 2100,  // Set the maximum value
                    Increment = 1,   // Set the increment value (step size)
                };

                var binding = new Binding("StartYear")
                {
                    Mode = BindingMode.TwoWay, // Ensure two-way binding to update source and UI
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged // Update source immediately when the value changes
                };

                numericUpDown.Bind(NumericUpDown.ValueProperty, binding);

                return numericUpDown;
            }),
        });

        StaffMembersMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Position Name", Binding = new Binding("PositionName") });
        StaffMembersMainDataGrid.Columns.Add(new DataGridTemplateColumn
        {
            Header = "Position Salary",
            CellTemplate = new FuncDataTemplate<object>((item, namescope) =>
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding("PositionSalary", BindingMode.TwoWay)
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new IntToStringConverter(), // Apply the converter here
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                textBox.KeyDown += UIUtilities.TextBox_NumericInput_KeyDown;  // Attach the filtering function
                return textBox;
            }),
        });

    }
}