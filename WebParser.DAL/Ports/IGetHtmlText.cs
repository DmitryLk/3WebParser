using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.Data
{
    interface IGetHtmlText
    {
        string GetHtml(string uri);
    }
}
