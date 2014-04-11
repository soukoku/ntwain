using NTwain;
using NTwain.Data;
using NTwain.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommonWin32;
using System.Threading;

namespace Tester.WPF
{
    /// <summary>
    /// Wraps the twain session as a view model for databinding.
    /// </summary>
    class TwainVM : TwainSessionWPF
    {
        public TwainVM()
            : base(TWIdentity.CreateFromAssembly(DataGroups.Image | DataGroups.Audio, Assembly.GetEntryAssembly()))
        {

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
                RaisePropertyChanged("Image");
            }
        }

        protected override void OnTransferReady(TransferReadyEventArgs e)
        {
            // set it up to use file xfer

            if (this.GetCurrentCap(CapabilityId.ICapXferMech).ConvertToEnum<XferMech>() == XferMech.File)
            {
                var formats = this.CapGetImageFileFormat();
                var wantFormat = formats.Contains(FileFormat.Tiff) ? FileFormat.Tiff : FileFormat.Bmp;

                var fileSetup = new TWSetupFileXfer
                {
                    Format = wantFormat,
                    FileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.tif")
                };
                var rc = this.DGControl.SetupFileXfer.Set(fileSetup);
            }
            base.OnTransferReady(e);
        }

        protected override void OnDataTransferred(DataTransferredEventArgs e)
        {
            if (e.NativeData != IntPtr.Zero)
            {
                Image = e.NativeData.GetWPFBitmap();
            }
            else if (!string.IsNullOrEmpty(e.FileDataPath))
            {
                var img = new BitmapImage(new Uri(e.FileDataPath));
                Image = img;
            }
            base.OnDataTransferred(e);
        }

        public void TestCapture(IntPtr hwnd)
        {
            if (State == 4)
            {
                if (this.CapGetPixelTypes().Contains(PixelType.BlackWhite))
                {
                    this.CapSetPixelType(PixelType.BlackWhite);
                }

                if (this.CapGetImageXferMechs().Contains(XferMech.File))
                {
                    this.CapSetImageXferMech(XferMech.File);
                }

                var rc = EnableSource(SourceEnableMode.NoUI, false, hwnd, SynchronizationContext.Current);
            }
        }
    }
}
