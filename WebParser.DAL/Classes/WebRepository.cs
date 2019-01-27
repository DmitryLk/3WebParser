using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebParser.App;
using System.Windows.Media.Imaging;



namespace WebParser.Data
{
    public class WebRepository : IRepository
    {
        //private readonly ChromiumWebBrowser browser = new ChromiumWebBrowser(string.Empty)
        
        //public WebRepository(ChromiumWebBrowser browser)
        //{
        //    this.browser = browser ?? throw new ArgumentNullException(nameof(browser));
        //}

        public async Task<SpaceObjectImageResponseDTO> QueryFindSpaceObjectImageByName(string spaceObjectName)
        {
            var parsingToolKit = new SpaceObjectWikipediaEnParsingToolKit();
            // 1. По поисковому запросу пользователя через поиск на сайте находится первая страница
            // 2. Определяется тип страницы (список вариантов найденного, целевая страница, ничего не найдено(по запросу ничего не найдено))
            // 3. Пользователю выводится список подходящих вариантов под его запрос
            // x. Из найденной страницы извлекается целевая информация

            var firstFoundPage = await parsingToolKit.GetHtmlDocumentByUri(new Uri("/w/index.php?search=" + spaceObjectName + "&title=Special%3ASearch&go=Go", UriKind.Relative));

            var targetPage = await parsingToolKit.GetTargetPage(firstFoundPage);
            
            var spaceObjectImageUri = parsingToolKit.GetUriSpaceObjectImage(targetPage);
            var spaceObjectBitmapImage = new BitmapImage(spaceObjectImageUri) ?? throw new Exception("На удалось прочитать BitmapImage");

            return new SpaceObjectImageResponseDTO
            { SpaceObjectImageUri = spaceObjectImageUri, SpaceObjectName = spaceObjectName, SpaceObjectImage = spaceObjectBitmapImage };
            
        }

        public async Task<MovieInfoResponseDTO> QueryFindImdbByFilmName(string filmName)
        {
            var parsingToolKit = new MovieInfoKinopoiskParsingToolKit();
          


            var firstFoundPage = await parsingToolKit.GetHtmlDocumentByUri(new Uri("/index.php?kp_query=" + filmName + "&what=", UriKind.Relative));

            var targetPage = await parsingToolKit.GetTargetPage(firstFoundPage);

            var spaceObjectImageUri = parsingToolKit.GetUriSpaceObjectImage(targetPage);
            var spaceObjectBitmapImage = new BitmapImage(spaceObjectImageUri) ?? throw new Exception("На удалось прочитать BitmapImage");

            //return new MovieInfoResponseDTO
            //{ SpaceObjectImageUri = spaceObjectImageUri, SpaceObjectName = spaceObjectName, SpaceObjectImage = spaceObjectBitmapImage };

            await Task.Delay(50);
            MovieInfoResponseDTO v = new MovieInfoResponseDTO();
            return v;

        }

    }
}

