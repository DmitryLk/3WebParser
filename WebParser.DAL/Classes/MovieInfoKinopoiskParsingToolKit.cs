using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.OffScreen;
using System.Threading;
using CefSharp.Internals;

namespace WebParser.Data
{
    class MovieInfoKinopoiskParsingToolKit : CommonParsingToolKit
    {
        

        public MovieInfoKinopoiskParsingToolKit(IHtmlByUriGetter htmlGetter) : base("https://www.kinopoisk.ru/", htmlGetter) {}

        public async Task<HtmlDocument> GetTargetPage(HtmlDocument checkedDocument)
        {
            HtmlDocument searchResultsPage;
            if (IsTargetPage(checkedDocument) == true) return checkedDocument;
            else if (IsSearchResultsPage(checkedDocument) == true) searchResultsPage = checkedDocument;
            else if (IsLinkToSearchResultsPage(checkedDocument)) searchResultsPage = await GetSearchResultsPage(checkedDocument);
            else if (IsResultNotFoundPage(checkedDocument)) throw new Exception("По запросу ничего не найдено"); 
            else throw new Exception("Найдена незнакомая страница");

            var searchedNodes = await FoundAllSearchResultsNodes(searchResultsPage);

            if (searchedNodes!=null)
            foreach (var node in searchedNodes)
            {
                //var resultPage = await GetHtmlDocumentByUri(uri);
                //if (CheckPage_TargetPage(resultPage)) return resultPage;
            }
            throw new Exception("Не найдено страниц о космическом объекте в списке");
        }



        public async Task<IEnumerable<HtmlNode>> FoundAllSearchResultsNodes(HtmlDocument checkedDocument)
        {
            Uri uri;
            var htmlNodes = new List<HtmlNode>();

            var nodeMostWanted = checkedDocument.DocumentNode.SelectSingleNode("//div[@class='element most_wanted']") ?? throw new Exception("Не найден раздел \"Скорее всего, вы ищете:\"");
            htmlNodes.Add(nodeMostWanted);


            uri = ExtractListUriByInnerTextKeywordsList(checkedDocument, new List<string> { "Похожие результаты" }).First();


            if (uri == null)    // раздела Похожие результаты нет - надо найти ноды element на этой же странице
            {
                var nodes = checkedDocument.DocumentNode.SelectNodes("//div[@class='element']");
                foreach (var node in nodes)
                {
                    htmlNodes.Add(node);
                }
            }
            else // раздел Похожие результаты есть
            {
                do
                {
                    var documentSearchPage = await GetHtmlDocumentByUri(uri);

                    var nodes = documentSearchPage.DocumentNode.SelectNodes("//div[contains(@class,'element')]") ?? throw new Exception("Не найдены элементы в разделе \"Похожие результаты\"");

                    foreach (var node in nodes)
                    {
                        htmlNodes.Add(node);
                    }

                    nodes = documentSearchPage.DocumentNode.SelectNodes("//a[text()='&raquo;' or text()='»']");
                    if (nodes == null) break;
                    uri = ExtractLinkFromNode(nodes.First());
                }
                while (true);


            }
            return htmlNodes;
        }

        //---------------------------------------------------------------------------------------------


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
            return FindNodesByTagAndInnerText(checkedDocument, "Скорее всего, вы ищете:") != null;
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
