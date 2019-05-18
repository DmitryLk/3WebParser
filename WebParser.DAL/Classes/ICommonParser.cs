using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using HtmlAgilityPack;

namespace WebParser.Data
{
    public interface IParser
    {
        IEnumerable<Uri> ExtractLinksByInnerText(HtmlDocument checkedDocument, List<string> keywordsList);
        HtmlNodeCollection FindNodesByTagAndInnerText(HtmlDocument checkedDocument, string textContains, string tag = "*");
        Task<BitmapImage> GetBitmapImageByUriAsync1(string link);
        Task<BitmapImage> GetBitmapImageByUriAsync2(string link);
        Task<HtmlDocument> GetHtmlDocumentByUriAsync(Uri uri);
        Task<HtmlDocument> GetSearchResultsPage(HtmlDocument checkedDocument);
    }
}