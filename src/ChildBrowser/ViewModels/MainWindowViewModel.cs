using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Globalization;
using ChildBrowser.Views;

namespace ChildBrowser.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private readonly BookmarksViewModel _bookmarksViewModel = new BookmarksViewModel();
        private readonly BrowserUrl _browserUrl;
        private readonly AddressCompletionProvider _addressCompletionProvider = new AddressCompletionProvider();
        private BrowserViewModel _selectedBrowser;

        public MainWindowViewModel(
            BrowserUrl browserUrl)
        {
            _browserUrl = browserUrl ?? throw new ArgumentNullException(nameof(browserUrl));

            ExitCommand = new RelayCommand(
                (arg) => Application.Current.Shutdown()
            );

            Bookmarks = _bookmarksViewModel.Bookmarks;

            _addressCompletionProvider.RegisterProvider(new BookmarksUriProvider(() => _bookmarksViewModel.Bookmarks.Select(b => b.Bookmark)));
            _addressCompletionProvider.RegisterProvider(new AllowedUrisProvider(_browserUrl));

            AddBookmarkCommand = new RelayCommand(
                (arg) =>
                {
                    var viewModel = new BookmarkViewModel()
                    {
                        Title = SelectedBrowser.Browser.DocumentTitle,
                        Address = SelectedBrowser.Browser.Source.ToString()
                    };

                    var dialog = new EditBookmark(viewModel);

                    if(dialog.ShowDialog() == true)
                    {
                        _bookmarksViewModel.AddBookmarkCommand.Execute(viewModel);
                    }
                }
            );

            AddNewTabCommand = new RelayCommand(arg =>
            {
                var viewModel = new BrowserViewModel(_browserUrl);
                viewModel.Closing += OnBrowserTabClosing;

                Browsers.Add(viewModel);

                SelectedBrowser = viewModel;
            });

            SetLanguageCommand = new RelayCommand(arg => {
                var language = (string)arg;

                if(string.IsNullOrWhiteSpace(language)
                || CultureInfo.GetCultureInfo(language) == null)
                {
                    Configuration.Language = null;
                }
                else
                {
                    Configuration.Language = language;
                }

                NotifyPropertyChanged(nameof(Language));

                Configuration.SetCurrentCulture();
            });

            _selectedBrowser = new BrowserViewModel(_browserUrl);
            _selectedBrowser.Closing += OnBrowserTabClosing;

            Browsers.Add(_selectedBrowser);
        }

        public string GetAddressCompletion(string notSelectedPartOfAddress)
        {
            return _addressCompletionProvider.GetAddressCompletion(notSelectedPartOfAddress);
        }

        public ObservableCollection<BookmarkViewModel> Bookmarks { get; }

        public string Language => Configuration.Language;

        public ICommand AddBookmarkCommand { get; }

        public ICommand AddNewTabCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand SetLanguageCommand { get; }

        public ObservableCollection<BrowserViewModel> Browsers { get; } = new ObservableCollection<BrowserViewModel>();

        public BrowserViewModel SelectedBrowser
        {
            get => _selectedBrowser;
            set
            {
                if (_selectedBrowser != value)
                {
                    _selectedBrowser = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void OnBrowserTabClosing(object sender, BrowserViewModel viewModel)
        {
            // Do not close the only tab
            if (Browsers.Count == 1) return;

            viewModel.Closing -= OnBrowserTabClosing;

            var index = Browsers.IndexOf(viewModel);

            Browsers.Remove(viewModel);

            if(SelectedBrowser == viewModel)
            {
                SelectedBrowser = Browsers[Math.Min(index, Browsers.Count - 1)];
            }
        }
    }
}
