using CommonWin32;
using GalaSoft.MvvmLight.Messaging;
using NTwain;
using NTwain.Data;
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tester.WPF
{
    /// <summary>
    /// Wraps the twain session as a view model for databinding.
    /// </summary>
    class TwainVM : TwainSession
    {
        public TwainVM()
            : base(TWIdentity.CreateFromAssembly(DataGroups.Image | DataGroups.Audio, Assembly.GetEntryAssembly()))
        {
            //this.SynchronizationContext = SynchronizationContext.Current;
        }

        private ImageSource _image;

        /// <summary>
        /// Gets or sets the captured image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public ImageSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        protected override void OnTransferError(TransferErrorEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.Exception != null)
                {
                    Messenger.Default.Send(new DialogMessage(e.Exception.Message, null)
                    {
                        Caption = "Transfer Error Exception",
                        Icon = System.Windows.MessageBoxImage.Error,
                        Button = System.Windows.MessageBoxButton.OK
                    });
                }
                else
                {
                    Messenger.Default.Send(new DialogMessage(string.Format("Return Code: {0}\nCondition Code: {1}", e.ReturnCode, e.SourceStatus.ConditionCode), null)
                    {
                        Caption = "Transfer Error",
                        Icon = System.Windows.MessageBoxImage.Error,
                        Button = System.Windows.MessageBoxButton.OK
                    });
                }
            }));
        }

        protected override void OnTransferReady(TransferReadyEventArgs e)
        {
            // set it up to use file xfer

            if (this.CurrentSource.CapGetCurrent(CapabilityId.ICapXferMech).ConvertToEnum<XferMech>() == XferMech.File)
            {
                var formats = this.CurrentSource.CapGetImageFileFormat();
                var wantFormat = formats.Contains(FileFormat.Tiff) ? FileFormat.Tiff : FileFormat.Bmp;

                var fileSetup = new TWSetupFileXfer
                {
                    Format = wantFormat,
                    FileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.tif")
                };
                var rc = this.DGControl.SetupFileXfer.Set(fileSetup);
            }
        }

        protected override void OnDataTransferred(DataTransferredEventArgs e)
        {
            ImageSource img = null;
            if (e.NativeData != IntPtr.Zero)
            {
                img = e.NativeData.GetWPFBitmap();
            }
            else if (!string.IsNullOrEmpty(e.FileDataPath))
            {
                img = new BitmapImage(new Uri(e.FileDataPath));
            }
            if (img != null)
            {
                if (img.CanFreeze)
                {
                    img.Freeze();
                }
                App.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Image = img;
                }));
            }
        }

        public void TestCapture(IntPtr hwnd)
        {
            if (State == 4)
            {
                if (this.CurrentSource.CapGetPixelTypes().Contains(PixelType.BlackWhite))
                {
                    this.CurrentSource.CapSetPixelType(PixelType.BlackWhite);
                }

                if (this.CurrentSource.CapGetImageXferMechs().Contains(XferMech.File))
                {
                    this.CurrentSource.CapSetImageXferMech(XferMech.File);
                }

                var rc = this.CurrentSource.StartTransfer(SourceEnableMode.NoUI, false, hwnd);
            }
        }
    }
}
