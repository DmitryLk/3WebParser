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
using WebParser.App;
using NLog;

namespace WebParser.Data
{
    class MovieInfoKinopoiskParsingToolKit : CommonParser
    {
        private readonly Logger _logger;

        public MovieInfoKinopoiskParsingToolKit(IHtmlByUriGetter htmlGetter, Logger logger) : 
            base(new Uri("https://www.kinopoisk.ru/"), htmlGetter, logger)
        {
            _logger = logger;

        }

        public async Task<HtmlDocument> GetTargetPage(HtmlDocument checkedDocument)
        {
            HtmlDocument searchResultsPage;
            if (IsTargetPage(checkedDocument)) return checkedDocument;
            else if (IsSearchResultsPage(checkedDocument)) searchResultsPage = checkedDocument;
            else if (IsLinkToSearchResultsPage(checkedDocument)) searchResultsPage = await GetSearchResultsPage(checkedDocument);
            else if (IsResultNotFoundPage(checkedDocument)) throw new Exception("По запросу ничего не найдено");
            else if (IsProtectionFromRobot(checkedDocument)) throw new Exception("Сработка защиты от роботов");
            else throw new Exception("Найдена незнакомая страница");

            var searchedNodes = await FoundAllSearchResultsNodes(searchResultsPage);

            string s1, s2;
            var ls = new List<string>();
            if (searchedNodes != null)
                foreach (var node in searchedNodes)
                {
                    s1 = node.SelectSingleNode(".//p[@class='name']")?.SelectSingleNode(".//a[@href]")?.InnerText ?? "";
                    s2 = node.SelectSingleNode(".//p[@class='name']")?.SelectSingleNode(".//span[@class='year']")?.InnerText ?? "";
                    ls.Add(s1 + " (" + s2 + ")");
                }
            throw new Exception("Не найдено страниц о космическом объекте в списке");
            await Task.Delay(10);
            return checkedDocument;
        }



     

        public async Task<IEnumerable<MovieDTO>> GetMovieList(HtmlDocument searchResultsPage)
        {
            string result;
            var movieList = new List<MovieDTO>();
            var searchedNodes = await FoundAllSearchResultsNodes(searchResultsPage) ?? throw new Exception("Пустой список поиска");

            string name, year;
            if (searchedNodes != null)
                foreach (var node in searchedNodes)
                {
                    name = node.SelectSingleNode(".//p[@class='name']")?.SelectSingleNode(".//a[@href]")?.InnerText ?? "";
                    year = node.SelectSingleNode(".//p[@class='name']")?.SelectSingleNode(".//span[@class='year']")?.InnerText ?? "";

                    result = $"{name} ({year})";


                    movieList.Add(new MovieDTO { Name = name, Year = year });
                }
            return movieList;
        }

        public async Task<IEnumerable<HtmlNode>> FoundAllSearchResultsNodes(HtmlDocument checkedDocument)
        {
            Uri uri;
            var htmlNodes = new List<HtmlNode>();

            var nodeMostWanted = checkedDocument.DocumentNode.SelectSingleNode("//div[@class='element most_wanted']") ?? throw new Exception("Не найден раздел \"Скорее всего, вы ищете:\"");
            htmlNodes.Add(nodeMostWanted);


            uri = ExtractLinksByInnerText(checkedDocument, new List<string> { "Похожие результаты" }).First();


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
                    var documentSearchPage = await GetHtmlDocumentByUriAsync(uri);
                    if (IsProtectionFromRobot(documentSearchPage)) throw new Exception("Сработка защиты от роботов");


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
            var keywordsList = new List<string> { "Рейтинг фильма", "Рейтинг кинокритиков", "Отзывы и рецензии зрителей", "Для того чтобы добавить рецензию", "режиссер", "композитор", "В главных ролях:" };

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

        public bool IsProtectionFromRobot(HtmlDocument checkedDocument)
        {
            return FindNodesByTagAndInnerText(checkedDocument, "Система защиты от роботов решила") != null;
        }
        


        //--------------------------------------------------------------------------------------

    }
}
