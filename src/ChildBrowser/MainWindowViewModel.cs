using Microsoft.Toolkit.Win32.UI.Controls;
using Microsoft.Toolkit.Wpf.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace ChildBrowser
{
    class MainWindowViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand BackCommand { get; }

        public ICommand ForwardCommand { get; }

        public ICommand RefreshCommand { get; }
    }
}
