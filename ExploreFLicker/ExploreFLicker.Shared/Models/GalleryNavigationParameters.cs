using System;
using System.Collections.Generic;
using System.Text;
using ExploreFlicker.Models.Response;

namespace ExploreFLicker.Models
{
    public class GalleryNavigationParameters
    {
        public Photo SelectedPhoto { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
