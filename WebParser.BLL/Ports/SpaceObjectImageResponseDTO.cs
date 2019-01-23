using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace WebParser.App
{
    public class SpaceObjectImageResponseDTO
    {
        //public Uri PageSpaceObjectUri { get; set; }
        public Uri SpaceObjectImageUri { get; set; }
        public string SpaceObjectName { get; set; }
        public BitmapImage SpaceObjectImage { get; set; }
    }
}
