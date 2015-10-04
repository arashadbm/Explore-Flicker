// @AhmedRashad Based on saramgsilva solution---------------------------------------------------------------------------------
// <copyright file="NavigationService.cs" company="saramgsilva">
//   Copyright (c) 2012 saramgsilva. All rights reserved.
// </copyright>
// <summary>
//   Defines the NavigationService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExploreFlicker.Helpers.Interfaces;

namespace ExploreFlicker.Helpers
{
    public class NavigationService : INavigationService
    {
        /// <summary>
        /// The view model routing.
        /// </summary>
        private readonly Dictionary<Type, Type> _viewModelRouting = new Dictionary<Type, Type>();

        /// <summary>
        /// To be able to Navigate to a view model, 
        /// You need to register the mapping between this view model and its view by Register<TViewModelType, PageType>
        /// </summary>
        /// <typeparam name="TViewModelType"></typeparam>
        /// <param name="parameter"></param>
        public void NavigateByViewModel<TViewModelType> ( object parameter = null )
        {
            var dest = _viewModelRouting[typeof(TViewModelType)];

            RootFrame.Navigate(dest, parameter);
        }

        /// <summary>
        /// This will be used only in rare cases when you to naviagte to a view which doesn't have viewmodel mapping
        /// If the page has view model Use NavigateByViewModel
        /// </summary>
        /// <typeparam name="TPageType"></typeparam>
        /// <param name="parameter"></param>
        public void NavigateByPage<TPageType> ( object parameter = null ) where TPageType : Page
        {
            RootFrame.Navigate(typeof(TPageType), parameter);
        }

        public void NavigateByPage ( Type pageType, object parameter = null )
        {
            RootFrame.Navigate(pageType, parameter);
        }

        public void NavigateBack ()
        {
            RootFrame.GoBack();
        }

        public void NavigateForward ()
        {
            RootFrame.GoForward();
        }

        /// <summary>
        /// Gets the root frame.
        /// </summary>
        private static Frame RootFrame
        {
            get { return Window.Current.Content as Frame; }
        }

        /// <summary>
        /// Gets a value indicating whether can go back.
        /// </summary>
        public bool CanGoBack ()
        {
            return RootFrame.CanGoBack;
        }

        public void ClearBackStack ()
        {
            while(RootFrame.BackStack.Count > 0)
                RootFrame.BackStack.RemoveAt(RootFrame.BackStack.Count - 1);
        }

        /// <summary>
        /// The go back.
        /// </summary>
        public void GoBack ()
        {
            RootFrame.GoBack();
        }

        public void Register<TViewModelType, TPageType> ()
        {
            _viewModelRouting[typeof(TViewModelType)] = typeof(TPageType);
        }
    }
}
