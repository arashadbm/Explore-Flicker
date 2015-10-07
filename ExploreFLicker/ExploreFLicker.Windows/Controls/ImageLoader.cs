using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ExploreFlicker.Controls
{

    //Original class is here http://www.getcodesamples.com/src/93B434/E6246C1F
    //Control ported by Ahmed Rashad to winrt

    /// <summary>
    /// A wrapper around the Image control which displays a loader and fades the image in when loaded.
    /// </summary>
    [TemplatePart(Name = "PART_Image", Type = typeof(Image))]
    public class ImageLoader : Control
    {
        #region Fields
        /// <summary>
        /// The image control which displays the image.
        /// </summary>
        private Image _image;

        /// <summary>
        /// This is the last bitmap used to update image.
        /// </summary>
        private BitmapImage _internalBitmap;

        private bool _isLoaded;
        #endregion

        #region Properties

        private static bool DesignMode
        {
            get { return Windows.ApplicationModel.DesignMode.DesignModeEnabled; }
        }

        /// <summary>
        /// Gets the image displayed in this control.
        /// </summary>
        /// <value>The image displayed in this control.</value>
        internal BitmapSource BitmapImage
        {
            get
            {
                return _image == null ? null : _image.Source as BitmapSource;
            }
        }

        #region UriSource dp

        public Uri UriSource
        {
            get { return (Uri)GetValue(UriSourceProperty); }
            set { SetValue(UriSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UriSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UriSourceProperty =
            DependencyProperty.Register("UriSource", typeof(Uri), typeof(ImageLoader),
            new PropertyMetadata(null, ( sender, e ) => ((ImageLoader)sender).UpdateUriSource(e.OldValue as Uri)));

        /// <summary>
        /// Updates the UriSource.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        private void UpdateUriSource ( Uri oldValue )
        {
            if(_image == null || !_isLoaded)
            {
                return;
            }

            if(DesignMode)
            {
                _image.Opacity = 1;
                return;
            }

            if(oldValue != null && UriSource != null && oldValue.OriginalString == UriSource.OriginalString)
            {
                return;
            }

            _image.Source = null;

            if(UriSource == null || String.IsNullOrEmpty(UriSource.OriginalString))
            {
                return;
            }

            IsImageLoading = true;

            UnSubscribe();

            _internalBitmap = new BitmapImage();
            if(RenderAtSize)
            {
                UpdateLayout();
                _internalBitmap.DecodePixelWidth = (int)Math.Max(ActualWidth, ActualHeight);
            }
            _internalBitmap.ImageOpened += InternalBitmapImageOpened;
            _internalBitmap.ImageFailed += InternalBitmapImageFailed;

            //Start loading
            _internalBitmap.UriSource = UriSource;
            _image.Source = _internalBitmap;

        }

        void InternalBitmapImageFailed ( object sender, ExceptionRoutedEventArgs e )
        {
            UnSubscribe();
            IsImageLoading = false;
        }

        void InternalBitmapImageOpened ( object sender, RoutedEventArgs e )
        {
            UnSubscribe();
            IsImageLoading = false;
            //_image.Source = e.OriginalSource as BitmapImage;
            OnImageLoaded();
        }

        private void UnSubscribe ()
        {
            if(_internalBitmap != null)
            {
                _internalBitmap.ImageOpened -= InternalBitmapImageOpened;
                _internalBitmap.ImageFailed -= InternalBitmapImageFailed;
            }
        }

        #endregion

        #region
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageLoader), new PropertyMetadata(Windows.UI.Xaml.Media.Stretch.None));



        #endregion

        #region IsImageLoading

        /// <summary>
        /// Gets or sets a value indicating whether the image is loading.
        /// </summary>
        /// <value>
        /// <c>true</c> if the image is loading; otherwise, <c>false</c>.
        /// </value>
        public bool IsImageLoading
        {
            get { return (bool)GetValue(IsImageLoadingProperty); }
            set { SetValue(IsImageLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsImageLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsImageLoadingProperty =
            DependencyProperty.Register("IsImageLoading", typeof(bool), typeof(ImageLoader), new PropertyMetadata(false));

        #endregion

        #region RenderAtSize

        /// <summary>
        /// Gets or sets a value indicating whether the image should be rendered at the size of the loader. Set to true if the image will never be scaled up.
        /// </summary>
        /// <value><c>true</c> if [render at size]; otherwise, <c>false</c>.</value>
        public bool RenderAtSize
        {
            get { return (bool)GetValue(RenderAtSizeProperty); }
            set { SetValue(RenderAtSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RenderAtSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RenderAtSizeProperty =
            DependencyProperty.Register("RenderAtSize", typeof(bool), typeof(ImageLoader), new PropertyMetadata(false));


        #endregion

        #endregion//end of properties region

        #region Initialization

        public ImageLoader ()
        {
            DefaultStyleKey = typeof(ImageLoader);
        }
        #endregion

        #region Methods
        protected override void OnApplyTemplate ()
        {
            if(!DesignMode)
            {
                _image = GetTemplateChild("PART_Image") as Image;
                Loaded += ImageLoader_Loaded;
            }

            base.OnApplyTemplate();
        }

        void ImageLoader_Loaded ( object sender, Windows.UI.Xaml.RoutedEventArgs e )
        {
            _isLoaded = true;
            UpdateUriSource(null);
            Loaded -= ImageLoader_Loaded;
        }


        #endregion
/*
        #region IDisposable

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose ( bool disposing )
        {
            if(disposing)
            {

                if(_internalBitmap != null)
                {
                    try
                    {
                        _internalBitmap.UriSource = new Uri("");
                    }
                    catch
                    {
                        // ignored
                    }
                    _internalBitmap = null;
                }
                if(_image.Source != null)
                {
                    _image.Source = null;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        */

        

        #region Events
        /// <summary>
        /// Occurs when the image has loaded, or failed loading.
        /// </summary>
        public event EventHandler ImageLoaded;
        #endregion

        protected virtual void OnImageLoaded ()
        {
            var handler = ImageLoaded;
            if(handler != null) handler(this, EventArgs.Empty);
        }
    }
}