using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WebParser.Domain;

namespace WebParser.App
{
    public interface IXLSRepository
    {
        Task<XLSColumnsToListDTO> QueryGetDataFromColumnUntilEmpty(XLSFileRequestDTO requestDTO);
        //Task<MovieInfoResponseDTO> QueryGetMovieData(XLSFileRequestDTO requestDTO);
    }
}
