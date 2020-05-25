using Microsoft.Toolkit.Win32.UI.Controls;
using System.Windows.Input;

namespace ChildBrowser
{
    class MainWindowViewModel
    {
        private readonly IWebView _browser;

        public MainWindowViewModel(IWebView browser)
        {
            _browser = browser;

            BackCommand = new RelayCommand(
                (arg) => { _browser.GoBack(); },
                (arg) => _browser.CanGoBack
            );

            ForwardCommand = new RelayCommand(
                (arg) => { _browser.GoForward(); },
                (arg) => _browser.CanGoForward
            );

            RefreshCommand = new RelayCommand(
                (arg) => _browser.Refresh()
            );
        }

        public ICommand BackCommand { get; }

        public ICommand ForwardCommand { get; }

        public ICommand RefreshCommand { get; }
    }
}
