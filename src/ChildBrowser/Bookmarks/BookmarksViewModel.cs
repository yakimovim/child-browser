using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ChildBrowser.Bookmarks
{
    class BookmarksViewModel : ViewModel
    {
        private readonly BookmarksStorage _storage;

        public BookmarksViewModel()
        {
            _storage = new BookmarksStorage();

            foreach (var bookmark in _storage.Bookmarks)
            {
                Bookmarks.Add(new BookmarkViewModel(this, bookmark));
            }

            AddBookmarkCommand = new RelayCommand(arg => {
                var bookmarkViewModel = (BookmarkViewModel)arg;

                _storage.Add(bookmarkViewModel.Bookmark);

                Bookmarks.Add(bookmarkViewModel);
            });

            DeleteBookmarkCommand = new RelayCommand(arg => {
                var bookmarkViewModel = (BookmarkViewModel)arg;

                if(MessageBox.Show("Do you really want to delete this bookmark?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _storage.Remove(bookmarkViewModel.Bookmark);

                    Bookmarks.Remove(bookmarkViewModel);
                }
            });

            EditBookmarkCommand = new RelayCommand(arg => {
                var bookmarkViewModel = (BookmarkViewModel)arg;

                var editable = bookmarkViewModel.Clone();

                var dialog = new EditBookmark(editable);

                if (dialog.ShowDialog() == true)
                {
                    bookmarkViewModel.FillFrom(editable);

                    _storage.Save();
                }
            });
        }

        public ObservableCollection<BookmarkViewModel> Bookmarks { get; } = new ObservableCollection<BookmarkViewModel>();

        public ICommand AddBookmarkCommand { get; }

        public ICommand EditBookmarkCommand { get; }

        public ICommand DeleteBookmarkCommand { get; }
    }
}
