using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebParser.Data
{
    public interface IHtmlByUriGetter
    {
        Task<HtmlDocument> GetHtml(string uri);
    }
}