using System;
using System.Collections.Generic;
using System.Text;
using ExploreFlicker.Models.Response;

namespace ExploreFlicker.Models
{
    public class GalleryNavigationParameters
    {
        public Photo SelectedPhoto { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
