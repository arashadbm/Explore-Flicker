using System;
using Windows.UI.Xaml.Controls;

namespace ExploreFlicker.Helpers.Interfaces
{
    public interface INavigationService
    {
        void NavigateByViewModel<TViewModelType> ( object parameter = null );
        void NavigateByPage<TPageType> ( object parameter = null ) where TPageType : Page;
        void NavigateByPage ( Type pageType, object parameter = null );
        void NavigateBack ();
        void NavigateForward ();
        bool CanGoBack ();
        void GoBack ();
        void ClearBackStack ();
        void Register<TViewModelType, TPageType> ();
    }
}
