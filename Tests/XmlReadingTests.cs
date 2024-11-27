using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tests;
using System.Xml;
using System.Xml.Linq;
using Parsings;
using UnityEngine;
using static Parsings.ParF;

namespace Tests
{
    public class XmlReadingTests
    {
        [Test]
        public void HtmlRead()
        {
            var html = Utility.ReadString("html.html");
            Assert.That(html, Is.Not.Empty);
        }

        [Test]
        public void HtmlInnerText()
        {
            var html  = Utility.ReadString("html.html");
            var doc   = XDocument.Parse(html);
            var style = doc.Descendants("style").First().Value;
            Debug.Log(style);
            Assert.That(style, Is.Not.Empty);
        }

        [Test]
        public void StyleParse()
        {
            CssClass clss =
                Utility.ReadString("html.html")
                   .Pipe(XDocument.Parse)
                   .Apply(it => it.Descendants("style").First().Value)
                   .Pipe(TrimStart).Pipe(GetClass).value;

            Debug.Log(clss.name);
            clss.props.Iter(p => Debug.Log(p));
            Assert.That(clss.name, Is.Not.Empty);
        }
    }
}