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
        
        string ImdbFilmRating { set; }
        SpaceObjectImageResponseModelView SpaceObjectImageResponse { set; }

    }
}
