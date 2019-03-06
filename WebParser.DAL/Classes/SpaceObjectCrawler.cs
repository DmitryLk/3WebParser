using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WebParser.Data
{

    public class SpaceObjectCrawler
    {
        private readonly IParser _parser;
        private readonly Logger _logger;

        public SpaceObjectCrawler(IParser parser, Logger logger)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

     

        public async Task<BitmapImage> GetBitmapImageByName(string spaceObjectName)
        {
            //var targetPage = await GetSpaceObjectPage(spaceObjectName);
            BitmapImage spaceObjectBitmapImage = null;

            var initialPage = await GetInitialPage(spaceObjectName);
            var targetPage = await SearchTargetPageFromThisPage(initialPage);

            if (targetPage != null)
            {
                var spaceObjectBitmapImageLink = GetSpaceObjectImageLink(targetPage); // ?? _logger.Debug(spaceObjectName);

                if (spaceObjectBitmapImageLink!=null)
                    spaceObjectBitmapImage = await _parser.GetBitmapImageByUriAsync2(spaceObjectBitmapImageLink);
            }
            return spaceObjectBitmapImage;
        }

        //public bool IsTargetPage
        //public bool IsDisambugationPage
        //public bool IsLinkToDisambugationPage
        //public bool IsLinkToTargetPage
        //public bool IsResultNotFoundPage


        public async Task<SpaceObjectDocument> SearchTargetPageFromThisPage(SpaceObjectDocument checkedDocument)
        {
            SpaceObjectDocument resultDocument = null;
            if (checkedDocument.IsTargetPage) return checkedDocument;

            if (checkedDocument.IsResultNotFoundPage) return null;

            if (checkedDocument.IsDisambugationPage)
            {
                var uriList = _parser.ExtractListUriByInnerTextKeywordsList(checkedDocument.Document, new List<string> { "moon", "planet", "asteroid", "comet" });
                foreach (var uri in uriList)
                {
                    var nextDocument = new SpaceObjectDocument(_parser);
                    nextDocument.Document= await _parser.GetHtmlDocumentByUriAsync(uri);
                    resultDocument = await SearchTargetPageFromThisPage(nextDocument);
                    if (resultDocument != null) return resultDocument;
                }
            }

            if (checkedDocument.IsLinkToDisambugationPage)
            {
                var uriList = _parser.ExtractListUriByInnerTextKeywordsList(checkedDocument.Document, new List<string> { "(disambiguation)" });
                foreach (var uri in uriList)
                {
                    var nextDocument = new SpaceObjectDocument(_parser);
                    nextDocument.Document = await _parser.GetHtmlDocumentByUriAsync(uri);
                    resultDocument = await SearchTargetPageFromThisPage(nextDocument);
                    if (resultDocument != null) return resultDocument;
                }
            }

            if (checkedDocument.IsLinkToTargetPage)
            {
                var uriList = _parser.ExtractListUriByInnerTextKeywordsList(checkedDocument.Document, new List<string> { "moon", "planet", "asteroid", "comet" });
                foreach (var uri in uriList)
                {
                    var nextDocument = new SpaceObjectDocument(_parser);
                    nextDocument.Document = await _parser.GetHtmlDocumentByUriAsync(uri);
                    resultDocument = await SearchTargetPageFromThisPage(nextDocument);
                    if (resultDocument != null) return resultDocument;
                }
            }
            return resultDocument;
        }

        public async Task<int> MySum(int a, int b)
        {
            await Task.Delay(10);
            return a + b;
        }

        public async Task<SpaceObjectDocument> GetInitialPage(string spaceObjectName)
        {
            var uri = new Uri("/w/index.php?search=" + spaceObjectName + "&title=Special%3ASearch&go=Go", UriKind.Relative);
            var document = await _parser.GetHtmlDocumentByUriAsync(uri);
            return new SpaceObjectDocument(_parser, document);
        }

        //=============================================================================================================================================
      
        public string GetSpaceObjectImageLink(SpaceObjectDocument document)
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

