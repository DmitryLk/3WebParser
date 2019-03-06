using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;



namespace WebParser.PresentierController
{
    public interface IPresentierView
    {
        IEnumerable<string> MovieList { set; }
        string ImdbFilmRating { set; }
        SpaceObjectImageResponseViewModel SpaceObjectImageResponse { set; }

    }
}
