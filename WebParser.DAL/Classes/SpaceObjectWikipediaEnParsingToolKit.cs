using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebParser.Data
{
    class SpaceObjectWikipediaEnParsingToolKit : CommonParsingToolKit
    {

        public SpaceObjectWikipediaEnParsingToolKit(IHtmlByUriGetter htmlGetter) : base("https://en.wikipedia.org/", htmlGetter) {}


        public async Task<HtmlDocument> GetTargetPage(HtmlDocument checkedDocument)
        {
            HtmlDocument searchResultsPage;
            if (IsTargetPage(checkedDocument) == true) return checkedDocument;
            else if (IsSearchResultsPage(checkedDocument) == true) searchResultsPage = checkedDocument;
            else if (IsLinkToSearchResultsPage(checkedDocument)) searchResultsPage = await GetSearchResultsPage(checkedDocument);
            else if (IsResultNotFoundPage(checkedDocument)) throw new Exception("По запросу ничего не найдено");
            else throw new Exception("Найдена незнакомая страница");

            foreach (var uri in ExtractListUriByInnerTextKeywordsList(searchResultsPage, new List<string> { "moon", "planet", "asteroid", "comet" }))
            {
                var resultPage = await GetHtmlDocumentByUri(uri);
                if (IsTargetPage(resultPage)) return resultPage;
            }
            throw new Exception("Не найдено страниц о космическом объекте в списке поиска");
        }

     

        public Uri GetUriSpaceObjectImage(HtmlDocument checkedDocument)
        {
            string link = checkedDocument.DocumentNode.SelectSingleNode("//td[@colspan='2']")?.SelectSingleNode(".//a")?.SelectSingleNode(".//img")?.Attributes["src"]?.Value
                ?? throw new Exception("На странице не нашлось изображения космического тела");
            return new Uri(_baseUri, link);
        }

        //--------------------------------------------------------------------------------------

        public bool IsTargetPage(HtmlDocument checkedDocument)
        {
            int i = 0;
            var keywordsList = new List<string> { "Eccentricity", "Volume", "Mass", "Orbital", "Temperature", "Semi-major", "anomaly" };

            foreach (var keyword in keywordsList)
            {
                if (FindNodesByTagAndInnerText(checkedDocument, keyword) != null) i++;
            }
            return i >= 3;
        }

        public bool IsSearchResultsPage(HtmlDocument checkedDocument)
        {
            return FindNodesByTagAndInnerText(checkedDocument, "Disambiguation pages") != null;
        }

        public bool IsLinkToSearchResultsPage(HtmlDocument checkedDocument)
        {
            return FindNodesByTagAndInnerText(checkedDocument, "(disambiguation)") != null;
        }

        public bool IsResultNotFoundPage(HtmlDocument checkedDocument)
        {
            return FindNodesByTagAndInnerText(checkedDocument, "К сожалению, по вашему запросу ничего не найдено...") != null;
        }

        //--------------------------------------------------------------------------------------

    }
}


