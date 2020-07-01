using ChildBrowser.Resources;
using Microsoft.Toolkit.Win32.UI.Controls;
using Microsoft.Toolkit.Wpf.UI.Controls;
using System;
using System.Windows.Input;

namespace ChildBrowser.ViewModels
{
    class BrowserViewModel : ViewModel
    {
        private string _status = string.Empty;
        private string _address;
        private string _title = UITexts.AbsentPageTitle;
        private readonly BrowserUrl _browserUrl;

        public BrowserViewModel(BrowserUrl browserUrl)
        {
            _browserUrl = browserUrl ?? throw new ArgumentNullException(nameof(browserUrl));

            Browser = new WebView();
            Browser.NavigationStarting += OnNavigationStarting;
            Browser.NavigationCompleted += OnNavigationCompleted;

            BackCommand = new RelayCommand(
                (arg) => { Browser.GoBack(); },
                (arg) => Browser.CanGoBack
            );

            ForwardCommand = new RelayCommand(
                (arg) => { Browser.GoForward(); },
                (arg) => Browser.CanGoForward
            );

            RefreshCommand = new RelayCommand(
                (arg) => Browser.Refresh()
            );

            GoToBookmarkCommand = new RelayCommand(arg => {
                var address = (string)arg;

                GoToAddress(address);
            });

            CloseCommand = new RelayCommand(arg => {
                Closing(this, this);
            });
        }

        public event EventHandler<BrowserViewModel> Closing;

        public IWebView Browser { get; }

        public string Status
        {
            get => _status;
            set
            {
                value = value ?? string.Empty;

                if(_status != value)
                {
                    _status = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                value = value ?? string.Empty;

                if (_address != value)
                {
                    _address = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                value = value ?? string.Empty;

                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand BackCommand { get; }

        public ICommand ForwardCommand { get; }

        public ICommand RefreshCommand { get; }

        public ICommand GoToBookmarkCommand { get; }

        public ICommand CloseCommand { get; }

        private void OnNavigationStarting(
            object sender,
            Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs e)
        {
            Status = UITexts.Loading;

            var address = e.Uri.ToString();

            if (address == "about:blank") return;

            var uri = _browserUrl.GetUri(address);

            if (uri == null)
            {
                e.Cancel = true;
                Status = string.Format(UITexts.AddressIsNotAllowed, address);
            }

            CommandManager.InvalidateRequerySuggested();
        }

        private void OnNavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                Address = e.Uri.ToString();
                Status = UITexts.PageIsLoaded;
                Title = string.IsNullOrWhiteSpace(Browser.DocumentTitle) 
                    ? UITexts.AbsentPageTitle
                    : Browser.DocumentTitle;
            }
            else
            {
                Status = UITexts.PageFailedToLoad;
                Title = UITexts.AbsentPageTitle;
            }

            CommandManager.InvalidateRequerySuggested();
        }

        public void GoToAddress(string address)
        {
            var uri = _browserUrl.GetUri(address);

            if (uri != null)
            {
                Browser.Navigate(uri);
            }
            else
            {
                Status = string.Format(UITexts.AddressIsNotAllowed, address);
            }
        }
    }
}
