using GalaSoft.MvvmLight.Messaging;
using ModernWPF.Controls;
using NTwain;
using NTwain.Data;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Tester.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TwainVM _twainVM;

        public MainWindow()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (PlatformInfo.Current.IsApp64Bit)
                {
                    Title = Title + " (64bit)";
                }
                else
                {
                    Title = Title + " (32bit)";
                }

                _twainVM = new TwainVM();
                this.DataContext = _twainVM;

                Messenger.Default.Register<DialogMessage>(this, msg =>
                {
                    if (Dispatcher.CheckAccess())
                    {
                        ModernMessageBox.Show(this, msg.Content, msg.Caption, msg.Button, msg.Icon, msg.DefaultResult);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ModernMessageBox.Show(this, msg.Content, msg.Caption, msg.Button, msg.Icon, msg.DefaultResult);
                        }));
                    }
                });
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = _twainVM.State > 4;
            base.OnClosing(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            if (_twainVM.State == 4)
            {
                _twainVM.CurrentSource.Close();
            }
            _twainVM.Close();
            base.OnClosed(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // use this for internal msg loop
            //var rc = _twainVM.Open();

            // use this to hook into current app loop
            var rc = _twainVM.Open(new WpfMessageLoopHook(new WindowInteropHelper(this).Handle));

            if (rc == ReturnCode.Success)
            {
                SrcList.ItemsSource = _twainVM.Select(s => new DSVM { DS = s });
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _twainVM.TestCapture(new WindowInteropHelper(this).Handle);
        }

        private void SrcList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_twainVM.State == 4)
            {
                _twainVM.CurrentSource.Close();
            }

            var dsId = SrcList.SelectedItem as DSVM;
            if (dsId != null)
            {
                var rc = dsId.DS.Open();
                //rc = DGControl.Status.Get(dsId, ref stat);
                if (rc == ReturnCode.Success)
                {
                    var caps = dsId.DS.SupportedCaps.Select(o => new CapVM(dsId, o)).OrderBy(o => o.Name).ToList();
                    CapList.ItemsSource = caps;
                }
            }
            else
            {
                CapList.ItemsSource = null;
            }
        }


        private void CapList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var capVM = CapList.SelectedItem as CapVM;
            if (capVM != null)
            {
                CapDetailList.ItemsSource = capVM.Get();
                CapDetailList.SelectedItem = capVM.GetCurrent();
            }
            else
            {
                CapDetailList.ItemsSource = null;
            }
        }

        private void CapDetailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var capVM = CapList.SelectedItem as CapVM;
            if (capVM != null)
            {
                if (capVM.Supports.HasFlag(QuerySupports.Set))
                {
                    try
                    {
                        capVM.Set(CapDetailList.SelectedItem);
                    }
                    catch (Exception ex)
                    {
                        ModernMessageBox.Show(this, ex.Message, "Cannot Set", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
