using Windows.Networking.Connectivity;
using ExploreFlicker.Helpers;

namespace ExploreFlicker.Helpers
{
    public class NetworkHelper : INetworkHelper
    {
        public bool HasInternetAccess ()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
    }
}
