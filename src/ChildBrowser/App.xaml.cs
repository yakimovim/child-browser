using System.IO;
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
            base.OnStartup(e);

            var allowedHosts = File.ReadAllLines("AllowedHosts.txt");

            var browserUrl = new BrowserUrl(allowedHosts);

            MainWindow = new MainWindow(browserUrl);

            MainWindow.Show();
        }
    }
}
