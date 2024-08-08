using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace AlmanUI.Views
{

    public partial class PrecontractsPageView : UserControl
    {
        private IReadOnlyList<IChildBase>? _childrenTable;

        private IReadOnlyList<IPrecontractBase>? _precontractsTable;

        private IReadOnlyList<CompositeItemPrecontract>? _childPrecontracts { get; set; }

        private void LoadItems(int year, int month)
        {
            
            _childrenTable = ChildrenControl.GetChildrenByFilter(ch =>
                ch.ChildStartYear == year &&
                ch.ChildStartMonth == month);

            if (_childrenTable is null)
            {
                return;
            }

            _precontractsTable = PrecontractsControl.GetPrecontractsByFilter(pr =>
                pr.PYear == year &&
                pr.PMonth == month);


            var childPrecontracts = new List<CompositeItemPrecontract>();

            foreach (var child in _childrenTable)
            {
                var newItem = new CompositeItemPrecontract { PChild = child };
                IPrecontractBase newItemPrecontract;

                if (_precontractsTable.Count == 0 || _precontractsTable.Single(pr => pr.PchildId == child.ChildId) == null)
                {
                    newItemPrecontract = new PrecontractUI { PchildId = child.ChildId, PMonth = child.ChildStartMonth, PYear = child.ChildStartYear };

                }
                else
                {
                    newItemPrecontract = _precontractsTable.Single(pr => pr.PchildId == child.ChildId);
                }
                newItem.Precontract = newItemPrecontract;
                childPrecontracts.Add(newItem);
            }

            _childPrecontracts = childPrecontracts;

        }

        public PrecontractsPageView()
        {
            LoadItems(DateTime.Now.Year, DateTime.Now.Month);
            if (_childPrecontracts  is null)
            {
                _childPrecontracts = new List<CompositeItemPrecontract>();
            }
            InitializeComponent();
            InitPrecontractsMainDataGrid();
            Mediator.Mediator.Instance.NotifyWithParams += OnNotifyWithParams;

        }

        private void OnNotifyWithParams(string message, int year, int month)
        {
            if (message == "UpdatePrecontractsMainDataGrid")
            {
                UpdatePrecontractsMainDataGrid(year, month);
            }
        }

        private void InitPrecontractsMainDataGrid()
        {
            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Child Name", Binding = new Binding("PChild.ChildName") });
            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Child Lastname", Binding = new Binding("PChild.ChildLastName") });

            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Paid Sum", Binding = new Binding("Precontract.Psum") });
            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Comment", Binding = new Binding("Precontract.Pcomment") });

            var monthColumn = new FuncDataTemplate<CompositeItemPrecontract>((x, _) =>
            {
                var monthUpDown = new NumericUpDown
                {
                    Minimum = 1,
                    Maximum = 12,
                    Increment = 1,
                    [!NumericUpDown.ValueProperty] = new Binding("Precontract.PMonth")
                };

                return monthUpDown;
            });

            var yearColumn = new FuncDataTemplate<CompositeItemPrecontract>((x, _) =>
            {
                var yearUpDown = new NumericUpDown
                {
                    Minimum = 2000,
                    Maximum = 2100,
                    Increment = 1,
                    [!NumericUpDown.ValueProperty] = new Binding("Precontract.PYear")
                };
                return yearUpDown;
            });



            PrecontractsMainDataGrid.Columns.Add(new DataGridTemplateColumn { Header = "Month", Width = DataGridLength.Auto, CellTemplate = monthColumn });
            PrecontractsMainDataGrid.Columns.Add(new DataGridTemplateColumn { Header = "Year", Width = DataGridLength.Auto, CellTemplate = yearColumn });

            PrecontractsMainDataGrid.ItemsSource = _childPrecontracts;
            SavePrecontractsButton.CommandParameter = _childPrecontracts;


        }

        private void UpdatePrecontractsMainDataGrid(int year, int month)
        {
            LoadItems(year, month);

            PrecontractsMainDataGrid.Columns.Clear();
            InitPrecontractsMainDataGrid();

        }

    }


    public class CompositeItemPrecontract
    {
        public IChildBase? PChild { get; set; }
        public IPrecontractBase? Precontract { get; set; }
    }
}