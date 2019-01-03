using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.App;

namespace WebParser.Data
{
    public class Repository : IRepository
    {
        public float QueryFindImdbByFilmName(string filmName)
        {
            HtmlWeb web;
            HtmlDocument document;

            string webs = "https://en.wikipedia.org/wiki/Oberon_(moon)";
            web = new HtmlWeb();
            document = web.Load(webs);

            /*

            var document = new HtmlWeb().Load(url);
            var urls = document.DocumentNode.Descendants("img")
                                            .Select(e => e.GetAttributeValue("src", null))
                                            .Where(s => !String.IsNullOrEmpty(s));


            HtmlDocument doc = new HtmlDocument();
            List<string> image_links = new List<string>();
            doc.Load("file.htm");
            foreach (HtmlNode link in doc.DocumentElement.SelectNodes("//img"))
            {
                image_links.Add(link.GetAttributeValue("src", ""));
            }

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//img"))
            {
                imgList[i] = node.Attributes["src"].Value;
                i++;
            }

            foreach (HtmlNode node in nodes)
            {
                listBox1.Items.Add(node.Attributes["src"].Value);
            }

            var htmlDocument = new HtmlWeb().Load("URL of website you are targeting...");
            var imageNode = htmlDocument.DocumentNode.SelectSingleNode("XPath of image you are targeting...");
            string imagePath = imageNode.Attributes["src"].Value;

            var imageStream = HttpWebRequest.Create(imagePath).GetResponse().GetResponseStream();
            this.pictureBox1.Image = Image.FromStream(imageStream);

            //{
            //    if (imgg.Attributes["src"] == null)
            //        continue;
            HtmlAttribute src = imgs[0].Attributes["src"];

            imgList.Add(src.Value);
            //Do something with src.Value such as Get the image and save it to pictureBox
            Image img = GetImage(src.Value);
            pictureBox1.Image = img;
            //}
            return imgList;
        }

        private Image GetImage(string url)
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);

            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();

            Bitmap bmp = new Bitmap(responseStream);
            responseStream.Dispose();
            return bmp;
        } 
        */


            return 5.5F;
        }
    }
}


/*
 * 
 * text = document.DocumentNode.SelectSingleNode("//div[@data-qa='vacancies-total-found']")?.InnerText ?? "";
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

 * 
 * 
 * */
