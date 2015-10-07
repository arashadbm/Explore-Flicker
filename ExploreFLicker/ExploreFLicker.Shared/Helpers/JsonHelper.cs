using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;

namespace ExploreFlicker.Helpers
{
    public class JsonHelper
    {
        public async Task<T> DeserializeJsonFileAsync<T> ( String path )
        {
            Uri dataUri = new Uri(path);

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            T ret = await Task.Factory.StartNew<T>(() =>
            {

                T result = JsonConvert.DeserializeObject<T>(jsonText);
                return result;
            });
            return ret;
        }
    }
}
