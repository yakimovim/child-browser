using ChildBrowser.Bookmarks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
