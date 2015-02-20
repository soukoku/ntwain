using GalaSoft.MvvmLight.Messaging;
using ModernWPF.Controls;
using ModernWPF.Messages;
using NTwain;
using NTwain.Data;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
                _twainVM = this.DataContext as TwainVM;

                Messenger.Default.Register<RefreshCommandsMessage>(this, m => m.HandleRefreshCommands());
                Messenger.Default.Register<DialogMessage>(this, msg =>
                {
                    if (Dispatcher.CheckAccess())
                    {
                        this.HandleDialogMessageModern(msg);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.HandleDialogMessageModern(msg);
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
            _twainVM.CloseDown();
            base.OnClosed(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _twainVM.WindowHandle = new WindowInteropHelper(this).Handle;

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
                        if (ex is TargetInvocationException)
                        {
                            ex = ex.InnerException;
                        }
                        ModernMessageBox.Show(this, ex.Message, "Cannot Set", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
