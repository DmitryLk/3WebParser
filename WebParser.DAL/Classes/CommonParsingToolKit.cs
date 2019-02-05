using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class CommonParsingToolKit
    {
        protected readonly Uri _baseUri;
        protected readonly IHtmlByUriGetter _htmlGetter;


        public CommonParsingToolKit(string baseUri, IHtmlByUriGetter htmlGetter)
        {
            _baseUri = new Uri(baseUri);
            _htmlGetter = htmlGetter;
        }

        public async Task<HtmlDocument> GetHtmlDocumentByUri(Uri uri)
        {
            var searchPageUri = new Uri(_baseUri, uri);
            var document = await _htmlGetter.GetHtml(searchPageUri.ToString());
            return document;
        }


        public async Task<HtmlDocument> GetSearchResultsPage(HtmlDocument checkedDocument)
        {
            var disambiguationLinkList = ExtractListUriByInnerTextKeywordsList(checkedDocument, new List<string> { "(disambiguation)" });
            return await GetHtmlDocumentByUri(disambiguationLinkList.FirstOrDefault());
        }

        public IEnumerable<Uri> ExtractListUriByInnerTextKeywordsList(HtmlDocument checkedDocument, List<string> keywordsList)
        {
            Uri uri;
            foreach (var keyword in keywordsList)
            {
                var nodes = FindNodesByTagAndInnerText(checkedDocument, keyword);
                if (nodes != null)
                    foreach (var node in nodes)
                    {
                        foreach (var nNode in node.DescendantsAndSelf())
                        {
                            uri = ExtractLinkFromNode(nNode);
                            if (uri != null) yield return uri;
                        }
                    }
            }
        }

        public Uri ExtractLinkFromNode(HtmlNode nodeWithHref)
        {
            Uri uri = null;
            if (nodeWithHref.Name == "a")
                uri = new Uri(_baseUri, nodeWithHref.Attributes["href"].Value);

            return uri;
        }


        public HtmlNodeCollection FindNodesByTagAndInnerText(HtmlDocument checkedDocument, string textContains, string tag = "*")
        {
            var nodesFound = checkedDocument.DocumentNode.SelectNodes("//" + tag + "[text()[contains(., '" + textContains + "')]]");
            return nodesFound;
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


//private ChromiumWebBrowser browser;
//static void browser_LoadError(object sender, LoadErrorEventArgs e)
//{
//    if (e != null)
//        Console.WriteLine("Retrieving error: {0}", e);
//    else
//        Console.WriteLine("Retrieving unknown error");
//}
//private void WebBrowserFrameLoadEnded(object sender, FrameLoadEndEventArgs e)
//{
//    if (e.Frame.IsMain)
//    {
//        browser.ViewSource();
//        browser.GetSourceAsync().ContinueWith(taskHtml =>
//        {
//            var html = taskHtml.Result;
//        });
//    }
//}

//private readonly ChromiumWebBrowser browser = new ChromiumWebBrowser(string.Empty)

//public WebRepository(ChromiumWebBrowser browser)
//{
//    this.browser = browser ?? throw new ArgumentNullException(nameof(browser));
//}


//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//using (browser = new ChromiumWebBrowser("https://www.google.com")
//{
//    browser.FrameLoadEnd += WebBrowserFrameLoadEnded;
//    browser.LoadError += browser_LoadError;
//    Console.WriteLine("Wait for output and then press any key...");
//    Console.ReadKey();
//}
//browser = new ChromiumWebBrowser();
//Cef.Initialize(settings);
//browser.FrameLoadEnd += BrowserFrameLoadEnd;
//var browser = new ChromiumWebBrowser(string.Empty) { Dock = DockStyle.Fill };
//settings.CefCommandLineArgs.Add("proxy-server", "37.75.9.131:8080");
//CefSettings settings = new CefSettings();
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

//nodes = documentSearchPage.DocumentNode.SelectNodes("//*[.='" + tag + "']");
//nodes = documentSearchPage.DocumentNode.SelectNodes("//*[text()='»']");
//nodes = documentSearchPage.DocumentNode.SelectNodes("//*[.='»']");



//List<string> links = new List<string>();
//string hrefValue;

//    foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]"))
//    {
//        hrefValue = link.Attributes["href"].Value;
//        links.Add(hrefValue);
//    }

//    return links;

//public IEnumerable<Uri> ExtractListUriByInnerTextKeywordsList(HtmlDocument checkedDocument, List<string> keywordsList)
//{
//    foreach (var keyword in keywordsList)
//    {
//        var nodes = FindNodesByTagAndInnerText(checkedDocument, keyword, "a");
//        if (nodes != null)
//            foreach (var node in nodes)
//            {
//                yield return ExtractLinkFromNode(node); 
//            }
//    }
//}

/*

//*[text()[contains(., '" + valueContains + "')]]
//checkedDocument.DocumentNode.SelectSingleNode("//a[contains(@" + attribute + ", '" + valueContains + "')]").OuterHtml

 
text = document.DocumentNode.SelectSingleNode("//div[@data-qa='vacancies-total-found']")?.InnerText ?? "";
text = Regex.Match(text, @"\d+").Value;


web = new HtmlWeb();
document = web.Load(webs);
nodes = document.DocumentNode.SelectNodes("//div[@class='vacancy-serp-item ' or @class='vacancy-serp-item  vacancy-serp-item_premium']");
while (nodes != null)
{
foreach (HtmlNode node in nodes)
{
    rec = new Record();
    rec.Name = node.SelectSingleNode(".//div[@class='search-item-name']")?.InnerText ?? "";
    string link = node.SelectSingleNode(".//a[@class='bloko-link HH-LinkModifier']")?.Attributes["href"].Value;
    rec.Zp = node.SelectSingleNode(".//div[@class='vacancy-serp-item__compensation']")?.InnerText ?? "";
    rec.Comp = node.SelectSingleNode(".//div[@class='vacancy-serp-item__meta-info']")?.InnerText ?? "";
    rec.Town = node.SelectSingleNode(".//span[@class='vacancy-serp-item__meta-info']")?.InnerText ?? "";
    rec.Resp1 = node.SelectSingleNode(".//div[@data-qa='vacancy-serp__vacancy_snippet_responsibility']")?.InnerText ?? "";
    rec.Req1 = node.SelectSingleNode(".//div[@data-qa='vacancy-serp__vacancy_snippet_requirement']")?.InnerText ?? "";
    rec.Dat = node.SelectSingleNode(".//span[@class='vacancy-serp-item__publication-date']")?.InnerText ?? "";

    text = node.SelectSingleNode(".//script[@data-name='HH/VacancyResponsePopup/VacancyResponsePopup']")?.Attributes["data-params"]?.Value ?? "0";

    rec.Id = Regex.Match(text, @"\d+").Value;



    document2 = web.Load(link);
    rec.Opt = document2.DocumentNode.SelectSingleNode("(//div[@class='bloko-gap bloko-gap_bottom'])[3]")?.InnerText ?? "";
    root = document2.DocumentNode.SelectSingleNode("//div[@class='g-user-content' or @data-qa='vacancy-description']");

    foreach (HtmlNode node2 in root.DescendantsAndSelf())
    {
        if (!node2.HasChildNodes)
        {
            text = node2.InnerText;
            if (!string.IsNullOrWhiteSpace(text))
                rec.Desc.AppendLine(text);
        }
    }
        //string text = document.DocumentNode.SelectSingleNode("//div[@data-qa='vacancies-total-found']")?.InnerText ?? "";
        //var text = document.DocumentNode.SelectSingleNode("//a[contains(@href, '(disambiguation)')]");


        //*[text()[contains(., '" + valueContains + "')]]
    //string link = nodesFound?.Attributes["href"].Value ?? "";
    //checkedDocument.DocumentNode.SelectSingleNode("//a[contains(@" + attribute + ", '" + valueContains + "')]").OuterHtml
    //checkedDocument.DocumentNode.SelectSingleNode("//a[contains(@" + attribute + ", '" + valueContains + "')]").OuterHtml

* 
* 
* */
