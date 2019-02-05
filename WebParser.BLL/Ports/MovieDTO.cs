using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.App
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Country { get; set; }


        public float Imdb { get; set; }
    }
}
