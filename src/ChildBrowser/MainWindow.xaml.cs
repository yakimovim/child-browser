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

        private bool _autoCompleteInProgress;

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

        private void OnAddressChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!address.IsFocused) return;
            if (_autoCompleteInProgress) return;

            try
            {
                _autoCompleteInProgress = true;

                var selectionStart = address.SelectionStart;

                var notSelectedPartOfAddress = address.Text.Substring(0, selectionStart);

                var completion = _viewModel.GetAddressCompletion(notSelectedPartOfAddress);
                if (string.IsNullOrEmpty(completion)) return;

                var autoCompletedText = notSelectedPartOfAddress + completion;

                address.Text = autoCompletedText;
                address.SelectionStart = selectionStart;
                address.SelectionLength = completion.Length;
            }
            finally
            {
                _autoCompleteInProgress = false;
            }
        }
    }
}
