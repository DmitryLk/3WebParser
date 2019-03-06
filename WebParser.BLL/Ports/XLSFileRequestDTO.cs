using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebParser.App
{
    public class XLSFileRequestDTO
    {
        //[Required (ErrorMessage = "Film name {0} is required")]
        //[StringLength(30, MinimumLength = 1, ErrorMessage = "Film name lenght should be between 1 and 30")]
        //public string FilmName { get; set; }

        public string FileName { get; set; }
        public string ListName { get; set; }
        public int[] ColumnsNumber { get; set; }
        public int TopRowNumber { get; set; }
        public string TargetFolder { get; set; }




    }
}
