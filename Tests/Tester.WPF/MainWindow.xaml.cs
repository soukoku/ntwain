using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NTwain;
using NTwain.Data;
using NTwain.Values;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using CommonWin32;
using System.Threading;
using ModernWPF.Controls;
using System.Reflection;

namespace Tester.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TwainSessionWPF twain;
        public MainWindow()
        {
            InitializeComponent();
            if (IntPtr.Size == 8)
            {
                Title = Title + " (64bit)";
            }
            else
            {
                Title = Title + " (32bit)";
            }

            SetupTwain();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd).AddHook(twain.PreFilterMessage);
        }

        private void SetupTwain()
        {
            TWIdentity appId = TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetEntryAssembly());
            twain = new TwainSessionWPF(appId);
            twain.DataTransferred += (s, e) =>
            {
                if (e.Data != IntPtr.Zero)
                {
                    ImageDisplay.Source = e.Data.GetWPFBitmap();
                }
                else if (!string.IsNullOrEmpty(e.FilePath))
                {
                    var img = new BitmapImage(new Uri(e.FilePath));
                    ImageDisplay.Source = img;
                }
            };

            twain.SourceDisabled += delegate
            {
                //step = "Close DS";
                var rc2 = twain.CloseSource();
                rc2 = twain.CloseManager();

                ModernMessageBox.Show(this, "Success!");
            };
            twain.TransferReady += (s, te) =>
            {
                //var type = twain.GetCurrentCap<PixelType>(CapabilityId.ICapPixelType);
                //if (type == PixelType.BlackWhite)
                //{
                //    te.ImageFormat = ImageFileFormat.Tiff;
                //    te.ImageCompression = Compression.Group31D;
                //    te.OutputFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.tif");
                //}
                //else
                //{
                //te.OutputFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.bmp");
                //}
            };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TWIdentity dsId;
            TWStatus status = null;

            string step = "Open DSM";

            var hand = new WindowInteropHelper(Application.Current.MainWindow).Handle;

            var rc = twain.OpenManager(hand);
            if (rc == ReturnCode.Success)
            {
                step = "User Select";
                rc = twain.DGControl.Identity.UserSelect(out dsId);
                //rc = DGControl.Status.Get(ref stat);

                //TwEntryPoint entry;
                //rc = DGControl.EntryPoint.Get(dsId, out entry);
                if (rc == ReturnCode.Success)
                {
                    step = "Open DS";
                    rc = twain.OpenSource(dsId.ProductName);
                    //rc = DGControl.Status.Get(dsId, ref stat);
                    if (rc == ReturnCode.Success)
                    {
                        var caps = twain.SupportedCaps.Select(o =>
                        {
                            if (o > CapabilityId.CustomBase)
                            {
                                return "[Custom] " + ((int)o - (int)CapabilityId.CustomBase);
                            }
                            return o.ToString();
                        }).OrderBy(o => o).ToList();
                        CapList.ItemsSource = caps;

                        if (twain.CapGetPixelTypes().Contains(PixelType.BlackWhite))
                        {
                            twain.CapSetPixelType(PixelType.BlackWhite);
                        }

                        step = "Enable DS";
                        rc = twain.EnableSource(SourceEnableMode.NoUI, false, hand, SynchronizationContext.Current);
                        return;
                    }
                    else
                    {
                        twain.DGControl.Status.GetSource(out status);
                    }
                }
                else
                {
                    twain.DGControl.Status.GetManager(out status);
                }
                twain.CloseManager();
            }
            else
            {
                twain.DGControl.Status.GetManager(out status);
            }

            ModernMessageBox.Show(this, string.Format("Step {0}: RC={1}, CC={2}", step, rc, status.ConditionCode));
        }
    }
}
