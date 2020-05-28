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

            _viewModel = new MainWindowViewModel(browserUrl);

            DataContext = _viewModel;
        }

        private void OnAddressKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var uri = _browserUrl.GetUri(address.Text);

                if(uri != null)
                {
                    _viewModel.SelectedBrowser.Browser.Navigate(uri);
                }
                else
                {
                    _viewModel.SelectedBrowser.Status = $"Address '{address.Text}' is not allowed";
                }
            }
        }
    }
}
