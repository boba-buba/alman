using Alman.SharedModels;
using AlmanUI.Controls;
using AlmanUI.Models;
using AlmanUI;
using Avalonia.Data.Converters;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace AlmanUI.Views
{

    public partial class PrecontractsPageView : UserControl
    {
        private IReadOnlyList<IChildBase>? _childrenTable;

        private IReadOnlyList<IPrecontractBase>? _precontractsTable;

        private IReadOnlyList<PrecontractCompositeItem>? _childPrecontracts { get; set; }

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


            var childPrecontracts = new List<PrecontractCompositeItem>();

            foreach (var child in _childrenTable)
            {
                var newItem = new PrecontractCompositeItem { PChild = child };
                IPrecontractBase? newItemPrecontract = _precontractsTable.SingleOrDefault(pr => pr.PchildId == child.ChildId);

                if (_precontractsTable.Count == 0 || newItemPrecontract == null)
                {
                    newItemPrecontract = new PrecontractUI { PchildId = child.ChildId, PMonth = child.ChildStartMonth, PYear = child.ChildStartYear };

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
                _childPrecontracts = new List<PrecontractCompositeItem>();
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
            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Child Name", Binding = new Binding("PChild.ChildName"), IsReadOnly = true });
            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Child Lastname", Binding = new Binding("PChild.ChildLastName"), IsReadOnly = true });

            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Paid Sum", Binding = new Binding("Precontract.Psum") {
                Converter = new IntToStringConverter(), // Apply the converter here
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            }});
            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Comment", Binding = new Binding("Precontract.Pcomment") });

            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Month", Binding = new Binding("Precontract.PMonth"), IsReadOnly = true });
            PrecontractsMainDataGrid.Columns.Add(new DataGridTextColumn { Header = "Year", Binding = new Binding("Precontract.PYear"), IsReadOnly = true});

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


    
}