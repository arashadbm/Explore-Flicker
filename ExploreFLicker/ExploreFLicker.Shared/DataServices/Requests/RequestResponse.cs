using System.Net;

namespace FlickrExplorer.DataServices.Requests
{
    public class RequestResponse
    {
        public ResponseStatus ResponseStatus { get; set; }
        /// <summary>
        /// Check this value only when response status equals HttpError
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        // public HttpStatusCode? StatusCode { get; set; }
    }

    public class RequestResponse<T> : RequestResponse
    {
        public T Result { get; set; }
    }

    public enum ResponseStatus
    {
        SuccessWithResult,
        NoInternet,
        HttpError,
        SuccessWithNoData,//Operation with server went successfully,but server returns empty response
        ClientSideError,
        TimeOut,//Request Timedout
        ParserException,//Input returned from server wasn't parsed correctly and throws exception
        UserCanceled,//User has canceled the request
        MalformedRequest//Request is malformed, like missing request url method
    }
}
