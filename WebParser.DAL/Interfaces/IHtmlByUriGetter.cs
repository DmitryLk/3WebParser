using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebParser.Data
{
    interface IHtmlByUriGetter
    {
        Task<HtmlDocument> GetHtml(string uri);
    }
}