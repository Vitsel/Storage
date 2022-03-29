using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Challenge5.Model
{
    public class NotifiableModel : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}