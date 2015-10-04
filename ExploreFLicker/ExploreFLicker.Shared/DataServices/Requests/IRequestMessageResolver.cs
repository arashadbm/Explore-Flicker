using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrExplorer.DataServices.Requests;

namespace FlickrExplorer.DataServices.Interfaces
{
    public interface IRequestMessageResolver
    {
        RequestMessage ResultToMessage ( RequestResponse response );
    }
}
