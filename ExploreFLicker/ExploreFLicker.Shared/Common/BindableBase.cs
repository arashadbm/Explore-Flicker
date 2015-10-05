using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExploreFlicker.Common
{
    public class BindableBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T> ( ref T storage, T value, [CallerMemberName] String propertyName = null )
        {
            if(Equals(storage, value))
                return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
        protected virtual void OnPropertyChanged ( [CallerMemberName] string propertyName = null )
        {
            var eventHandler = this.PropertyChanged;
            if(eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }


        //public bool IsDesignMode
        //{
        //    get { return DesignerProperties.IsInDesignTool; }

        //}

        #endregion
    }

}
