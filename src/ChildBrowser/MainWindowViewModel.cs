using ChildBrowser.Bookmarks;
using Microsoft.Toolkit.Win32.UI.Controls;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ChildBrowser
{
    class MainWindowViewModel : ViewModel
    {
        private readonly BookmarksViewModel _bookmarksViewModel = new BookmarksViewModel();
        private readonly IWebView _browser;
        private readonly BrowserUrl _browserUrl;
        private string _status = string.Empty;

        public MainWindowViewModel(
            IWebView browser,
            BrowserUrl browserUrl)
        {
            _browser = browser ?? throw new ArgumentNullException(nameof(browser));
            _browserUrl = browserUrl ?? throw new ArgumentNullException(nameof(browserUrl));

            ExitCommand = new RelayCommand(
                (arg) => Application.Current.Shutdown()
            );

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

            Bookmarks = _bookmarksViewModel.Bookmarks;

            AddBookmarkCommand = new RelayCommand(
                (arg) =>
                {
                    var viewModel = new BookmarkViewModel(_bookmarksViewModel)
                    {
                        Title = _browser.DocumentTitle,
                        Address = _browser.Source.ToString()
                    };

                    var dialog = new EditBookmark(viewModel);

                    if(dialog.ShowDialog() == true)
                    {
                        _bookmarksViewModel.AddBookmarkCommand.Execute(viewModel);
                    }
                }
            );

            GoToBookmarkCommand = new RelayCommand(arg => {
                var address = (string)arg;

                var uri = _browserUrl.GetUri(address);

                if (uri != null)
                {
                    browser.Navigate(uri);
                }
                else
                {
                    Status = $"Address '{address}' is not allowed";
                }
            });
        }

        public ObservableCollection<BookmarkViewModel> Bookmarks { get; }

        public ICommand AddBookmarkCommand { get; }

        public ICommand GoToBookmarkCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand BackCommand { get; }

        public ICommand ForwardCommand { get; }

        public ICommand RefreshCommand { get; }

        public string Status
        {
            get => _status;
            set
            {
                if(_status != value)
                {
                    _status = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