/*

            //HtmlWeb web;
            //HtmlDocument document;
            //string webs = "https://en.wikipedia.org/wiki/Oberon_(moon)";
            //web = new HtmlWeb();
            //document = web.Load(webs);


            //BitmapImage image = new BitmapImage();
            //image.BeginInit();
            //image.UriSource = new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/0/09/Voyager_2_picture_of_Oberon.jpg/220px-Voyager_2_picture_of_Oberon.jpg");
            //image.EndInit();
            //imgTestImage.Source = image;
            //return GetBitmapImage("https://upload.wikimedia.org/wikipedia/commons/thumb/0/09/Voyager_2_picture_of_Oberon.jpg/220px-Voyager_2_picture_of_Oberon.jpg");



        private BitmapImage GetBitmapImage(string url)
        {
            BitmapImage bitmap;
            using (WebClient webClient = new WebClient())
            {
                using (Stream stream = webClient.OpenRead(url))
                {
                    bitmap = new BitmapImage(stream);
                    stream.Close();
                }
            }
            return bitmap;
        }


    
var webClient = new WebClient();
byte[] imageBytes = webClient.DownloadData(new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/0/09/Voyager_2_picture_of_Oberon.jpg/220px-Voyager_2_picture_of_Oberon.jpg"));
MemoryStream stream = new MemoryStream(imageBytes);
Image img = Image.FromStream(stream);
stream.Close();



private Bitmap GetImage(string url)
{
    WebRequest request = WebRequest.Create(url);
    WebResponse response = request.GetResponse();
    Stream responseStream = response.GetResponseStream();
    Bitmap bmp = new Bitmap(responseStream);
    responseStream.Dispose();
    return bmp;
}



//System.Net.WebRequest request = System.Net.WebRequest.Create(url);

//System.Net.WebResponse response = request.GetResponse();
//System.IO.Stream responseStream = response.GetResponseStream();

//using (var stream = new MemoryStream(data))
//{
//    var bitmap = new BitmapImage();
//    bitmap.BeginInit();
//    bitmap.StreamSource = stream;
//    bitmap.CacheOption = BitmapCacheOption.OnLoad;
//    bitmap.EndInit();
//    bitmap.Freeze();
//}


//using (WebClient wc = new WebClient())
//{
//    using (Stream s = wc.OpenRead("http://hell.com/leaders/cthulhu.jpg"))
//    {
//        using (Bitmap bmp = new Bitmap(s))
//        {
//            bmp.Save("C:\\temp\\octopus.jpg");
//        }
//    }
//}


//WebClient wc = new WebClient();
//byte[] originalData = wc.DownloadData(url);

//MemoryStream stream = new MemoryStream(originalData);
//Bitmap Bitmap = new Bitmap(stream);




//"//upload.wikimedia.org/wikipedia/commons/thumb/0/09/Voyager_2_picture_of_Oberon.jpg/220px-Voyager_2_picture_of_Oberon.jpg"


/*
 * 
 * 
 *  BitmapImage b = new BitmapImage();
b.BeginInit();
b.UriSource = new Uri("c:\\plus.png");
b.EndInit();




 * Image image = new Image();
using (MemoryStream stream = new MemoryStream(byteArray))
{
image.Source = BitmapFrame.Create(stream,
                          BitmapCreateOptions.None,
                          BitmapCacheOption.OnLoad);
}



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



public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
{
System.Drawing.Image image = null;

try
{
System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
webRequest.AllowWriteStreamBuffering = true;
webRequest.Timeout = 30000;

System.Net.WebResponse webResponse = webRequest.GetResponse();

System.IO.Stream stream = webResponse.GetResponseStream();

image = System.Drawing.Image.FromStream(stream);

webResponse.Close();
}
catch (Exception ex)
{
return null;
}

return image;
}
rotected void btnSave_Click(object sender, EventArgs e)
{
System.Drawing.Image image = DownloadImageFromUrl(txtUrl.Text.Trim());

string rootPath = @"C:\DownloadedImageFromUrl";
string fileName = System.IO.Path.Combine(rootPath, "test.gif");
image.Save(fileName);
}



using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

public class DownloadImage {
private string imageUrl;
private Bitmap bitmap;
public DownloadImage(string imageUrl) {
this.imageUrl = imageUrl;
}
public void Download() {
try {
WebClient client = new WebClient();
Stream stream = client.OpenRead(imageUrl);
bitmap = new Bitmap(stream);
stream.Flush();
stream.Close();
}
catch (Exception e) {
Console.WriteLine(e.Message);
}
}
public Bitmap GetImage() {
return bitmap;
}
public void SaveImage(string filename, ImageFormat format) {
if (bitmap != null) {
bitmap.Save(filename, format);
}
}
}



string url = "http://resources.printofast.com/media/bdccb931-1599-4eba-817d-6b3968ab90ee.jpg";
WebClient webClient = new WebClient();
string path = Server.MapPath("Images") ; // Create a folder named 'Images' in your root directory
string fileName = Path.GetFileName(url);
webClient.DownloadFile(url, path + "\\" + fileName);



using (WebClient client = new WebClient()) 
{
client.DownloadFile(new Uri(url), @"c:\temp\image35.png");

//OR 

client.DownloadFileAsync(new Uri(url), @"c:\temp\image35.png");
}



public void SaveImage(string filename, ImageFormat format) {

WebClient client = new WebClient();
Stream stream = client.OpenRead(imageUrl);
Bitmap bitmap;  bitmap = new Bitmap(stream);

if (bitmap != null) 
bitmap.Save(filename, format);

stream.Flush();
stream.Close();
client.Dispose();
}




byte[] imageData = DownloadData(Url); //DownloadData function from here
MemoryStream stream = new MemoryStream(imageData);
Image img = Image.FromStream(stream);
stream.Close();




Image tmpimg = null;
HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
Stream stream = httpWebReponse.GetResponseStream();
return Image.FromStream(stream);


using (WebClient client = new WebClient()) {
    client.DownloadFile("http://www.mywebsite.com/img.aspx?imgid=12345", "selectedfile.gif");
}






public class ImageDownloader
{
public void DownloadImagesFromUrl(string url, string folderImagesPath)
{
var uri = new Uri(url + "/?per_page=50");
var pages = new List<HtmlNode> { LoadHtmlDocument(uri) };

pages.AddRange(LoadOtherPages(pages[0], url));

pages.SelectMany(p => p.SelectNodes("//a[@class='catalog__displayedItem__columnFotomainLnk']/img"))
 .Select(node => Tuple.Create(new UriBuilder(uri.Scheme, uri.Host, uri.Port, node.Attributes["src"].Value).Uri, new WebClient()))
 .AsParallel()
 .ForAll(t => DownloadImage(folderImagesPath, t.Item1, t.Item2));
}

private static void DownloadImage(string folderImagesPath, Uri url, WebClient webClient)
{
try
{
webClient.DownloadFile(url, Path.Combine(folderImagesPath, Path.GetFileName(url.ToString())));
}
catch (Exception ex)
{
Console.WriteLine(ex.Message);
}
}

private static IEnumerable<HtmlNode> LoadOtherPages(HtmlNode firstPage, string url)
{
return Enumerable.Range(1, DiscoverTotalPages(firstPage))
             .AsParallel()
             .Select(i => LoadHtmlDocument(new Uri(url + "/?per_page=50&page=" + i)));
}

private static int DiscoverTotalPages(HtmlNode documentNode)
{
var totalItemsDescription = documentNode.SelectNodes("//div[@class='catalogItemList__numsInWiev']").First().InnerText.Trim();
var totalItems = int.Parse(Regex.Match(totalItemsDescription, @"\d+$").ToString());
var totalPages = (int)Math.Ceiling(totalItems / 50d);
return totalPages;
}

private static HtmlNode LoadHtmlDocument(Uri uri)
{
var doc = new HtmlDocument();
var wc = new WebClient();
doc.LoadHtml(wc.DownloadString(uri));

var documentNode = doc.DocumentNode;
return documentNode;
}
}

DownloadImagesFromUrl("http://www.onlinetrade.ru/catalogue/televizori-c181/", @"C:\temp\televizori-c181\images");






<img alt="Voyager 2 picture of Oberon.jpg" src="//upload.wikimedia.org/wikipedia/commons/thumb/0/09/Voyager_2_picture_of_Oberon.jpg/220px-Voyager_2_picture_of_Oberon.jpg" 
width="220" height="220" srcset="//upload.wikimedia.org/wikipedia/commons/thumb/0/09/Voyager_2_picture_of_Oberon.jpg/330px-Voyager_2_picture_of_Oberon.jpg 1.5x, 
//upload.wikimedia.org/wikipedia/commons/thumb/0/09/Voyager_2_picture_of_Oberon.jpg/440px-Voyager_2_picture_of_Oberon.jpg 2x" data-file-width="640" data-file-height="640">

*/






