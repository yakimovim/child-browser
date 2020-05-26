using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChildBrowser
{
    abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) return;

            var handlers = PropertyChanged;

            if (handlers == null) return;

            foreach (PropertyChangedEventHandler handler in handlers.GetInvocationList())
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
