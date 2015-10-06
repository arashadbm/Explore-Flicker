using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace ExploreFlicker.Controls
{
    public class SearchTextBox : TextBox
    {
        #region Fields
        private Storyboard _storyboard;

        #endregion

        #region Search Requested Command Dp

        public ICommand SearchRequestedCommand
        {
            get { return (ICommand)GetValue(SearchRequestedCommandProperty); }
            set { SetValue(SearchRequestedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchRequestedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchRequestedCommandProperty =
            DependencyProperty.Register("SearchRequestedCommand", typeof(ICommand), typeof(SearchTextBox), new PropertyMetadata(null));


        #endregion

        #region initialization

        public SearchTextBox()
        {
            DefaultStyleKey = typeof(TextBox);
            _storyboard = new Storyboard()
            {
                Duration = TimeSpan.FromMilliseconds(250)
            };
            _storyboard.Completed += _storyboard_Completed;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TextChanged -= SearchTextBox_TextChanged;
            TextChanged += SearchTextBox_TextChanged;
        }

        #endregion

        #region Methods

        private void _storyboard_Completed(object sender, object e)
        {
            RaiseSearchRequested(Text);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _storyboard.Stop();
            _storyboard.Seek(TimeSpan.Zero);
            _storyboard.Begin();
        }

        protected void RaiseSearchRequested(String searchTerm)
        {
            EventHandler<String> handler = SearchRequested;
            if (handler != null)
            {
                handler(this, searchTerm);
            }

            if (SearchRequestedCommand != null)
            {
                SearchRequestedCommand.Execute(searchTerm);
            }
        }
        #endregion

        #region Event declaration
        public event EventHandler<String> SearchRequested;
        #endregion
    }
}
