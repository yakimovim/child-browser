using System;
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
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(BrowserUrl browserUrl)
        {
            _browserUrl = browserUrl ?? throw new ArgumentNullException(nameof(browserUrl));

            InitializeComponent();

            _viewModel = new MainWindowViewModel(browser, browserUrl);

            DataContext = _viewModel;
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
                    _viewModel.Status = $"Address '{address.Text}' is not allowed";
                }
            }
        }

        private void OnNavigationStarting(
            object sender, 
            Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs e)
        {
            _viewModel.Status = "Loading...";

            var address = e.Uri.ToString();

            if (address == "about:blank") return;

            var uri = _browserUrl.GetUri(address);

            if(uri == null)
            {
                e.Cancel = true;
                _viewModel.Status = $"Address '{address}' is not allowed";
            }

            CommandManager.InvalidateRequerySuggested();
        }

        private void OnNavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            if(e.IsSuccess)
            {
                address.Text = e.Uri.ToString();
                _viewModel.Status = "Page is loaded";
            }
            else
            {
                _viewModel.Status = "Page is failed to load";
            }

            CommandManager.InvalidateRequerySuggested();
        }
    }
}
