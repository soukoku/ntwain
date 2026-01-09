using NTwain;
using NTwain.Data;
using NTwain.Events;
using System.Diagnostics;

namespace ConsoleSample;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"[{(Environment.Is64BitProcess ? "64bit" : "32bit")}] Creating TwainAppSession instance...");

        bool sourceDisabledEventFired = false;

        using var twain = new NTwain.TwainAppSession();
        twain.SourceDisabled += (s, e) =>
        {
            sourceDisabledEventFired = true;
            Console.WriteLine($"Source disabled: {e.ProductName}");
        };
        twain.Transferred += OnTransferred;

        Console.WriteLine("Opening DSM...");
        var sts = twain.OpenDsm();
        Console.WriteLine($"OpenDsm returned: {sts}");

        if (sts.IsSuccess)
        {
            while (true)
            {
                var sources = twain.GetSources();
                Console.WriteLine($"Found {sources.Count} sources.");
                Console.WriteLine(" Source 0: [Choose by old dialog]");
                for (var i = 0; i < sources.Count; i++)
                {
                    var src = sources[i];
                    Console.WriteLine($" Source {i + 1}: {src.ProductName}");
                }

                Console.WriteLine("Choose a source index to open (or just press Enter to skip): ");
                var input = Console.ReadLine();
                TWIdentityWrapper? selectedSource = null;
                var validInput = int.TryParse(input, out int index);
                if (validInput && index > 0 && index <= sources.Count)
                {
                    selectedSource = sources[index - 1];
                }
                else if (validInput && index == 0)
                {
                    Console.WriteLine("Showing user select dialog...");
                    sts = twain.ShowUserSelect();
                    Console.WriteLine($"OpenSource returned: {sts}");

                    if (sts.IsSuccess)
                    {
                        selectedSource = twain.DefaultSource;
                    }
                }

                if (selectedSource == null)
                {
                    break;
                }


                Console.WriteLine($"Opening source: {selectedSource.ProductName}");
                sts = twain.OpenSource(selectedSource);
                Console.WriteLine($"OpenSource returned: {sts}");

                if (sts.IsSuccess)
                {
                    sourceDisabledEventFired = false;
                    xferCount = 0;
                    watch.Restart();
                    if (twain.EnableSource(NTwain.SourceEnableOption.ShowUI).IsSuccess)
                    {
                        while(!sourceDisabledEventFired)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to enable source.");
                    }


                    Console.WriteLine($"Closing source: {selectedSource.ProductName}");
                    sts = twain.CloseSource();
                    Console.WriteLine($"CloseSource returned: {sts}");
                }
            }

            Thread.Sleep(1000);

            Console.WriteLine("Closing DSM...");
            sts = twain.CloseDsm();
            Console.WriteLine($"CloseDsm returned: {sts}");
        }


        Console.WriteLine("Press Enter to quit...");
        Console.ReadLine();
    }

    static int xferCount = 0;
    static Stopwatch watch = new Stopwatch();
    private static void OnTransferred(TwainAppSession sender, TransferredEventArgs e)
    {
        if (e.Data != null)
        {
            var saveFile = Path.Combine(Environment.CurrentDirectory, $"twain_{DateTime.Now:yyyyMMdd_HHmmss}_{xferCount}");
            
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
                Console.WriteLine($"SUCCESS! Got twain memory data #{++xferCount} on thread {Environment.CurrentManagedThreadId}, saving to {saveFile}.");
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
}
