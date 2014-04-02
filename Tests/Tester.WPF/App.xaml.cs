using ModernWPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
