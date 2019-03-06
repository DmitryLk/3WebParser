using HtmlAgilityPack;
using System;
using System.Collections.Generic;


namespace WebParser.Data
{
    public class SpaceObjectDocument
    {
        private readonly IParser _parser;

        public HtmlDocument Document { get; set; }

        //================================================================================================================

        public SpaceObjectDocument(IParser parser, HtmlDocument document)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }


        public SpaceObjectDocument(IParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }
        //================================================================================================================
        public bool IsTargetPage
        {
            get {
                int i = 0;
                var keywordsList = new List<string> { "Eccentricity", "Volume", "Mass", "Orbital", "Temperature", "Semi-major", "anomaly" };

                foreach (var keyword in keywordsList)
                {
                    if (_parser.FindNodesByTagAndInnerText(Document, keyword) != null) i++;
                }
                return i >= 3;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsDisambugationPage
        {
            get {
                return _parser.FindNodesByTagAndInnerText(Document, "Disambiguation pages") != null;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsLinkToDisambugationPage
        {
            get {
                return _parser.FindNodesByTagAndInnerText(Document, "(disambiguation)") != null;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsLinkToTargetPage
        {
            get {
                if (_parser.FindNodesByTagAndInnerText(Document, "(moon)") != null) return true;
                if (_parser.FindNodesByTagAndInnerText(Document, "(planet)") != null) return true;
                if (_parser.FindNodesByTagAndInnerText(Document, "(asteroid)") != null) return true;
                if (_parser.FindNodesByTagAndInnerText(Document, "(comet)") != null) return true;
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsResultNotFoundPage
        {
            get {
                return _parser.FindNodesByTagAndInnerText(Document, "К сожалению, по вашему запросу ничего не найдено...") != null;
            }
        }
        //================================================================================================================
    }
}
















