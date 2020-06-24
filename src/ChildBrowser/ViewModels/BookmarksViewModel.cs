using ChildBrowser.Bookmarks;
using ChildBrowser.Resources;
using ChildBrowser.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ChildBrowser.ViewModels
{
    class BookmarksViewModel : ViewModel
    {
        private readonly BookmarksStorage _storage;

        public BookmarksViewModel()
        {
            _storage = new BookmarksStorage();

            foreach (var bookmark in _storage.Bookmarks)
            {
                var viewModel = new BookmarkViewModel(bookmark);

                viewModel.Deleting += OnBookmarkDeleting;
                viewModel.Editing += OnBookmarkEditing;

                Bookmarks.Add(viewModel);
            }

            AddBookmarkCommand = new RelayCommand(arg => {
                var bookmarkViewModel = (BookmarkViewModel)arg;

                bookmarkViewModel.Deleting += OnBookmarkDeleting;
                bookmarkViewModel.Editing += OnBookmarkEditing;

                _storage.Add(bookmarkViewModel.Bookmark);

                Bookmarks.Add(bookmarkViewModel);
            });
        }

        private void OnBookmarkEditing(object sender, BookmarkViewModel bookmarkViewModel)
        {
            var editable = bookmarkViewModel.Clone();

            var dialog = new EditBookmark(editable);

            if (dialog.ShowDialog() == true)
            {
                bookmarkViewModel.FillFrom(editable);

                _storage.Save();
            }
        }

        private void OnBookmarkDeleting(object sender, BookmarkViewModel bookmarkViewModel)
        {
            if (MessageBox.Show(UITexts.DeleteBookmarkConfirmation, UITexts.ConfirmationDialogTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _storage.Remove(bookmarkViewModel.Bookmark);

                Bookmarks.Remove(bookmarkViewModel);

                bookmarkViewModel.Deleting -= OnBookmarkDeleting;
                bookmarkViewModel.Editing -= OnBookmarkEditing;
            }
        }

        public ObservableCollection<BookmarkViewModel> Bookmarks { get; } = new ObservableCollection<BookmarkViewModel>();

        public ICommand AddBookmarkCommand { get; }
    }
}
