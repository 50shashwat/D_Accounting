using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace D_Accounting
{
    internal class ListCasesXmlReaderParser
    {
        private XmlReader reader = null;

        public ListCasesXmlReaderParser(string filePath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;

            reader = XmlReader.Create(filePath, settings);
        }

        public ListCases Read()
        {
            return new ListCases(reader);
        }

    }
}
