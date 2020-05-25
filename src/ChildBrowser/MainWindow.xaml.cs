using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ChildBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BrowserUrl _browserUrl;

        public MainWindow()
        {
            InitializeComponent();

            var allowedHosts = File.ReadAllLines("AllowedHosts.txt");

            _browserUrl = new BrowserUrl(allowedHosts);

            DataContext = new MainWindowViewModel(browser);
        }

        private void OnAddressKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var uri = _browserUrl.GetUri(address.Text);

                if(uri != null)
                {
                    browser.Navigate(uri);
                }
                else
                {
                    status.Text = $"Address '{address.Text}' is not allowed";
                }
            }
        }

        private void OnNavigationStarting(
            object sender, 
            Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs e)
        {
            status.Text = "Loading...";

            var address = e.Uri.ToString();

            if (address == "about:blank") return;

            var uri = _browserUrl.GetUri(address);

            if(uri == null)
            {
                e.Cancel = true;
                status.Text = $"Address '{address}' is not allowed";
            }
        }

        private void OnNavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            if(e.IsSuccess)
            {
                address.Text = e.Uri.ToString();
                status.Text = "Page is loaded";
            }
            else
            {
                status.Text = "Page is failed to load";
            }
        }
    }
}
