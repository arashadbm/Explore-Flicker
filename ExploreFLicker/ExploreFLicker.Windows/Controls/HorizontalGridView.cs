using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SuperMama.Lists;

namespace ExploreFlicker.Controls
{
    [TemplatePart(Name = ScrollViewerPartName, Type = typeof(ScrollViewer))]
    public class HorizontalGridView : GridView
    {
        #region Fields
        /// <summary>
        /// This value is compared against the remaining offset in scrollviewer to end, 
        /// when the remaining is less than it, it will trigger load more event.
        /// A value of zero will make user aware that he reached end before loading again.
        /// Other values implement infinite scrolling.
        /// </summary>
        private const int OffsetToFire = 1200;

        //Not Used
        /// <summary>
        /// This value is compared against the scrolled percentage of scrollviewer, 
        /// If the scrolled percentage is larger, it will trigger load more event.
        /// A value of 100% will make user aware that he reached end before loading again.
        /// Other values implement infinite scrolling.
        /// </summary>
        private const double LoadAfterScrollPrecentage = 70;
        private const String ScrollViewerPartName = "ScrollViewer";
        private ScrollViewer _rootScrollViewer;
        #endregion

        #region IsLoading More Enabled dp

        public bool IsLoadingMoreEnabled
        {
            get { return (bool)GetValue(IsLoadingMoreEnabledProperty); }
            set { SetValue(IsLoadingMoreEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoadingMoreEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingMoreEnabledProperty =
            DependencyProperty.Register("IsLoadingMoreEnabled", typeof(bool), typeof(HorizontalGridView), new PropertyMetadata(false));

        #endregion

        #region LoadMoreTemplate dp

        public DataTemplate LoadMoreTemplate
        {
            get { return (DataTemplate)GetValue(LoadMoreTemplateProperty); }
            set { SetValue(LoadMoreTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadMoreTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadMoreTemplateProperty =
            DependencyProperty.Register("LoadMoreTemplate", typeof(DataTemplate), typeof(HorizontalGridView), new PropertyMetadata(null));

        #endregion

        #region LoadMoreProgressTemplate


        public DataTemplate LoadMoreProgressTemplate
        {
            get { return (DataTemplate)GetValue(LoadMoreProgressTemplateProperty); }
            set { SetValue(LoadMoreProgressTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadMoreProgressTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadMoreProgressTemplateProperty =
            DependencyProperty.Register("LoadMoreProgressTemplate", typeof(DataTemplate), typeof(HorizontalGridView), new PropertyMetadata(null));


        private bool _isLoadingMore;

        public bool IsLoadingMore
        {
            get { return _isLoadingMore; }
            protected set
            {
                _isLoadingMore = value;
                UpdateLoadMoreState(true);
            }
        }
        #endregion

        #region Empty  Content Template

        public DataTemplate EmptyContentTemplate
        {
            get { return (DataTemplate)GetValue(EmptyContentTemplateProperty); }
            set { SetValue(EmptyContentTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmptyContentTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmptyContentTemplateProperty =
            DependencyProperty.Register("EmptyContentTemplate", typeof(DataTemplate), typeof(HorizontalGridView), new PropertyMetadata(null));


        #endregion

        #region Empty Content
        public object EmptyContent
        {
            get { return (object)GetValue(EmptyContentProperty); }
            set { SetValue(EmptyContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmptyContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmptyContentProperty =
            DependencyProperty.Register("EmptyContent", typeof(object), typeof(HorizontalGridView), new PropertyMetadata(null));


        #endregion

        #region EmptyRefreshCommand

        public ICommand EmptyRefreshCommand
        {
            get { return (ICommand)GetValue(EmptyRefreshCommandProperty); }
            set { SetValue(EmptyRefreshCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmptyRefreshCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmptyRefreshCommandProperty =
            DependencyProperty.Register("EmptyRefreshCommand", typeof(ICommand), typeof(HorizontalGridView), new PropertyMetadata(null));

        #endregion

        #region  Empty Content Mode

        public EmptyContentMode EmptyContentMode
        {
            get { return (EmptyContentMode)GetValue(EmptyContentModeProperty); }
            set { SetValue(EmptyContentModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EmptyContentMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EmptyContentModeProperty =
            DependencyProperty.Register("EmptyContentMode", typeof(EmptyContentMode), typeof(HorizontalGridView), new PropertyMetadata(EmptyContentMode.Empty, OnEmptyContentModeChanged));

        private static void OnEmptyContentModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region Initialization
        public HorizontalGridView()
        {
            DefaultStyleKey = typeof(HorizontalGridView);
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateLoadMoreState(false);
            _rootScrollViewer = (ScrollViewer)GetTemplateChild(ScrollViewerPartName);
            if (_rootScrollViewer != null)
            {
                _rootScrollViewer.SizeChanged += _rootScrollViewer_SizeChanged;
                _rootScrollViewer.ViewChanged += _rootScrollViewer_ViewChanged;
            }
        }
        #endregion

        void UpdateLoadMoreState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsLoadingMore ? "LoadMoreProgress" : "NoLoadMore", useTransitions);
        }

        #region ScrollViewer Events

        private void _rootScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            double pos = _rootScrollViewer.HorizontalOffset;
            // if (e.IsIntermediate) return;
            Debug.WriteLine("diff: " + Math.Abs(pos - _rootScrollViewer.ScrollableWidth));

            //If items are few, There shouldn't be load more
            if (Math.Abs(_rootScrollViewer.ScrollableWidth - _rootScrollViewer.ActualWidth) < 1e-6)
                return;

            if (Math.Abs(_rootScrollViewer.ScrollableWidth) > 1e-6 && Math.Abs(pos - _rootScrollViewer.ScrollableWidth) <= OffsetToFire)
            {
                //TODO:Check why it will call twice if there is not execution code in LoadMoreRequested event

                try
                {
                    if (IsLoadingMoreEnabled && !IsLoadingMore && LoadMoreRequested != null)
                    {
                        IsLoadingMore = true;
                        _rootScrollViewer.ChangeView(null, _rootScrollViewer.ScrollableHeight - 8, null);
                        UpdateLoadMoreState(true);
                        OnLoadMoreRequested(new LoadMoreEventArgs(this));
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("Load More Exception");
                }
            }
        }

        void _rootScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
        #endregion

        #region Load More Event & Class
        protected virtual void OnLoadMoreRequested(LoadMoreEventArgs e)
        {
            var handler = LoadMoreRequested;
            if (handler != null) handler(this, e);
        }
        public event EventHandler<LoadMoreEventArgs> LoadMoreRequested;

        public class LoadMoreEventArgs
        {
            private readonly HorizontalGridView _gridView;
            public LoadMoreEventArgs(HorizontalGridView gridView)
            {
                _gridView = gridView;
            }

            /// <summary>
            /// Set this value to false to collpase Loading more UI.
            /// </summary>
            public bool IsLoadingMore
            {
                get
                {
                    if (_gridView != null)
                        return _gridView.IsLoadingMore;
                    return false;
                }
                set
                {
                    if (_gridView != null)
                        _gridView.IsLoadingMore = value;
                }
            }
            #endregion

        }
    }

}
