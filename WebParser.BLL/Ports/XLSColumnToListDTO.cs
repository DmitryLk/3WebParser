using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.App
{
    public class XLSColumnsToListDTO
    {
        public int SearchCount { get; set; }
        public IEnumerable<IEnumerable<string>> SearchResultsList { get; set; }
        
    }
}
