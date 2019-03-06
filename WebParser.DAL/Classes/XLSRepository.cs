using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebParser.App;
using System.Windows.Media.Imaging;
using ClosedXML.Excel;



namespace WebParser.Data
{
    public class XLSRepository : IXLSRepository
    {
      
     

        public async Task<XLSColumnsToListDTO> QueryGetDataFromColumnUntilEmpty(XLSFileRequestDTO requestDTO)
        {
            var result = new List<List<string>>();
            string cellValue;
            List<string> rowValue;
            await Task.Run(() =>
            {
                using (var wb = new XLWorkbook(requestDTO.FileName))
                {
                    var ws = wb.Worksheet(requestDTO.ListName);
                    {
                        var row = ws.Row(requestDTO.TopRowNumber);
                        while (!row.Cell(requestDTO.ColumnsNumber[0]).IsEmpty())
                        {
                            rowValue = new List<string>();
                            foreach (var col in requestDTO.ColumnsNumber)
                            {
                                cellValue = row.Cell(col).GetString();
                                rowValue.Add(cellValue);
                            }
                            result.Add(rowValue);
                            row = row.RowBelow();
                        }
                    }
                }
            });

            await Task.Delay(10);
            return new XLSColumnsToListDTO { SearchCount = result.Count(), SearchResultsList = result };
        }
    }
}


//public async Task<MovieInfoResponseDTO> QueryGetMovieData(XLSFileRequestDTO requestDTO)
//{
//    var tempResults = await QueryGetDataFromColumnUntilEmpty(requestDTO);
//    var results = new List<MovieDTO>();

//    foreach (var res in tempResults.SearchResultsList)
//    {
//        results.Add(new MovieDTO { Name = res.ElementAtOrDefault(0), Year = res.ElementAtOrDefault(1) });
//    }

//    return new MovieInfoResponseDTO {SearchResultsList= results };

//}