using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebParser.Data
{

    public enum PageTypes
    {
        TargetPage,
        LinkToSpacePage,

        DisambugationPage,
        LinkToDisambugationPage,

        NotIdentifiedPage,
        AnotherDomen,
        //IsPageDoesNotExist,
        ResultNotFoundPage,
        UnknownPage
    }

    public class HtmlUriDocument
    {
        public HtmlDocument Document { get; set; }
        public Uri Uri { get; set; }
        public PageTypes pageType;
    }
}








