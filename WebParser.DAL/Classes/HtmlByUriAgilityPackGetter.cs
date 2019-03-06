using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace WebParser.Data
{
    public class HtmlByUriAgilityPackGetter : IHtmlByUriGetter
    {
        public async Task<HtmlDocument> GetHtml(string uri)
        {
            HtmlWeb web;
            HtmlDocument document;
            web = new HtmlWeb();
            document = await web.LoadFromWebAsync(uri);
            return document;
        }
    }


}
