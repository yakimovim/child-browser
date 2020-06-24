using ChildBrowser.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace ChildBrowser.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        private bool _autoCompleteInProgress;

        public MainWindow(BrowserUrl browserUrl)
        {
            if (browserUrl is null)
            {
                throw new ArgumentNullException(nameof(browserUrl));
            }

            InitializeComponent();

            _viewModel = new MainWindowViewModel(browserUrl);

            DataContext = _viewModel;
        }

        private void OnAddressKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                _viewModel.SelectedBrowser.GoToAddress(address.Text);
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
