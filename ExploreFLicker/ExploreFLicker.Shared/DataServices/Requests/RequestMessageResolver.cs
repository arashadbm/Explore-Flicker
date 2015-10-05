using ExploreFlickr.Strings;
using ExploreFlicker.Assets;
using FlickrExplorer.DataServices.Interfaces;

namespace FlickrExplorer.DataServices.Requests
{
    public class RequestMessageResolver : IRequestMessageResolver
    {
        private readonly Resources _resources;

        public RequestMessageResolver ( Resources resources )
        {
            _resources = resources;
        }

        public RequestMessage ResultToMessage (ResponseWrapper response )
        {

            switch(response.ResponseStatus)
            {
                case ResponseStatus.NoInternet:
                    return new RequestMessage(_resources.NoInternet, GeometryPaths.NoInternet);
                case ResponseStatus.HttpError:
                    return new RequestMessage(_resources.ErrorConnecting, GeometryPaths.NoInternet);
                case ResponseStatus.SuccessWithNoData:
                    return new RequestMessage(_resources.NoData, GeometryPaths.NoResults);
                case ResponseStatus.ParserException:
                case ResponseStatus.ClientSideError:
                    return new RequestMessage(_resources.ClientSideError, GeometryPaths.NoResults);
                case ResponseStatus.SuccessWithResult:
                    return new RequestMessage(_resources.Success, null) { MessageType = RequestMessageType.Info };
                default:
                    return new RequestMessage("", null) { MessageType = RequestMessageType.Info };
            }
        }
    }
}
