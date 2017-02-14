using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ModernWpf.Messages;
using NTwain;
using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace Sample.WPF
{
    /// <summary>
    /// Wraps a data source as view model.
    /// </summary>
    class DataSourceVM : ViewModelBase
    {
        public DataSource DS { get; set; }

        public string Name { get { return DS.Name; } }
        public string Version { get { return DS.Version.Info; } }
        public string Protocol { get { return DS.ProtocolVersion.ToString(); } }

        ICollectionView _capView;
        public DataSourceVM()
        {
            Caps = new ObservableCollection<CapVM>();
            _capView = CollectionViewSource.GetDefaultView(Caps);
            _capView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            _capView.Filter = FilterCapRoutine;
        }

        private bool FilterCapRoutine(object obj)
        {
            if (!string.IsNullOrWhiteSpace(CapFilter))
            {
                var vm = obj as CapVM;
                if (vm != null)
                {
                    return vm.Name.IndexOf(CapFilter, System.StringComparison.OrdinalIgnoreCase) > -1;
                }
            }
            return true;
        }

        public void Open()
        {
            Caps.Clear();
            var rc = DS.Open();
            //rc = DGControl.Status.Get(dsId, ref stat);
            if (rc == ReturnCode.Success)
            {
                foreach (var c in DS.Capabilities.CapSupportedCaps.GetValues().Select(o => new CapVM(DS, o)))
                {
                    Caps.Add(c);
                }
            }
        }

        private string _capFilter;

        public string CapFilter
        {
            get { return _capFilter; }
            set
            {
                _capFilter = value;
                _capView.Refresh();
            }
        }

        public ObservableCollection<CapVM> Caps { get; private set; }

        private ICommand _saveCapCommand;

        public ICommand SaveCapValuesCommand
        {
            get
            {
                return _saveCapCommand ?? (_saveCapCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new ChooseFileMessage(files =>
                    {
                        StringBuilder report = new StringBuilder();
                        report.Append("Cap values for TWAIN device ").AppendLine(DS.Name);
                        report.Append("Generated on ").AppendLine(DateTime.Now.ToString("yyyy/MM/dd hh:mm tt")).AppendLine();

                        foreach (CapVM cap in _capView)
                        {
                            report.Append(cap.Name).AppendLine(":");
                            try
                            {
                                report.Append('\t').Append("Supports: ").Append(cap.Supports).AppendLine();
                                report.Append('\t').Append("Values: ");
                                foreach (var v in cap.Get())
                                {
                                    report.Append(v).Append(',');
                                }
                                report.AppendLine();
                                report.Append('\t').Append("Current: ").Append(cap.GetCurrent()).AppendLine();
                            }
                            catch (Exception ex)
                            {
                                report.Append('\t').Append("Failed: ").Append(ex.Message).AppendLine();
                            }
                            report.AppendLine();
                        }

                        File.WriteAllText(files.First(), report.ToString());

                        using (Process.Start(files.First())) { }
                    })
                    {
                        Caption = "Choose Save File",
                        Filters = "Text files|*.txt",
                        InitialFileName = DS.Name + " capability",
                        Purpose = FilePurpose.Save,
                    });
                }, () => DS.IsOpen));
            }
        }

    }
}
