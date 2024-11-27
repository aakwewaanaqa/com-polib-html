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
            var html  = Utility.ReadString("html.html");
            var doc   = XDocument.Parse(html);
            var style = doc.Descendants("style").First().Value;

            string clssName = "";
            var    props    = new List<CssProp>();

            style.Pipe(TrimStart).Pipe(GetClassName)
               .Inject(opt => clssName = opt.value)
               .remaining.Pipe(TrimStart).Pipe(GetScope)
               .Inject(opt =>
                {
                    var src = opt.value;
                    for (;;)
                    {
                        var x = src.Pipe(TrimStart).Pipe(GetProperty);
                        if (x.isRejected) break;
                        props.Add(x.value);
                        src = x.remaining;
                    }
                })
                ;

            Debug.Log(clssName);
            props.ForEach(p => Debug.Log(p));
            Assert.That(clssName, Is.Not.Empty);
        }
    }
}