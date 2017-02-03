using GalaSoft.MvvmLight.Messaging;
using ModernWpf;
using ModernWpf.Controls;
using ModernWpf.Messages;
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

namespace Sample.WPF
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
                _twainVM.PropertyChanged += _twainVM_PropertyChanged;
                Messenger.Default.Register<RefreshCommandsMessage>(this, m => m.HandleIt());
                Messenger.Default.Register<ChooseFileMessage>(this, m =>
                {
                        m.HandleWithPlatform(this);
                });
                Messenger.Default.Register<MessageBoxMessage>(this, msg =>
                {
                    if (Dispatcher.CheckAccess())
                    {
                        msg.HandleWithModern(this);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            msg.HandleWithModern(this);
                        }));
                    }
                });
            }
        }

        private void _twainVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                if (_twainVM.State == 5)
                {
                    Theme.ApplyTheme(ThemeColor.Light, Accent.Orange);
                }
                else if (_twainVM.State == 4)
                {
                    Theme.ApplyTheme(ThemeColor.Light, Accent.Green);
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = _twainVM.State > 4;
            base.OnClosing(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            Messenger.Default.Unregister(this);
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
