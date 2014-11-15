using GalaSoft.MvvmLight;
using NTwain;
using NTwain.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Tester.WPF
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
                foreach (var c in DS.SupportedCaps.Select(o => new CapVM(DS, o)))
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
    }
}
