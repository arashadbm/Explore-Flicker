using ExploreFlicker.Viewmodels;
using ExploreFLicker.Assets;

namespace FlickrExplorer.DataServices.Requests
{
    public class RequestMessage
    {
        #region Properties
        public string Description { get; set; }

        /// <summary>
        /// Represents Gemoetry path of icon
        /// </summary>
        public string GeometryPath { get; set; }

        private RequestMessageType _messageType = RequestMessageType.Error;
        public RequestMessageType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }
        #endregion

        #region Initialization
        public RequestMessage ( string description, string geometryPath )
            : this(description)
        {
            GeometryPath = geometryPath;
        }
        public RequestMessage ( string description )
            : this()
        {
            Description = description;
        }

        public RequestMessage ()
        {

        }
        #endregion

        #region Methods

        /// <summary>
        /// Overidded ToString to return description, to avoid cases that I didn't bind to Description property.
        /// </summary>
        /// <returns></returns>
        public override string ToString ()
        {
            return Description;
        }

        public static RequestMessage GetNoDataMessage ()
        {
            return new RequestMessage(ViewModelLocator.Resources.NoData, GeometryPaths.NoResults);
        }

        public static RequestMessage GetNoDataMessage ( string message )
        {
            return new RequestMessage(message, GeometryPaths.NoResults);
        }
        public static RequestMessage GetClientErrorMessage ()
        {
            return new RequestMessage(ViewModelLocator.Resources.ClientSideError, GeometryPaths.NoResults);
        }

        #endregion

    }

    public enum RequestMessageType
    {
        None,
        Error,
        Warning,
        Info
    }
}
