using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ExploreFlicker.Helpers;
using Newtonsoft.Json;

namespace FlickrExplorer.DataServices.Requests
{
    public class BaseRequest
    {
        public HttpMessageHandler HttpMessageHandler { get; set; }
        public string RequestUrl { get; set; }
        /// <summary>
        /// Avoid retrieving results from the client side cache, Default is true.
        /// </summary>
        public bool NoCahce { get; set; }

        /// <summary>
        /// Default is true, which will dispose handler automatically
        /// </summary>
        public bool DisposeHandler = true;

        private readonly INetworkHelper _networkHelper;

        private ContentType _resultContentType = ContentType.Json;
        /// <summary>
        /// Default is Json
        /// </summary>
        public ContentType ResultContentType
        {
            get { return _resultContentType; }
            set { _resultContentType = value; }
        }

        /// <summary>
        /// Override User Agent of request
        /// </summary>
        public string UserAgent { get; set; }


        public BaseRequest ( INetworkHelper networkHelper )
        {
            _networkHelper = networkHelper;
            NoCahce = true;
        }

        public async Task<ResponseWrapper<TR>> PostAsync<TR> ( StringContent content, CancellationToken? token = null )
        {

            var response = new ResponseWrapper<TR>();
            HttpClient httpClient = null;
            try
            {
                if(!_networkHelper.HasInternetAccess())
                {
                    response.ResponseStatus = ResponseStatus.NoInternet;
                    return response;
                }

                httpClient = HttpMessageHandler != null ?
                    new HttpClient(HttpMessageHandler, DisposeHandler) :
                    new HttpClient(new HttpClientHandler(), DisposeHandler);
                if(NoCahce)
                    httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };

                if(!String.IsNullOrEmpty(UserAgent))
                    httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                HttpResponseMessage postResponse;
                if(token == null)
                    postResponse = await httpClient.PostAsync(RequestUrl, content);
                else
                    postResponse = await httpClient.PostAsync(RequestUrl, content, token.Value);

                if(token != null && token.Value.IsCancellationRequested)
                {
                    response.ResponseStatus = ResponseStatus.UserCanceled;
                    return response;
                }

                var stringValue = await postResponse.Content.ReadAsStringAsync();
                response.StatusCode = postResponse.StatusCode;
                ParseResult(stringValue, response);//parse Result even when there is error
                if(!postResponse.IsSuccessStatusCode)
                {
                    response.ResponseStatus = ResponseStatus.HttpError;//Override responseWrapper status value to be HttpError
                }
                return response;
            }
            catch(OperationCanceledException)
            {
                if(token != null && token.Value.IsCancellationRequested)
                    response.ResponseStatus = ResponseStatus.UserCanceled;
                else
                    response.ResponseStatus = ResponseStatus.TimeOut;
            }
            catch(WebException)
            {
                response.ResponseStatus = ResponseStatus.HttpError;
            }
            catch(HttpRequestException)
            {
                response.ResponseStatus = ResponseStatus.HttpError;
            }
            catch(Exception)
            {
                response.ResponseStatus = ResponseStatus.ClientSideError;
            }
            finally
            {
                if(httpClient != null) httpClient.Dispose();

            }
            return response;
        }

        public async Task<ResponseWrapper<TR>> GetAsync<TR> ( CancellationToken? token = null )
        {
            var response = new ResponseWrapper<TR>();
            HttpClient httpClient = null;
            try
            {
                if(!_networkHelper.HasInternetAccess())
                {
                    response.ResponseStatus = ResponseStatus.NoInternet;
                    return response;
                }
                httpClient = HttpMessageHandler != null ?
                      new HttpClient(HttpMessageHandler, DisposeHandler) :
                      new HttpClient(new HttpClientHandler(), DisposeHandler);

                if(NoCahce)
                    httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
                HttpResponseMessage getResponse;
                if(token == null)
                    getResponse = await httpClient.GetAsync(RequestUrl);
                else
                    getResponse = await httpClient.GetAsync(RequestUrl, token.Value);

                if(token != null && token.Value.IsCancellationRequested)
                {
                    response.ResponseStatus = ResponseStatus.UserCanceled;
                    return response;
                }

                response.StatusCode = getResponse.StatusCode;
                var stringValue = await getResponse.Content.ReadAsStringAsync();
                ParseResult(stringValue, response);//parse Result even when there is error
                if(!getResponse.IsSuccessStatusCode)
                {
                    response.ResponseStatus = ResponseStatus.HttpError;//Override responseWrapper status value to be HttpError
                }

                return response;
            }
            catch(OperationCanceledException)
            {
                if(token != null && token.Value.IsCancellationRequested)
                    response.ResponseStatus = ResponseStatus.UserCanceled;
                else
                    response.ResponseStatus = ResponseStatus.TimeOut;
            }
            catch(WebException)
            {
                response.ResponseStatus = ResponseStatus.HttpError;
            }
            catch(HttpRequestException)
            {
                response.ResponseStatus = ResponseStatus.HttpError;
            }
            catch(Exception)
            {
                response.ResponseStatus = ResponseStatus.ClientSideError;
            }
            finally
            {
                if(httpClient != null) httpClient.Dispose();
            }
            return response;
        }

        private void ParseResult<TR> ( string stringValue, ResponseWrapper<TR> responseWrapper )
        {
            if(String.IsNullOrWhiteSpace(stringValue))
            {
                responseWrapper.ResponseStatus = ResponseStatus.SuccessWithNoData;
                return;
            }
            if(ResultContentType == ContentType.Json)
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<TR>(stringValue);
                    responseWrapper.Result = result;
                    responseWrapper.ResponseStatus = ResponseStatus.SuccessWithResult;
                }
                catch(Exception)
                {
                    responseWrapper.ResponseStatus = ResponseStatus.ParserException;
                    return;
                }
            }
            else if(ResultContentType == ContentType.Text)
            {
                try
                {
                    responseWrapper.Result = (TR)Convert.ChangeType(stringValue, typeof(TR));
                }
                catch(Exception)
                {
                    responseWrapper.ResponseStatus = ResponseStatus.ParserException;
                    return;
                }
            }

            if(responseWrapper.Result.Equals(default(TR))) responseWrapper.ResponseStatus = ResponseStatus.SuccessWithNoData;
            //Check if the result is of type Ilist, and the count ==0, if true assign ResponseStatus to no Data
            else if(responseWrapper.Result is IList && (responseWrapper.Result as IList).Count == 0) responseWrapper.ResponseStatus = ResponseStatus.SuccessWithNoData;
        }
    }

    public enum ContentType
    {
        Text,
        Json,
        Xml,
    }
}
