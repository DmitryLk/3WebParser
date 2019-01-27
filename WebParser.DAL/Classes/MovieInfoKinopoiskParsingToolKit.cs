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
    class MovieInfoKinopoiskParsingToolKit
    {
        private readonly Uri _baseUrl = new Uri("https://www.kinopoisk.ru/");

      

      


        public async Task<HtmlDocument> GetTargetPage(HtmlDocument checkedDocument)
        {
            if (CheckPage_TargetPage(checkedDocument)) return checkedDocument;
            if (CheckPage_NotFoundPage(checkedDocument)) throw new Exception("По данному запросу ничего не найдено");


            var searchResultsPage = await GetSearchResultsPage(checkedDocument);

            var searchedNodes = await FoundAllSearchResultsNodes(searchResultsPage);

            if (searchedNodes!=null)
            foreach (var node in searchedNodes)
            {
                //var resultPage = await GetHtmlDocumentByUri(uri);
                //if (CheckPage_TargetPage(resultPage)) return resultPage;
            }
            throw new Exception("Не найдено страниц о космическом объекте в списке");
        }

        public async Task<HtmlDocument> GetSearchResultsPage(HtmlDocument checkedDocument)
        {
            if (CheckPage_SearchResultsPage(checkedDocument)) return checkedDocument;
            if (!CheckPage_LinkToSearchResultsPage(checkedDocument)) throw new Exception("На странице не нашлось перехода на страницу с поиском");

            var disambiguationLinkList = ExtractListUriByInnerTextKeywordsList(checkedDocument, new List<string> { "(disambiguation)" });
            return await GetHtmlDocumentByUri(disambiguationLinkList.FirstOrDefault());
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




        public Uri ExtractLinkFromNode(HtmlNode nodeWithHref)
        {
            Uri uri = null;
            if (nodeWithHref.Name == "a")
                uri = new Uri(_baseUrl, nodeWithHref.Attributes["href"].Value);

            return uri;
        }


        public HtmlNodeCollection FindNodesByTagAndInnerText(HtmlDocument checkedDocument, string textContains, string tag = "*")
        {
            var nodesFound = checkedDocument.DocumentNode.SelectNodes("//" + tag + "[text()[contains(., '" + textContains + "')]]");
            return nodesFound;
        }

        public Uri GetUriSpaceObjectImage(HtmlDocument checkedDocument)
        {
            string link = checkedDocument.DocumentNode.SelectSingleNode("//td[@colspan='2']")?.SelectSingleNode(".//a")?.SelectSingleNode(".//img")?.Attributes["src"]?.Value
                ?? throw new Exception("На странице не нашлось изображения космического тела");
            return new Uri(_baseUrl, link);
        }

        //---------------------------------------------------------------------------------------------


        public bool CheckPage_TargetPage(HtmlDocument checkedDocument)
        {
            int i = 0;
            var keywordsList = new List<string> { "Eccentricity", "Volume", "Mass", "Orbital", "Temperature", "Semi-major", "anomaly" };

            foreach (var keyword in keywordsList)
            {
                if (FindNodesByTagAndInnerText(checkedDocument, keyword) != null) i++;
            }
            return i >= 3;
        }


        public bool CheckPage_SearchResultsPage(HtmlDocument checkedDocument)
        {
            return FindNodesByTagAndInnerText(checkedDocument, "Скорее всего, вы ищете:") != null;
        }

        public bool CheckPage_LinkToSearchResultsPage(HtmlDocument checkedDocument)
        {
            return FindNodesByTagAndInnerText(checkedDocument, "(disambiguation)") != null;
        }

        public bool CheckPage_NotFoundPage(HtmlDocument checkedDocument)
        {
            return FindNodesByTagAndInnerText(checkedDocument, "К сожалению, по вашему запросу ничего не найдено...") != null;
        }


        //===================================================================================



        public int CountTagsOnPage(HtmlDocument document, string tag)
        {
            return document.DocumentNode.SelectNodes("//" + tag).Count;
        }

        public List<string> GetAllLinksFromDocument(HtmlDocument document)
        {
            List<string> links = new List<string>();
            string hrefValue;

            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]"))
            {
                hrefValue = link.Attributes["href"].Value;
                links.Add(hrefValue);
            }

            return links;
        }

        private Boolean CheckHtmlNodeWithAttributeValueExists(HtmlDocument checkedDocument, string attribute, string valueContains)
        {
            return checkedDocument.DocumentNode.SelectSingleNode("//a[contains(@" + attribute + ", '" + valueContains + "')]") != null;
        }

        private string GetAttributeValueContainsString(HtmlDocument checkedDocument, string attribute, string valueContains)
        {
            string link = checkedDocument.DocumentNode.SelectSingleNode("//a[contains(@" + attribute + ", '" + valueContains + "')]")?.Attributes["href"].Value;
            return link;
        }

        private string GetAttributeValueContainsInnerText(HtmlDocument checkedDocument, string attribute, string valueContains)
        {
            HtmlNode nodeFound = checkedDocument.DocumentNode.SelectSingleNode("//*[text()[contains(., '" + valueContains + "')]]");
            string link = nodeFound?.Attributes["href"].Value;
            return link;
        }


    }
}
