using System;
using System.Windows;
using System.Windows.Input;

namespace ChildBrowser.Bookmarks
{
    class BookmarkViewModel : ViewModel
    {
        private readonly BookmarksViewModel _bookmarksViewModel;

        public BookmarkViewModel(BookmarksViewModel bookmarksViewModel, Bookmark bookmark = null)
        {
            _bookmarksViewModel = bookmarksViewModel ?? throw new ArgumentNullException(nameof(bookmarksViewModel));
            Bookmark = bookmark ?? new Bookmark();

            OkCommand = new RelayCommand(arg => ((Window)arg).DialogResult = true);
            CancelCommand = new RelayCommand(arg => ((Window)arg).DialogResult = false);

            DeleteBookmarkCommand = new RelayCommand(arg => {
                _bookmarksViewModel.DeleteBookmarkCommand.Execute(this);
            });

            EditBookmarkCommand = new RelayCommand(arg => {
                _bookmarksViewModel.EditBookmarkCommand.Execute(this);
            });
        }

        public string Title
        {
            get => Bookmark.Title;
            set
            {
                if (Bookmark.Title != value)
                {
                    Bookmark.Title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => Bookmark.Address;
            set
            {
                if (Bookmark.Address != value)
                {
                    Bookmark.Address = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Bookmark Bookmark { get; }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand DeleteBookmarkCommand { get; }

        public ICommand EditBookmarkCommand { get; }

        public BookmarkViewModel Clone()
        {
            return new BookmarkViewModel(_bookmarksViewModel)
            {
                Title = Title,
                Address = Address
            };
        }

        public void FillFrom(BookmarkViewModel bookmarkViewModel)
        {
            if (bookmarkViewModel is null)
            {
                throw new ArgumentNullException(nameof(bookmarkViewModel));
            }

            Title = bookmarkViewModel.Title;
            Address = bookmarkViewModel.Address;
        }
    }
}
