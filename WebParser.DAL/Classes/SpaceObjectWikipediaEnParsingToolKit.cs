using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebParser.Data
{
    class SpaceObjectWikipediaEnParsingToolKit
    {
        private readonly Uri _baseUrl = new Uri("https://en.wikipedia.org/");

        public async Task<HtmlDocument> GetHtmlDocumentByUri(Uri uri)
        {
            HtmlWeb web;
            HtmlDocument document;
            web = new HtmlWeb();

            var searchSpaceObjectPageUri = new Uri(_baseUrl, uri);

            document = await web.LoadFromWebAsync(searchSpaceObjectPageUri.ToString());
            return document;
        }

        public async Task<HtmlDocument> GetSpaceObjectPage(HtmlDocument checkedDocument)
        {
            if (CheckPage_SpaceObjectImagePage(checkedDocument)) return checkedDocument;

            var disambiguationPage = await GetDisambiguationPage(checkedDocument);

            foreach (var uri in ExtractListUriByInnerTextKeywordsList(disambiguationPage, new List<string> { "moon", "planet", "asteroid" }))
            {
                var resultPage = await GetHtmlDocumentByUri(uri);
                if (CheckPage_SpaceObjectImagePage(resultPage)) return resultPage;
            }
            throw new Exception("Не найдено страниц о космическом объекте в списке");
        }

        public async Task<HtmlDocument> GetDisambiguationPage(HtmlDocument checkedDocument)
        {
            if (CheckPage_DisambiguationPage(checkedDocument)) return checkedDocument;
            if (!CheckPage_LinkToDisambiguationPage(checkedDocument)) throw new Exception("На странице не нашлось перехода на страницу со списком");

            var disambiguationLinkList = ExtractListUriByInnerTextKeywordsList(checkedDocument, new List<string> { "(disambiguation)" });
            return await GetHtmlDocumentByUri(disambiguationLinkList.FirstOrDefault());
        }

        public IEnumerable<Uri> ExtractListUriByInnerTextKeywordsList(HtmlDocument checkedDocument, List<string> keywordsList)
        {
            foreach (var keyword in keywordsList)
            {
                var nodes = FindNodesByTagAndInnerText(checkedDocument, keyword);
                if (nodes != null)
                    foreach (var node in nodes)
                    {
                        foreach (var nNode in node.DescendantsAndSelf())
                        {
                            var uri = ExtractLinkFromNode(nNode);
                            if (uri != null)  yield return uri; 
                        }
                    }
            }
        }

        public HtmlNodeCollection FindNodesByTagAndInnerText(HtmlDocument checkedDocument, string textContains, string tag = "*")
        {
            var nodesFound = checkedDocument.DocumentNode.SelectNodes("//" + tag + "[text()[contains(., '" + textContains + "')]]");
            return nodesFound;
        }

        public Uri ExtractLinkFromNode(HtmlNode nodeWithHref)
        {
            Uri uri = null;
            if (nodeWithHref.Name == "a")
                uri = new Uri(_baseUrl, nodeWithHref.Attributes["href"].Value);

            return uri;
        }


        public bool CheckPage_SpaceObjectImagePage(HtmlDocument checkedDocument)
        {
            int i = 0;
            var keywordsList = new List<string> { "Eccentricity", "Volume", "Mass", "Orbital", "Temperature", "Semi-major", "anomaly" };

            foreach (var keyword in keywordsList)
            {
                if (FindNodesByTagAndInnerText(checkedDocument, keyword) != null) i++;
            }
            return i >= 3;
        }

        public bool CheckPage_DisambiguationPage(HtmlDocument checkedDocument)
        {
            return FindNodesByTagAndInnerText(checkedDocument, "Disambiguation pages") != null;
        }

        public bool CheckPage_LinkToDisambiguationPage(HtmlDocument checkedDocument)
        {
            return FindNodesByTagAndInnerText(checkedDocument, "(disambiguation)") != null;
        }

        public Uri GetUriSpaceObjectImage(HtmlDocument checkedDocument)
        {
            string link = checkedDocument.DocumentNode.SelectSingleNode("//td[@colspan='2']")?.SelectSingleNode(".//a")?.SelectSingleNode(".//img")?.Attributes["src"]?.Value 
                ?? throw new Exception("На странице не нашлось изображения космического тела");
            return new Uri(_baseUrl, link);
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




/*
* 




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
