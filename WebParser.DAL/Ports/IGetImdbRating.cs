using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.Data
{
    interface IGetImdbRating
    {
        double GetImdbRating(string filmName);
    }
}
