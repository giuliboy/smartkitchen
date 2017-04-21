using System;
using System.ComponentModel;

namespace HSR.CloudSolutions.SmartKitchen.UI.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {
            
        }
    }
}
