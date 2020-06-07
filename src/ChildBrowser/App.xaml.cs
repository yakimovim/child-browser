using System.IO;
using System.Threading;
using System.Windows;

namespace ChildBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");

            base.OnStartup(e);

            var allowedHosts = File.ReadAllLines("AllowedHosts.txt");

            var browserUrl = new BrowserUrl(allowedHosts);

            MainWindow = new MainWindow(browserUrl);

            MainWindow.Show();
        }
    }
}
