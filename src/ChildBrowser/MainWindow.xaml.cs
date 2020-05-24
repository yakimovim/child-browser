using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            _browserUrl = new BrowserUrl(new[] {
                "https://ru.wikipedia.org"
            });
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
                    MessageBox.Show("Not allowed URL");
                }
            }
        }

        private void OnNavigationStarting(
            object sender, 
            Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs e)
        {
            var address = e.Uri.ToString();

            if (address == "about:blank") return;

            var uri = _browserUrl.GetUri(address);

            if(uri == null)
            {
                MessageBox.Show("Not allowed URL");
                e.Cancel = true;
            }
        }

        private void OnNavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            if(e.IsSuccess)
            {
                address.Text = e.Uri.ToString();
            }
        }
    }
}
