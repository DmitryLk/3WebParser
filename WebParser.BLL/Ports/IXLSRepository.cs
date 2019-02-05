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
        //Task<XLSColumnToListDTO> QueryGetDataFromColumnUntilEmpty(string fileName, string listName, int columnNumber, int topRowNumber);
        Task<MovieInfoResponseDTO> QueryGetMovieData(XLSFileRequestDTO requestDTO);


    }
}
