using ChildBrowser.Bookmarks;
using System;
using System.Windows;

namespace ChildBrowser
{
    /// <summary>
    /// Interaction logic for EditBookmark.xaml
    /// </summary>
    public partial class EditBookmark : Window
    {
        internal EditBookmark(BookmarkViewModel viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
