using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.App
{
    public class MovieInfoResponseDTO
    {
        public int SearchCount { get; set; }
        public IEnumerable<MovieDTO> SearchResultsList { get; set; }
        //public float ImdbRating { get; set; }
    }
}
