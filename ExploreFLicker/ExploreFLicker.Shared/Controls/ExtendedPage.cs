using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
#endif
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ExploreFlicker.Common;


namespace ExploreFlicker.Controls
{
    public class ExtendedPage : Page
    {
        #region Fields
        protected bool HandlePopups = true;
        #endregion

        #region properties
        private readonly NavigationHelper _navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        private bool _isNewInstance;
        /// <summary>
        /// This value will be true when page is newly navigate to or it was in backstack but removed from cache.
        /// </summary>
        public bool IsNewInstance
        {
            get { return _isNewInstance; }
        }

        private bool _isActive;

        /// <summary>
        /// A value of true means this is the current page in Frame.
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
        }


        private NavigationMode _navigationMode;
        /// <summary>
        /// This value represents OnNavigatedTo navigation mode.
        /// </summary>
        public NavigationMode NavigationMode
        {
            get { return _navigationMode; }
        }

        /// <summary>
        /// True means load data,It's value based on IsNewInstance || NavigationMode != NavigationMode.Back.
        /// Use this property to determine when to load inside 'load State' method.
        /// You can also override this property to add new factors that trigger refresh.
        /// </summary>
        public virtual bool NeedsRefresh
        {
            get { return IsNewInstance || NavigationMode != NavigationMode.Back; }
        }
        #endregion

        public ExtendedPage()
        {
            this._navigationHelper = new NavigationHelper(this);
            this._navigationHelper.LoadState += navigationHelper_LoadState;
            this._navigationHelper.SaveState += navigationHelper_SaveState;

            //This is the only place where it will be set to true
            _isNewInstance = true;
        }

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the LoadState and SaveState
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        #region NavigationHelper registration

        /// <summary>
        /// Override this method to save your state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void SaveState(object sender, SaveStateEventArgs e)
        {
        }

        /// <summary>
        /// Ovveride this method to load your state and access passed parameter if any
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void LoadState(object sender, LoadStateEventArgs e)
        {
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Listen for key press event to handle popups close or any other logic related to back key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (e.Handled) return;
        }
#endif


        /// <summary>
        /// Use LoadState instead of this method, You can access the navigation parameter from LoadState method
        /// If you override this method, Don't remove base.OnNavigatedTo(NavigationEventArgs e)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationMode = e.NavigationMode;

            _isActive = true;

#if WINDOWS_PHONE_APP

            //Listen for Back key press event before Navigation helper does
            //useful to handle back button in some cases
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif

            base.OnNavigatedTo(e);

            NavigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// If you override this method, Don't remove base.OnNavigatedFrom(NavigationEventArgs e)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _isActive = false;

            //Set it's value to false when navigating from this page,
            //This can be used along with other factors to prevent unnecessary reloading.
            _isNewInstance = false;

#if WINDOWS_PHONE_APP
            //Release Back key press event
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

#endif

            base.OnNavigatedFrom(e);

            _navigationHelper.OnNavigatedFrom(e);
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            SaveState(sender, e);
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            LoadState(sender, e);
        }

        #endregion
    }
}
