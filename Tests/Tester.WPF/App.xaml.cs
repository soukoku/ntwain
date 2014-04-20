using ModernWPF;
using System.Windows;

namespace Tester.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
            ModernTheme.ApplyTheme(ModernTheme.Theme.Light, Accent.Green);
			base.OnStartup(e);
		}
	}
}
