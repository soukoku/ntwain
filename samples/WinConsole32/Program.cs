using NTwain;
using NTwain.Data;
using NTwain.Data.Kds;
using System.Diagnostics;

namespace WinConsole32
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var libVer = FileVersionInfo.GetVersionInfo(typeof(TwainAppSession).Assembly.Location).ProductVersion;
            Console.WriteLine($"Console sample {(TWPlatform.Is32bit ? " 32bit" : " 64bit")} on NTwain {libVer}");

            TwainAppSession twain = new TwainAppSession();

            twain.StateChanged += Session_StateChanged;
            twain.SourceDisabled += Session_SourceDisabled;
            twain.TransferReady += Session_TransferReady;
            twain.Transferred += Session_Transferred;

            var sts = await twain.OpenDSMAsync();

            if (sts.IsSuccess)
            {
                Console.WriteLine("Available data sources:");

                TW_IDENTITY_LEGACY firstSrc = default;
                foreach (var src in twain.GetSources())
                {
                    if (!firstSrc.HasValue) firstSrc = src;
                    Console.WriteLine($"\t{src}");
                }
                Console.WriteLine();

                var defaultSrc = twain.DefaultSource;
                Console.WriteLine($"Default data source = {defaultSrc}");
                Console.WriteLine();

                sts = twain.ShowUserSelect();
                if (sts.IsSuccess)
                {
                    Console.WriteLine($"Selected data source = {twain.DefaultSource}");
                    Console.WriteLine();

                    var targetSrc = defaultSrc.HasValue ? defaultSrc : firstSrc;

                    if (targetSrc.HasValue)
                    {
                        TestThisSource(twain, targetSrc);
                    }
                    else
                    {
                        Console.WriteLine("No data source to test.");
                        Console.WriteLine();
                    }

                    Console.WriteLine("---------------------------------------------");
                    Console.WriteLine("Test in progress, press Enter to stop testing");
                    Console.WriteLine("---------------------------------------------");
                    Console.ReadLine();
                    twain.TryStepdown(STATE.S1);
                }
            }
            else
            {
                Console.WriteLine("Failed to attach: " + sts);
            }

            Console.WriteLine("-------------------");
            Console.WriteLine("Press Enter to exit");
            Console.WriteLine("-------------------");
            Console.ReadLine();
        }

        private static void Session_TransferReady(TwainAppSession twain, TransferReadyEventArgs e)
        {
            if (e.ImgXferMech == TWSX.FILE)
            {
                var req = TW_EXTIMAGEINFO.CreateRequest(TWEI.CAMERA, (TWEI)KDS_TWEI.HDR_PAGENUMBER, (TWEI)KDS_TWEI.HDR_COMPRESSION);
                e.GetExtendedImageInfo(ref req);

                string? camera = null;
                TWCP comp = TWCP.NONE;
                int pageNum = 0;

                foreach (var ei in req.AsInfos())
                {
                    switch (ei.InfoId)
                    {
                        case TWEI.CAMERA:
                            camera = ei.ReadHandleString(twain);
                            break;
                        case (TWEI)KDS_TWEI.HDR_PAGENUMBER:
                            pageNum = ei.ReadNonPointerData<int>();
                            break;
                        case (TWEI)KDS_TWEI.HDR_COMPRESSION:
                            comp = ei.ReadNonPointerData<TWCP>();
                            break;
                    }
                }

                string? targetName = null;
                TWFF format = TWFF.TIFF;
                switch (comp)
                {
                    case TWCP.JPEG:
                        targetName = $"twain_{DateTime.Now:yyyyMMdd_HHmmss}_{xferCount:D4}.jpg";
                        format = TWFF.JFIF;
                        break;
                    case TWCP.NONE:
                        targetName = $"twain_{DateTime.Now:yyyyMMdd_HHmmss}_{xferCount:D4}.bmp";
                        format = TWFF.BMP;
                        break;
                    default:
                        targetName = $"twain_{DateTime.Now:yyyyMMdd_HHmmss}_{xferCount:D4}.tif";
                        break;
                }

                TW_SETUPFILEXFER setup = new()
                {
                    FileName = Path.Combine("Images", targetName),
                    Format = format,
                };
                e.SetupFileTransfer(ref setup);
            }
        }

        static int xferCount = 0;
        static Stopwatch watch = new Stopwatch();
        private static void Session_Transferred(TwainAppSession twain, TransferredEventArgs e)
        {
            if (e.Data != null)
            {
                var saveFile = $"twain_{DateTime.Now:yyyyMMdd_HHmmss}_{xferCount}";
                Console.WriteLine($"SUCCESS! Got twain memory data #{++xferCount} on thread {Environment.CurrentManagedThreadId}, saving to {saveFile}.");

                using (var img = new ImageMagick.MagickImage(e.Data.AsStream()))
                {
                    var format = ImageMagick.MagickFormat.Png;
                    if (img.ColorType == ImageMagick.ColorType.Palette)
                    {
                        // bw or gray
                        saveFile += ".png";
                    }
                    else
                    {
                        // color
                        saveFile += ".jpg";
                        format = ImageMagick.MagickFormat.Jpeg;
                        img.Settings.Compression = ImageMagick.CompressionMethod.JPEG;
                        img.Quality = (uint)85;
                    }
                    img.Write(saveFile, format);
                }
            }
            else if (e.FileInfo != null)
            {
                var fi = e.FileInfo.Value;
                Console.WriteLine($"SUCCESS! Got twain file data #{++xferCount} on thread {Environment.CurrentManagedThreadId} as {fi.FileName}.");
            }
            else
            {
                Console.WriteLine($"BUMMER! No twain data #{++xferCount} on thread {Environment.CurrentManagedThreadId}.");
            }
            e.Dispose();
        }

        private static void Session_StateChanged(TwainAppSession twain, STATE e)
        {
            Console.WriteLine($"Session state changed to {twain.State}");
        }

        private static void Session_SourceDisabled(TwainAppSession twain, TW_IDENTITY_LEGACY e)
        {
            watch.Stop();
            var elapsed = watch.Elapsed;
            Console.WriteLine($"Session source disabled, took {elapsed}, will retest in 3 sec...");

            Thread.Sleep(3000);
            if (twain.State > STATE.S3)
                TestThisSource(twain, e);
        }

        private static void TestThisSource(TwainAppSession twain, TW_IDENTITY_LEGACY source)
        {
            Console.WriteLine($"Testing data source {source}");
            Console.WriteLine();

            if (twain.State == STATE.S3) twain.OpenSource(source);

            if (source.ProductName.ToString().StartsWith("KODAK Scanner:"))
            {
                TryKodakSDMI(twain);
            }
            else
            {
                twain.Caps.ICAP_XFERMECH.Set(TWSX.NATIVE);
                twain.Caps.ICAP_PIXELTYPE.Set(TWPT.RGB);
                twain.Caps.ICAP_XRESOLUTION.Set(300f);
                twain.Caps.ICAP_YRESOLUTION.Set(300f);
                twain.Caps.CAP_XFERCOUNT.Set(4);

                xferCount = 0;
                watch.Restart();
                var rc = twain.EnableSource(true, false);
            }
        }

        private static void TryKodakSDMI(TwainAppSession twain)
        {
            twain.Caps.ICAP_XFERMECH.Set(TWSX.FILE);

            twain.Caps.ICAP_EXTIMAGEINFO.Set(TW_BOOL.True);
            //twain.Caps.ICAP_IMAGEFILEFORMAT.Set(TWFF.TIFF);

            var orders = twain.Caps.CAP_CAMERAORDER.GetCurrent().Cast<TWPT>().ToList();


            if (twain.ChangeFileSystemDirectory(TWFY.CAMERA, "/Camera_Bitonal_Both").IsSuccess)
            {
                twain.Caps.CAP_CAMERAENABLED.Set(TW_BOOL.True);
                twain.Caps.ICAP_XRESOLUTION.Set(300f);
                twain.Caps.ICAP_YRESOLUTION.Set(300f);
                twain.Caps.ICAP_COMPRESSION.Set(TWCP.GROUP4);
            }

            if (twain.ChangeFileSystemDirectory(TWFY.CAMERA, "/Camera_Color_Both").IsSuccess)
            {
                twain.Caps.CAP_CAMERAENABLED.Set(TW_BOOL.True);
                twain.Caps.ICAP_XRESOLUTION.Set(300f);
                twain.Caps.ICAP_YRESOLUTION.Set(300f);
                twain.Caps.ICAP_COMPRESSION.Set(TWCP.JPEG);
                twain.Caps.ICAP_JPEGQUALITY.Set(TWJQ.HIGH);
            }
            twain.Caps.CAP_XFERCOUNT.Set(4);

            xferCount = 0;
            watch = Stopwatch.StartNew();
            var rc = twain.EnableSource(false, false);
        }
    }
}