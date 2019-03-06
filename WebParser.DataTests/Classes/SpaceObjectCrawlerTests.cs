using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebParser.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NLog;
using HtmlAgilityPack;

namespace WebParser.Data.Tests
{
    [TestClass()]
    public class SpaceObjectCrawlerTests
    {


        //public async Task<SpaceObjectDocument> GetInitialPage(string spaceObjectName)
        //{
        //    var uri = new Uri("/w/index.php?search=" + spaceObjectName + "&title=Special%3ASearch&go=Go", UriKind.Relative);
        //    var document = await _parser.GetHtmlDocumentByUri(uri);
        //    return new SpaceObjectDocument(_parser, document);
        //}
        //Task.FromResult

        [TestMethod()]
        public void SumTest()
        {
            //Arrange
            var mock = new Mock<IParser>();
            var crawler = new SpaceObjectCrawler(mock.Object, LogManager.GetCurrentClassLogger());

            int expected = 5;
            //Act
            int actual = crawler.MySum(2, 3);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public async void GetInitialPageTest()
        {
            //Arrange
            var hd = new HtmlDocument();
            var mock = new Mock<IParser>();
            mock.Setup(m => m.GetHtmlDocumentByUriAsync(It.IsAny<Uri>()))
            //.Returns(Task.FromResult(new HtmlDocument()));
            .ReturnsAsync(hd);

            SpaceObjectCrawler crawler = new SpaceObjectCrawler(mock.Object, LogManager.GetCurrentClassLogger());
            var expected = hd;

            //Act
            var actual = await crawler.GetInitialPage("Sun");

            //Assert
            Assert.AreEqual(expected, actual.Document);
        }

      
    }
}