using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WebParser.App;

namespace WebParser.Data
{
  


    public class SpaceObjectCrawlerWikiEn
    {
        private readonly IParser _parser;
        private readonly Logger _logger;
        private readonly Uri _baseUri;
        private List<Uri> _history;

        public SpaceObjectCrawlerWikiEn(Logger logger)
        {
            _baseUri = new Uri("https://en.wikipedia.org/");
            _parser = new CommonParser(_baseUri, new HtmlByUriAgilityPackGetter(), _logger);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<HtmlUriDocument> GetInitialPage(string spaceObjectName)
        {
            var uri = new Uri(_baseUri, $"/w/index.php?search={spaceObjectName}&title=Special%3ASearch&go=Go");
            var document = new HtmlUriDocument { Uri = uri, Document = await _parser.GetHtmlDocumentByUriAsync(uri) };
            return document;
        }


        public async Task<SpaceObjectImageResponseDTO> GetBitmapImageByName(string spaceObjectName)
        {
            //var targetPage = await GetSpaceObjectPage(spaceObjectName);
            BitmapImage spaceObjectBitmapImage = null;

     
            var initialPage = await GetInitialPage(spaceObjectName);
            _history = new List<Uri>();
            var searchedPage = await SearchTargetPageFromThisPage(initialPage);

            if (searchedPage != null)
            {
                var spaceObjectBitmapImageLink = GetSpaceObjectImageLink(searchedPage); // ?? _logger.Debug(spaceObjectName);

                if (spaceObjectBitmapImageLink!=null)
                    spaceObjectBitmapImage = await _parser.GetBitmapImageByUriAsync2(spaceObjectBitmapImageLink);
            }
            return new SpaceObjectImageResponseDTO { SpaceObjectImageUri = searchedPage?.Uri, SpaceObjectName = spaceObjectName, SpaceObjectImage = spaceObjectBitmapImage };
        }




        //возвращает Null если целевая страница не найдена
        public async Task<HtmlUriDocument> SearchTargetPageFromThisPage(HtmlUriDocument checkedDocument)
        {
            // документ уже проверялся? - выход
            if (_history.Any(u => u == checkedDocument.Uri))
                return null;
            _history.Add(checkedDocument.Uri);


            HtmlUriDocument resultDocument;
            var pageType = GetPageType(checkedDocument);


            _logger.Debug($"{checkedDocument.Uri} - {pageType.ToString()}");
            IEnumerable<Uri> uriList;
            List<string> keywords=null;

            switch (pageType)
            {

                case PageTypes.TargetPage: return checkedDocument;
                case PageTypes.ResultNotFoundPage: return null;
                case PageTypes.UnknownPage: return null;
                case PageTypes.AnotherDomen: return null;

                case PageTypes.DisambugationPage: keywords = new List<string> { "moon", "planet", "asteroid", "comet" }; break;
                case PageTypes.LinkToDisambugationPage: keywords = new List<string> { "(disambiguation)" }; break;
                case PageTypes.LinkToSpacePage: keywords= new List<string> { "moon", "planet", "asteroid", "comet" }; break;
            }

            uriList = _parser.ExtractLinksByInnerText(checkedDocument.Document, keywords);
            foreach (var uri in uriList)
            {
                var nextDocument = new HtmlUriDocument { Uri = uri, Document = await _parser.GetHtmlDocumentByUriAsync(uri) };
                resultDocument = await SearchTargetPageFromThisPage(nextDocument);
                if (resultDocument != null) return resultDocument;
            }

            return null;

        }

        public async Task<int> MySum(int a, int b)
        {
            await Task.Delay(10);
            return a + b;
        }


        public PageTypes GetPageType(HtmlUriDocument checkedDocument)
        {
            //IsNotIdentifiedPage = false;

            int i = 0;
            var keywordsList = new List<string> { "Eccentricity", "Volume", "Mass", "Orbital", "Temperature", "Semi-major", "anomaly" };
            foreach (var keyword in keywordsList)
            {
                if (_parser.FindNodesByTagAndInnerText(checkedDocument.Document, keyword) != null) i++;
            }
            if (i >= 3) return PageTypes.TargetPage;

            if (checkedDocument.Document.DocumentNode.SelectSingleNode("//div[@class='catlinks']")?.SelectSingleNode(".//ul")?.
                SelectSingleNode(".//li")?.SelectSingleNode("//a[text()[contains(., 'Astronomical objects')]]") !=null) return PageTypes.TargetPage;

            //var temp = checkedDocument.Document.DocumentNode.SelectSingleNode("//p[@class='mw-search-createlink']")?.
            //    SelectSingleNode(".//a[@class='new']")?.Attributes["title"].Value;
            //if (temp!=null && temp.Contains("(page does not exist)")) return PageTypes.IsPageDoesNotExist;

            if (checkedDocument.Uri.GetLeftPart(System.UriPartial.Authority) != _baseUri.GetLeftPart(System.UriPartial.Authority)) return PageTypes.AnotherDomen;

            if (_parser.FindNodesByTagAndInnerText(checkedDocument.Document, "Disambiguation pages") != null) return PageTypes.DisambugationPage;

            if (_parser.FindNodesByTagAndInnerText(checkedDocument.Document, "(disambiguation)") != null) return PageTypes.LinkToDisambugationPage;


            if (_parser.FindNodesByTagAndInnerText(checkedDocument.Document, "(moon)") != null) return PageTypes.LinkToSpacePage;
            if (_parser.FindNodesByTagAndInnerText(checkedDocument.Document, "(planet)") != null) return PageTypes.LinkToSpacePage;
            if (_parser.FindNodesByTagAndInnerText(checkedDocument.Document, "(asteroid)") != null) return PageTypes.LinkToSpacePage;
            if (_parser.FindNodesByTagAndInnerText(checkedDocument.Document, "(comet)") != null) return PageTypes.LinkToSpacePage;

            if (_parser.FindNodesByTagAndInnerText(checkedDocument.Document, "К сожалению, по вашему запросу ничего не найдено...") != null) return PageTypes.ResultNotFoundPage;

            return PageTypes.UnknownPage;
        }

        //=============================================================================================================================================

        public string GetSpaceObjectImageLink(HtmlUriDocument document)
        {
            return document.Document.DocumentNode.SelectSingleNode("//td[@colspan='2']")?.SelectSingleNode(".//a")?.SelectSingleNode(".//img")?.Attributes["src"]?.Value; // ?? throw new Exception("На странице не нашлось изображения космического тела");
        }
    }
}

// 1. По поисковому запросу пользователя через поиск на сайте находится первая страница
// 2. Определяется тип страницы (список вариантов найденного, целевая страница, ничего не найдено(по запросу ничего не найдено))
// 3. Пользователю выводится список подходящих вариантов под его запрос
// x. Из найденной страницы извлекается целевая информация


//spaceObjectImageUri)

//var targetPage = await crawler.GetTargetPage(firstFoundPage);
//var spaceObjectImageUri = crawler.GetUriSpaceObjectImage(targetPage);
//public async Task<HtmlDocument> GetInitialPage(string spaceObjectName)
//{
//    var checkedDocument = new SpaceObjectHtmlDocument { document = firstFoundDocument };
//    var document = await crawler.GetHtmlDocumentByUri(new Uri("/w/index.php?search=" + spaceObjectName + "&title=Special%3ASearch&go=Go", UriKind.Relative));

