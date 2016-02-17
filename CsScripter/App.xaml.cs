using System.Windows;
using CsScripterLib;
using Microsoft.Practices.Unity;

namespace CsScripter
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			var unity = BootStrapper.UnityContainer;
			unity.RegisterType<MainWindow, MainWindow>();

			var mainWindow = unity.Resolve<MainWindow>();
			mainWindow.Show();
		}
	}
}
