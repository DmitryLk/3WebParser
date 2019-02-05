using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;



namespace WebParser.App
{
    public interface IWebRepository
    {
        Task<MovieInfoResponseDTO> QueryFindImdbByFilmName(string filmName);
        
        Task<SpaceObjectImageResponseDTO> QueryFindSpaceObjectImageByName(string spaceObjectName);
    }
}
