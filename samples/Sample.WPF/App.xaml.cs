using ModernWpf;
using System.Windows;

namespace Sample.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            UIHooks.EnableHighDpiSupport();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Theme.ApplyTheme(ThemeColor.Light, Accent.Green);
            base.OnStartup(e);
        }
    }
}
