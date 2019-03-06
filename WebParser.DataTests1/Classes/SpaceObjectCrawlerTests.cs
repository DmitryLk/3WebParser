using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebParser.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using HtmlAgilityPack;
using NLog;

namespace WebParser.Data.Tests
{
    [TestClass()]
    public class SpaceObjectCrawlerTests
    {

        //public async Task<SpaceObjectDocument> GetInitialPage(string spaceObjectName)
        //{
        //    var uri = new Uri("/w/index.php?search=" + spaceObjectName + "&title=Special%3ASearch&go=Go", UriKind.Relative);
        //    var document = await _parser.GetHtmlDocumentByUriAsync(uri);
        //    return new SpaceObjectDocument(_parser, document);
        //}


        [TestMethod()]
        public async Task GetInitialPageTest()
        {
            //Arrange
            var htmldoc = new HtmlDocument();

            Mock<IParser> mock = new Mock<IParser>();
            mock.Setup(m => m.GetHtmlDocumentByUriAsync(It.IsAny<Uri>()))
                .ReturnsAsync(htmldoc);

            SpaceObjectCrawler crawler = new SpaceObjectCrawler(mock.Object, LogManager.GetCurrentClassLogger());
            var expected = htmldoc;

            //Act
            var actual = await crawler.GetInitialPage("Sun");

            //Assert
            Assert.AreEqual(expected, actual.Document);
        }
    }
}